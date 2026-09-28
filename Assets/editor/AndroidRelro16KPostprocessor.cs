using System;
using System.IO;
using UnityEditor.Build;
using UnityEditor.Android;
using UnityEngine;

internal sealed class AndroidRelro16KPostprocessor : IPostGenerateGradleAndroidProject
{
    private const uint ElfProgramHeaderGnuRelro = 0x6474e552;
    private const uint ElfProgramHeaderLoad = 1;
    private const ulong PageSize = 0x4000;

    public int callbackOrder => int.MaxValue;

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        var directory = Path.Combine(path, "src", "main", "jniLibs", "arm64-v8a");
        if (!Directory.Exists(directory))
            throw new BuildFailedException("ARM64 native library directory was not generated: " + directory);

        foreach (var library in Directory.GetFiles(directory, "*.so"))
            AlignGnuRelroEnd(library);
    }

    private static void AlignGnuRelroEnd(string path)
    {
        using (var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        using (var reader = new BinaryReader(stream))
        using (var writer = new BinaryWriter(stream))
        {
            if (stream.Length < 64 || reader.ReadByte() != 0x7f || reader.ReadByte() != (byte)'E' ||
                reader.ReadByte() != (byte)'L' || reader.ReadByte() != (byte)'F')
                throw new BuildFailedException("Invalid ELF library: " + path);

            stream.Position = 4;
            if (reader.ReadByte() != 2 || reader.ReadByte() != 1)
                throw new BuildFailedException("Expected a little-endian ELF64 library: " + path);

            stream.Position = 0x20;
            var programHeaderOffset = reader.ReadUInt64();
            stream.Position = 0x36;
            var programHeaderSize = reader.ReadUInt16();
            var programHeaderCount = reader.ReadUInt16();

            if (programHeaderSize < 56 || programHeaderOffset > (ulong)stream.Length ||
                programHeaderOffset + (ulong)programHeaderSize * programHeaderCount > (ulong)stream.Length)
                throw new BuildFailedException("Invalid ELF program header table: " + path);

            ulong nextLoadAddress = ulong.MaxValue;
            ulong relroHeaderOffset = 0;
            ulong relroAddress = 0;
            ulong relroSize = 0;

            for (var index = 0; index < programHeaderCount; index++)
            {
                var headerOffset = programHeaderOffset + (ulong)index * programHeaderSize;
                stream.Position = (long)headerOffset;
                var type = reader.ReadUInt32();
                stream.Position = (long)headerOffset + 16;
                var virtualAddress = reader.ReadUInt64();
                stream.Position = (long)headerOffset + 40;
                var memorySize = reader.ReadUInt64();

                if (type == ElfProgramHeaderGnuRelro)
                {
                    relroHeaderOffset = headerOffset;
                    relroAddress = virtualAddress;
                    relroSize = memorySize;
                }
            }

            if (relroHeaderOffset == 0)
                return;

            var relroEnd = checked(relroAddress + relroSize);
            var alignedEnd = checked((relroEnd + PageSize - 1) & ~(PageSize - 1));
            if (alignedEnd == relroEnd)
                return;

            for (var index = 0; index < programHeaderCount; index++)
            {
                var headerOffset = programHeaderOffset + (ulong)index * programHeaderSize;
                stream.Position = (long)headerOffset;
                var type = reader.ReadUInt32();
                stream.Position = (long)headerOffset + 16;
                var virtualAddress = reader.ReadUInt64();
                if (type == ElfProgramHeaderLoad && virtualAddress >= relroEnd && virtualAddress < nextLoadAddress)
                    nextLoadAddress = virtualAddress;
            }

            if (alignedEnd > nextLoadAddress)
                throw new BuildFailedException("Cannot align GNU_RELRO without overlapping the next LOAD segment: " + path);

            var alignedSize = alignedEnd - relroAddress;
            stream.Position = (long)relroHeaderOffset + 40;
            writer.Write(alignedSize);
            Debug.LogFormat("16KB GNU_RELRO: {0}, MemSiz 0x{1:X} -> 0x{2:X}", Path.GetFileName(path), relroSize, alignedSize);
        }
    }
}
