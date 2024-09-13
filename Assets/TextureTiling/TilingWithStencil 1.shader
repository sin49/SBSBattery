Shader "Shader Graphs/WorldPositionToUVStencilMakeSillouettest"
{
    Properties
    {
        [NoScaleOffset]_Texture("Texture", 2D) = "white" {}
        _scale("scale", Float) = 0
        _Color("Color", Color) = (1, 0.8647798, 0.8647798, 0)
        _Metalic("Metalic", Range(0, 1)) = 0
        _Smoothness("Smoothness", Range(0, 1)) = 0
        _AmbientOcclusion("AmbientOcclusion", Range(0, 1)) = 0
        [HDR]_Emission("Emission", Color) = (0, 0, 0, 0)
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "UniversalMaterialType" = "Lit"
            "Queue"="Geometry"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalLitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
        // Render State
        Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On
           Stencil
{
    Ref 2            // 스텐실 버퍼에 2 기록
    Comp Always      // 항상 통과
    Pass Replace     // 테스트를 통과하면 값을 대체
}
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
        #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
        #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ _LIGHT_LAYERS
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _LIGHT_COOKIES
        #pragma multi_compile _ _FORWARD_PLUS
        #pragma multi_compile _ EVALUATE_SH_MIXED EVALUATE_SH_VERTEX
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #define VARYINGS_NEED_SHADOW_COORD
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_FORWARD
        #define _FOG_FRAGMENT 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
             float4 fogFactorAndVertexLight;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 TangentSpaceNormal;
             float3 AbsoluteWorldSpacePosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV : INTERP0;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV : INTERP1;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh : INTERP2;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord : INTERP3;
            #endif
             float4 tangentWS : INTERP4;
             float4 fogFactorAndVertexLight : INTERP5;
             float3 positionWS : INTERP6;
             float3 normalWS : INTERP7;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS.xyzw = input.tangentWS;
            output.fogFactorAndVertexLight.xyzw = input.fogFactorAndVertexLight;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS = input.tangentWS.xyzw;
            output.fogFactorAndVertexLight = input.fogFactorAndVertexLight.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Texture_TexelSize;
        float _scale;
        float4 _Color;
        float _Metalic;
        float _Smoothness;
        float _AmbientOcclusion;
        float4 _Emission;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Texture);
        SAMPLER(sampler_Texture);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Absolute_float(float In, out float Out)
        {
            Out = abs(In);
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            Rotation = Rotation * (3.1415926f/180.0f);
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_b9ee85e561044ab5b70f57e28b093198_R_1_Float = IN.WorldSpaceNormal[0];
            float _Split_b9ee85e561044ab5b70f57e28b093198_G_2_Float = IN.WorldSpaceNormal[1];
            float _Split_b9ee85e561044ab5b70f57e28b093198_B_3_Float = IN.WorldSpaceNormal[2];
            float _Split_b9ee85e561044ab5b70f57e28b093198_A_4_Float = 0;
            float _Absolute_a3f06fd987c44ab19ab0aa3668af3ce6_Out_1_Float;
            Unity_Absolute_float(_Split_b9ee85e561044ab5b70f57e28b093198_R_1_Float, _Absolute_a3f06fd987c44ab19ab0aa3668af3ce6_Out_1_Float);
            float _Comparison_dba6d9902a954a4d8d71665cb86b9329_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Absolute_a3f06fd987c44ab19ab0aa3668af3ce6_Out_1_Float, float(0.5), _Comparison_dba6d9902a954a4d8d71665cb86b9329_Out_2_Boolean);
            UnityTexture2D _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Texture);
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_A_4_Float = 0;
            float2 _Vector2_efa0ad3cb1634b2fb899f5008c0fcf1d_Out_0_Vector2 = float2(_Split_cdee594cedeb4f4d87cb548bb6bd67ac_G_2_Float, _Split_cdee594cedeb4f4d87cb548bb6bd67ac_B_3_Float);
            float _Property_cf6adbf3bcea454e8a6053de0c6a3c93_Out_0_Float = _scale;
            float2 _Multiply_1f9f12a83fed452c8495c364fd693d3b_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_efa0ad3cb1634b2fb899f5008c0fcf1d_Out_0_Vector2, (_Property_cf6adbf3bcea454e8a6053de0c6a3c93_Out_0_Float.xx), _Multiply_1f9f12a83fed452c8495c364fd693d3b_Out_2_Vector2);
            float2 _Rotate_9f33686b76fd4fb09a7ec56d897f9a4a_Out_3_Vector2;
            Unity_Rotate_Degrees_float(_Multiply_1f9f12a83fed452c8495c364fd693d3b_Out_2_Vector2, float2 (0.5, 0.5), float(-90), _Rotate_9f33686b76fd4fb09a7ec56d897f9a4a_Out_3_Vector2);
            float4 _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.tex, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.samplerstate, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.GetTransformedUV(_Rotate_9f33686b76fd4fb09a7ec56d897f9a4a_Out_3_Vector2) );
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_R_4_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.r;
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_G_5_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.g;
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_B_6_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.b;
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_A_7_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.a;
            float _Split_af3fa9a01ab1452c8086828784a71f08_R_1_Float = IN.WorldSpaceNormal[0];
            float _Split_af3fa9a01ab1452c8086828784a71f08_G_2_Float = IN.WorldSpaceNormal[1];
            float _Split_af3fa9a01ab1452c8086828784a71f08_B_3_Float = IN.WorldSpaceNormal[2];
            float _Split_af3fa9a01ab1452c8086828784a71f08_A_4_Float = 0;
            float _Absolute_f3b41c57a4464b25a6a8fd7b01b54659_Out_1_Float;
            Unity_Absolute_float(_Split_af3fa9a01ab1452c8086828784a71f08_B_3_Float, _Absolute_f3b41c57a4464b25a6a8fd7b01b54659_Out_1_Float);
            float _Comparison_27b5280c3643418ba883c44758f907c5_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Absolute_f3b41c57a4464b25a6a8fd7b01b54659_Out_1_Float, float(0.5), _Comparison_27b5280c3643418ba883c44758f907c5_Out_2_Boolean);
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_A_4_Float = 0;
            float2 _Vector2_8ef44dafcfb54edcbaa52342e6034cc5_Out_0_Vector2 = float2(_Split_a12ef1ba698947c6ae8131ac8169a4a7_R_1_Float, _Split_a12ef1ba698947c6ae8131ac8169a4a7_G_2_Float);
            float _Property_d708c9e67c6f4d48a4a03eb95e476afa_Out_0_Float = _scale;
            float2 _Multiply_b435997128b945d5afac42bb55f27097_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_8ef44dafcfb54edcbaa52342e6034cc5_Out_0_Vector2, (_Property_d708c9e67c6f4d48a4a03eb95e476afa_Out_0_Float.xx), _Multiply_b435997128b945d5afac42bb55f27097_Out_2_Vector2);
            float4 _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.tex, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.samplerstate, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.GetTransformedUV(_Multiply_b435997128b945d5afac42bb55f27097_Out_2_Vector2) );
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_R_4_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.r;
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_G_5_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.g;
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_B_6_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.b;
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_A_7_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.a;
            float _Split_794c6c454f1d410abc00702bf781ed72_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_794c6c454f1d410abc00702bf781ed72_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_794c6c454f1d410abc00702bf781ed72_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_794c6c454f1d410abc00702bf781ed72_A_4_Float = 0;
            float2 _Vector2_76f4e243596e4276981e72e6228dd06d_Out_0_Vector2 = float2(_Split_794c6c454f1d410abc00702bf781ed72_R_1_Float, _Split_794c6c454f1d410abc00702bf781ed72_B_3_Float);
            float _Property_95a8d830ba944e54b6c56cf541ea1962_Out_0_Float = _scale;
            float2 _Multiply_2e1bb0357d3643b9936199eaeedc7e74_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_76f4e243596e4276981e72e6228dd06d_Out_0_Vector2, (_Property_95a8d830ba944e54b6c56cf541ea1962_Out_0_Float.xx), _Multiply_2e1bb0357d3643b9936199eaeedc7e74_Out_2_Vector2);
            float4 _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.tex, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.samplerstate, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.GetTransformedUV(_Multiply_2e1bb0357d3643b9936199eaeedc7e74_Out_2_Vector2) );
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_R_4_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.r;
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_G_5_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.g;
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_B_6_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.b;
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_A_7_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.a;
            float4 _Branch_51a279b5c99e42809ee463fc8b6915f1_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_27b5280c3643418ba883c44758f907c5_Out_2_Boolean, _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4, _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4, _Branch_51a279b5c99e42809ee463fc8b6915f1_Out_3_Vector4);
            float4 _Branch_fb9dddaca1074dcdaef96c4b2c8f8ee1_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_dba6d9902a954a4d8d71665cb86b9329_Out_2_Boolean, _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4, _Branch_51a279b5c99e42809ee463fc8b6915f1_Out_3_Vector4, _Branch_fb9dddaca1074dcdaef96c4b2c8f8ee1_Out_3_Vector4);
            float4 _Property_993730a1a77846f5b4a0167d20dbbc2f_Out_0_Vector4 = _Color;
            float4 _Multiply_a9757b2ec5374ba9ae95e4275c82e034_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Branch_fb9dddaca1074dcdaef96c4b2c8f8ee1_Out_3_Vector4, _Property_993730a1a77846f5b4a0167d20dbbc2f_Out_0_Vector4, _Multiply_a9757b2ec5374ba9ae95e4275c82e034_Out_2_Vector4);
            float4 _Property_74b82bcd3b744222921452effc3c37e4_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_Emission) : _Emission;
            float _Property_60d58a0e87ca4e6e9e0409800aa12fdd_Out_0_Float = _Metalic;
            float _Property_5549af1a206b4a2f8066632ef935f41c_Out_0_Float = _Smoothness;
            float _Property_0768df51b84244af8a38a8c817af7926_Out_0_Float = _AmbientOcclusion;
            surface.BaseColor = (_Multiply_a9757b2ec5374ba9ae95e4275c82e034_Out_2_Vector4.xyz);
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.Emission = (_Property_74b82bcd3b744222921452effc3c37e4_Out_0_Vector4.xyz);
            surface.Metallic = _Property_60d58a0e87ca4e6e9e0409800aa12fdd_Out_0_Float;
            surface.Smoothness = _Property_5549af1a206b4a2f8066632ef935f41c_Out_0_Float;
            surface.Occlusion = _Property_0768df51b84244af8a38a8c817af7926_Out_0_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "GBuffer"
            Tags
            {
                "LightMode" = "UniversalGBuffer"
            }
        
        // Render State
        Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
        #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
        #pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #define VARYINGS_NEED_SHADOW_COORD
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_GBUFFER
        #define _FOG_FRAGMENT 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
             float4 fogFactorAndVertexLight;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 TangentSpaceNormal;
             float3 AbsoluteWorldSpacePosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV : INTERP0;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV : INTERP1;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh : INTERP2;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord : INTERP3;
            #endif
             float4 tangentWS : INTERP4;
             float4 fogFactorAndVertexLight : INTERP5;
             float3 positionWS : INTERP6;
             float3 normalWS : INTERP7;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS.xyzw = input.tangentWS;
            output.fogFactorAndVertexLight.xyzw = input.fogFactorAndVertexLight;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS = input.tangentWS.xyzw;
            output.fogFactorAndVertexLight = input.fogFactorAndVertexLight.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Texture_TexelSize;
        float _scale;
        float4 _Color;
        float _Metalic;
        float _Smoothness;
        float _AmbientOcclusion;
        float4 _Emission;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Texture);
        SAMPLER(sampler_Texture);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Absolute_float(float In, out float Out)
        {
            Out = abs(In);
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            Rotation = Rotation * (3.1415926f/180.0f);
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_b9ee85e561044ab5b70f57e28b093198_R_1_Float = IN.WorldSpaceNormal[0];
            float _Split_b9ee85e561044ab5b70f57e28b093198_G_2_Float = IN.WorldSpaceNormal[1];
            float _Split_b9ee85e561044ab5b70f57e28b093198_B_3_Float = IN.WorldSpaceNormal[2];
            float _Split_b9ee85e561044ab5b70f57e28b093198_A_4_Float = 0;
            float _Absolute_a3f06fd987c44ab19ab0aa3668af3ce6_Out_1_Float;
            Unity_Absolute_float(_Split_b9ee85e561044ab5b70f57e28b093198_R_1_Float, _Absolute_a3f06fd987c44ab19ab0aa3668af3ce6_Out_1_Float);
            float _Comparison_dba6d9902a954a4d8d71665cb86b9329_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Absolute_a3f06fd987c44ab19ab0aa3668af3ce6_Out_1_Float, float(0.5), _Comparison_dba6d9902a954a4d8d71665cb86b9329_Out_2_Boolean);
            UnityTexture2D _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Texture);
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_A_4_Float = 0;
            float2 _Vector2_efa0ad3cb1634b2fb899f5008c0fcf1d_Out_0_Vector2 = float2(_Split_cdee594cedeb4f4d87cb548bb6bd67ac_G_2_Float, _Split_cdee594cedeb4f4d87cb548bb6bd67ac_B_3_Float);
            float _Property_cf6adbf3bcea454e8a6053de0c6a3c93_Out_0_Float = _scale;
            float2 _Multiply_1f9f12a83fed452c8495c364fd693d3b_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_efa0ad3cb1634b2fb899f5008c0fcf1d_Out_0_Vector2, (_Property_cf6adbf3bcea454e8a6053de0c6a3c93_Out_0_Float.xx), _Multiply_1f9f12a83fed452c8495c364fd693d3b_Out_2_Vector2);
            float2 _Rotate_9f33686b76fd4fb09a7ec56d897f9a4a_Out_3_Vector2;
            Unity_Rotate_Degrees_float(_Multiply_1f9f12a83fed452c8495c364fd693d3b_Out_2_Vector2, float2 (0.5, 0.5), float(-90), _Rotate_9f33686b76fd4fb09a7ec56d897f9a4a_Out_3_Vector2);
            float4 _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.tex, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.samplerstate, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.GetTransformedUV(_Rotate_9f33686b76fd4fb09a7ec56d897f9a4a_Out_3_Vector2) );
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_R_4_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.r;
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_G_5_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.g;
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_B_6_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.b;
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_A_7_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.a;
            float _Split_af3fa9a01ab1452c8086828784a71f08_R_1_Float = IN.WorldSpaceNormal[0];
            float _Split_af3fa9a01ab1452c8086828784a71f08_G_2_Float = IN.WorldSpaceNormal[1];
            float _Split_af3fa9a01ab1452c8086828784a71f08_B_3_Float = IN.WorldSpaceNormal[2];
            float _Split_af3fa9a01ab1452c8086828784a71f08_A_4_Float = 0;
            float _Absolute_f3b41c57a4464b25a6a8fd7b01b54659_Out_1_Float;
            Unity_Absolute_float(_Split_af3fa9a01ab1452c8086828784a71f08_B_3_Float, _Absolute_f3b41c57a4464b25a6a8fd7b01b54659_Out_1_Float);
            float _Comparison_27b5280c3643418ba883c44758f907c5_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Absolute_f3b41c57a4464b25a6a8fd7b01b54659_Out_1_Float, float(0.5), _Comparison_27b5280c3643418ba883c44758f907c5_Out_2_Boolean);
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_A_4_Float = 0;
            float2 _Vector2_8ef44dafcfb54edcbaa52342e6034cc5_Out_0_Vector2 = float2(_Split_a12ef1ba698947c6ae8131ac8169a4a7_R_1_Float, _Split_a12ef1ba698947c6ae8131ac8169a4a7_G_2_Float);
            float _Property_d708c9e67c6f4d48a4a03eb95e476afa_Out_0_Float = _scale;
            float2 _Multiply_b435997128b945d5afac42bb55f27097_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_8ef44dafcfb54edcbaa52342e6034cc5_Out_0_Vector2, (_Property_d708c9e67c6f4d48a4a03eb95e476afa_Out_0_Float.xx), _Multiply_b435997128b945d5afac42bb55f27097_Out_2_Vector2);
            float4 _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.tex, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.samplerstate, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.GetTransformedUV(_Multiply_b435997128b945d5afac42bb55f27097_Out_2_Vector2) );
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_R_4_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.r;
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_G_5_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.g;
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_B_6_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.b;
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_A_7_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.a;
            float _Split_794c6c454f1d410abc00702bf781ed72_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_794c6c454f1d410abc00702bf781ed72_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_794c6c454f1d410abc00702bf781ed72_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_794c6c454f1d410abc00702bf781ed72_A_4_Float = 0;
            float2 _Vector2_76f4e243596e4276981e72e6228dd06d_Out_0_Vector2 = float2(_Split_794c6c454f1d410abc00702bf781ed72_R_1_Float, _Split_794c6c454f1d410abc00702bf781ed72_B_3_Float);
            float _Property_95a8d830ba944e54b6c56cf541ea1962_Out_0_Float = _scale;
            float2 _Multiply_2e1bb0357d3643b9936199eaeedc7e74_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_76f4e243596e4276981e72e6228dd06d_Out_0_Vector2, (_Property_95a8d830ba944e54b6c56cf541ea1962_Out_0_Float.xx), _Multiply_2e1bb0357d3643b9936199eaeedc7e74_Out_2_Vector2);
            float4 _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.tex, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.samplerstate, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.GetTransformedUV(_Multiply_2e1bb0357d3643b9936199eaeedc7e74_Out_2_Vector2) );
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_R_4_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.r;
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_G_5_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.g;
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_B_6_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.b;
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_A_7_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.a;
            float4 _Branch_51a279b5c99e42809ee463fc8b6915f1_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_27b5280c3643418ba883c44758f907c5_Out_2_Boolean, _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4, _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4, _Branch_51a279b5c99e42809ee463fc8b6915f1_Out_3_Vector4);
            float4 _Branch_fb9dddaca1074dcdaef96c4b2c8f8ee1_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_dba6d9902a954a4d8d71665cb86b9329_Out_2_Boolean, _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4, _Branch_51a279b5c99e42809ee463fc8b6915f1_Out_3_Vector4, _Branch_fb9dddaca1074dcdaef96c4b2c8f8ee1_Out_3_Vector4);
            float4 _Property_993730a1a77846f5b4a0167d20dbbc2f_Out_0_Vector4 = _Color;
            float4 _Multiply_a9757b2ec5374ba9ae95e4275c82e034_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Branch_fb9dddaca1074dcdaef96c4b2c8f8ee1_Out_3_Vector4, _Property_993730a1a77846f5b4a0167d20dbbc2f_Out_0_Vector4, _Multiply_a9757b2ec5374ba9ae95e4275c82e034_Out_2_Vector4);
            float4 _Property_74b82bcd3b744222921452effc3c37e4_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_Emission) : _Emission;
            float _Property_60d58a0e87ca4e6e9e0409800aa12fdd_Out_0_Float = _Metalic;
            float _Property_5549af1a206b4a2f8066632ef935f41c_Out_0_Float = _Smoothness;
            float _Property_0768df51b84244af8a38a8c817af7926_Out_0_Float = _AmbientOcclusion;
            surface.BaseColor = (_Multiply_a9757b2ec5374ba9ae95e4275c82e034_Out_2_Vector4.xyz);
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.Emission = (_Property_74b82bcd3b744222921452effc3c37e4_Out_0_Vector4.xyz);
            surface.Metallic = _Property_60d58a0e87ca4e6e9e0409800aa12fdd_Out_0_Float;
            surface.Smoothness = _Property_5549af1a206b4a2f8066632ef935f41c_Out_0_Float;
            surface.Occlusion = _Property_0768df51b84244af8a38a8c817af7926_Out_0_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRGBufferPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Texture_TexelSize;
        float _scale;
        float4 _Color;
        float _Metalic;
        float _Smoothness;
        float _AmbientOcclusion;
        float4 _Emission;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Texture);
        SAMPLER(sampler_Texture);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        ColorMask R
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Texture_TexelSize;
        float _scale;
        float4 _Color;
        float _Metalic;
        float _Smoothness;
        float _AmbientOcclusion;
        float4 _Emission;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Texture);
        SAMPLER(sampler_Texture);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormals"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALS
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 tangentWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 tangentWS : INTERP0;
             float3 normalWS : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.tangentWS.xyzw = input.tangentWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.tangentWS = input.tangentWS.xyzw;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Texture_TexelSize;
        float _scale;
        float4 _Color;
        float _Metalic;
        float _Smoothness;
        float _AmbientOcclusion;
        float4 _Emission;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Texture);
        SAMPLER(sampler_Texture);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 NormalTS;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.NormalTS = IN.TangentSpaceNormal;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Meta"
            Tags
            {
                "LightMode" = "Meta"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma shader_feature _ EDITOR_VISUALIZATION
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD1
        #define VARYINGS_NEED_TEXCOORD2
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_META
        #define _FOG_FRAGMENT 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
             float4 texCoord1;
             float4 texCoord2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 AbsoluteWorldSpacePosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float4 texCoord2 : INTERP2;
             float3 positionWS : INTERP3;
             float3 normalWS : INTERP4;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.texCoord2.xyzw = input.texCoord2;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.texCoord2 = input.texCoord2.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Texture_TexelSize;
        float _scale;
        float4 _Color;
        float _Metalic;
        float _Smoothness;
        float _AmbientOcclusion;
        float4 _Emission;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Texture);
        SAMPLER(sampler_Texture);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Absolute_float(float In, out float Out)
        {
            Out = abs(In);
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            Rotation = Rotation * (3.1415926f/180.0f);
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_b9ee85e561044ab5b70f57e28b093198_R_1_Float = IN.WorldSpaceNormal[0];
            float _Split_b9ee85e561044ab5b70f57e28b093198_G_2_Float = IN.WorldSpaceNormal[1];
            float _Split_b9ee85e561044ab5b70f57e28b093198_B_3_Float = IN.WorldSpaceNormal[2];
            float _Split_b9ee85e561044ab5b70f57e28b093198_A_4_Float = 0;
            float _Absolute_a3f06fd987c44ab19ab0aa3668af3ce6_Out_1_Float;
            Unity_Absolute_float(_Split_b9ee85e561044ab5b70f57e28b093198_R_1_Float, _Absolute_a3f06fd987c44ab19ab0aa3668af3ce6_Out_1_Float);
            float _Comparison_dba6d9902a954a4d8d71665cb86b9329_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Absolute_a3f06fd987c44ab19ab0aa3668af3ce6_Out_1_Float, float(0.5), _Comparison_dba6d9902a954a4d8d71665cb86b9329_Out_2_Boolean);
            UnityTexture2D _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Texture);
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_A_4_Float = 0;
            float2 _Vector2_efa0ad3cb1634b2fb899f5008c0fcf1d_Out_0_Vector2 = float2(_Split_cdee594cedeb4f4d87cb548bb6bd67ac_G_2_Float, _Split_cdee594cedeb4f4d87cb548bb6bd67ac_B_3_Float);
            float _Property_cf6adbf3bcea454e8a6053de0c6a3c93_Out_0_Float = _scale;
            float2 _Multiply_1f9f12a83fed452c8495c364fd693d3b_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_efa0ad3cb1634b2fb899f5008c0fcf1d_Out_0_Vector2, (_Property_cf6adbf3bcea454e8a6053de0c6a3c93_Out_0_Float.xx), _Multiply_1f9f12a83fed452c8495c364fd693d3b_Out_2_Vector2);
            float2 _Rotate_9f33686b76fd4fb09a7ec56d897f9a4a_Out_3_Vector2;
            Unity_Rotate_Degrees_float(_Multiply_1f9f12a83fed452c8495c364fd693d3b_Out_2_Vector2, float2 (0.5, 0.5), float(-90), _Rotate_9f33686b76fd4fb09a7ec56d897f9a4a_Out_3_Vector2);
            float4 _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.tex, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.samplerstate, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.GetTransformedUV(_Rotate_9f33686b76fd4fb09a7ec56d897f9a4a_Out_3_Vector2) );
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_R_4_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.r;
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_G_5_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.g;
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_B_6_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.b;
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_A_7_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.a;
            float _Split_af3fa9a01ab1452c8086828784a71f08_R_1_Float = IN.WorldSpaceNormal[0];
            float _Split_af3fa9a01ab1452c8086828784a71f08_G_2_Float = IN.WorldSpaceNormal[1];
            float _Split_af3fa9a01ab1452c8086828784a71f08_B_3_Float = IN.WorldSpaceNormal[2];
            float _Split_af3fa9a01ab1452c8086828784a71f08_A_4_Float = 0;
            float _Absolute_f3b41c57a4464b25a6a8fd7b01b54659_Out_1_Float;
            Unity_Absolute_float(_Split_af3fa9a01ab1452c8086828784a71f08_B_3_Float, _Absolute_f3b41c57a4464b25a6a8fd7b01b54659_Out_1_Float);
            float _Comparison_27b5280c3643418ba883c44758f907c5_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Absolute_f3b41c57a4464b25a6a8fd7b01b54659_Out_1_Float, float(0.5), _Comparison_27b5280c3643418ba883c44758f907c5_Out_2_Boolean);
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_A_4_Float = 0;
            float2 _Vector2_8ef44dafcfb54edcbaa52342e6034cc5_Out_0_Vector2 = float2(_Split_a12ef1ba698947c6ae8131ac8169a4a7_R_1_Float, _Split_a12ef1ba698947c6ae8131ac8169a4a7_G_2_Float);
            float _Property_d708c9e67c6f4d48a4a03eb95e476afa_Out_0_Float = _scale;
            float2 _Multiply_b435997128b945d5afac42bb55f27097_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_8ef44dafcfb54edcbaa52342e6034cc5_Out_0_Vector2, (_Property_d708c9e67c6f4d48a4a03eb95e476afa_Out_0_Float.xx), _Multiply_b435997128b945d5afac42bb55f27097_Out_2_Vector2);
            float4 _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.tex, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.samplerstate, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.GetTransformedUV(_Multiply_b435997128b945d5afac42bb55f27097_Out_2_Vector2) );
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_R_4_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.r;
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_G_5_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.g;
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_B_6_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.b;
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_A_7_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.a;
            float _Split_794c6c454f1d410abc00702bf781ed72_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_794c6c454f1d410abc00702bf781ed72_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_794c6c454f1d410abc00702bf781ed72_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_794c6c454f1d410abc00702bf781ed72_A_4_Float = 0;
            float2 _Vector2_76f4e243596e4276981e72e6228dd06d_Out_0_Vector2 = float2(_Split_794c6c454f1d410abc00702bf781ed72_R_1_Float, _Split_794c6c454f1d410abc00702bf781ed72_B_3_Float);
            float _Property_95a8d830ba944e54b6c56cf541ea1962_Out_0_Float = _scale;
            float2 _Multiply_2e1bb0357d3643b9936199eaeedc7e74_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_76f4e243596e4276981e72e6228dd06d_Out_0_Vector2, (_Property_95a8d830ba944e54b6c56cf541ea1962_Out_0_Float.xx), _Multiply_2e1bb0357d3643b9936199eaeedc7e74_Out_2_Vector2);
            float4 _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.tex, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.samplerstate, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.GetTransformedUV(_Multiply_2e1bb0357d3643b9936199eaeedc7e74_Out_2_Vector2) );
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_R_4_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.r;
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_G_5_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.g;
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_B_6_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.b;
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_A_7_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.a;
            float4 _Branch_51a279b5c99e42809ee463fc8b6915f1_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_27b5280c3643418ba883c44758f907c5_Out_2_Boolean, _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4, _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4, _Branch_51a279b5c99e42809ee463fc8b6915f1_Out_3_Vector4);
            float4 _Branch_fb9dddaca1074dcdaef96c4b2c8f8ee1_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_dba6d9902a954a4d8d71665cb86b9329_Out_2_Boolean, _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4, _Branch_51a279b5c99e42809ee463fc8b6915f1_Out_3_Vector4, _Branch_fb9dddaca1074dcdaef96c4b2c8f8ee1_Out_3_Vector4);
            float4 _Property_993730a1a77846f5b4a0167d20dbbc2f_Out_0_Vector4 = _Color;
            float4 _Multiply_a9757b2ec5374ba9ae95e4275c82e034_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Branch_fb9dddaca1074dcdaef96c4b2c8f8ee1_Out_3_Vector4, _Property_993730a1a77846f5b4a0167d20dbbc2f_Out_0_Vector4, _Multiply_a9757b2ec5374ba9ae95e4275c82e034_Out_2_Vector4);
            float4 _Property_74b82bcd3b744222921452effc3c37e4_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_Emission) : _Emission;
            surface.BaseColor = (_Multiply_a9757b2ec5374ba9ae95e4275c82e034_Out_2_Vector4.xyz);
            surface.Emission = (_Property_74b82bcd3b744222921452effc3c37e4_Out_0_Vector4.xyz);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
        
        
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Texture_TexelSize;
        float _scale;
        float4 _Color;
        float _Metalic;
        float _Smoothness;
        float _AmbientOcclusion;
        float4 _Emission;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Texture);
        SAMPLER(sampler_Texture);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Texture_TexelSize;
        float _scale;
        float4 _Color;
        float _Metalic;
        float _Smoothness;
        float _AmbientOcclusion;
        float4 _Emission;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Texture);
        SAMPLER(sampler_Texture);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Universal 2D"
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
        // Render State
        Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_2D
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 AbsoluteWorldSpacePosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS : INTERP0;
             float3 normalWS : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Texture_TexelSize;
        float _scale;
        float4 _Color;
        float _Metalic;
        float _Smoothness;
        float _AmbientOcclusion;
        float4 _Emission;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Texture);
        SAMPLER(sampler_Texture);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Absolute_float(float In, out float Out)
        {
            Out = abs(In);
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            Rotation = Rotation * (3.1415926f/180.0f);
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_b9ee85e561044ab5b70f57e28b093198_R_1_Float = IN.WorldSpaceNormal[0];
            float _Split_b9ee85e561044ab5b70f57e28b093198_G_2_Float = IN.WorldSpaceNormal[1];
            float _Split_b9ee85e561044ab5b70f57e28b093198_B_3_Float = IN.WorldSpaceNormal[2];
            float _Split_b9ee85e561044ab5b70f57e28b093198_A_4_Float = 0;
            float _Absolute_a3f06fd987c44ab19ab0aa3668af3ce6_Out_1_Float;
            Unity_Absolute_float(_Split_b9ee85e561044ab5b70f57e28b093198_R_1_Float, _Absolute_a3f06fd987c44ab19ab0aa3668af3ce6_Out_1_Float);
            float _Comparison_dba6d9902a954a4d8d71665cb86b9329_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Absolute_a3f06fd987c44ab19ab0aa3668af3ce6_Out_1_Float, float(0.5), _Comparison_dba6d9902a954a4d8d71665cb86b9329_Out_2_Boolean);
            UnityTexture2D _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Texture);
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_cdee594cedeb4f4d87cb548bb6bd67ac_A_4_Float = 0;
            float2 _Vector2_efa0ad3cb1634b2fb899f5008c0fcf1d_Out_0_Vector2 = float2(_Split_cdee594cedeb4f4d87cb548bb6bd67ac_G_2_Float, _Split_cdee594cedeb4f4d87cb548bb6bd67ac_B_3_Float);
            float _Property_cf6adbf3bcea454e8a6053de0c6a3c93_Out_0_Float = _scale;
            float2 _Multiply_1f9f12a83fed452c8495c364fd693d3b_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_efa0ad3cb1634b2fb899f5008c0fcf1d_Out_0_Vector2, (_Property_cf6adbf3bcea454e8a6053de0c6a3c93_Out_0_Float.xx), _Multiply_1f9f12a83fed452c8495c364fd693d3b_Out_2_Vector2);
            float2 _Rotate_9f33686b76fd4fb09a7ec56d897f9a4a_Out_3_Vector2;
            Unity_Rotate_Degrees_float(_Multiply_1f9f12a83fed452c8495c364fd693d3b_Out_2_Vector2, float2 (0.5, 0.5), float(-90), _Rotate_9f33686b76fd4fb09a7ec56d897f9a4a_Out_3_Vector2);
            float4 _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.tex, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.samplerstate, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.GetTransformedUV(_Rotate_9f33686b76fd4fb09a7ec56d897f9a4a_Out_3_Vector2) );
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_R_4_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.r;
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_G_5_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.g;
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_B_6_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.b;
            float _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_A_7_Float = _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4.a;
            float _Split_af3fa9a01ab1452c8086828784a71f08_R_1_Float = IN.WorldSpaceNormal[0];
            float _Split_af3fa9a01ab1452c8086828784a71f08_G_2_Float = IN.WorldSpaceNormal[1];
            float _Split_af3fa9a01ab1452c8086828784a71f08_B_3_Float = IN.WorldSpaceNormal[2];
            float _Split_af3fa9a01ab1452c8086828784a71f08_A_4_Float = 0;
            float _Absolute_f3b41c57a4464b25a6a8fd7b01b54659_Out_1_Float;
            Unity_Absolute_float(_Split_af3fa9a01ab1452c8086828784a71f08_B_3_Float, _Absolute_f3b41c57a4464b25a6a8fd7b01b54659_Out_1_Float);
            float _Comparison_27b5280c3643418ba883c44758f907c5_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Absolute_f3b41c57a4464b25a6a8fd7b01b54659_Out_1_Float, float(0.5), _Comparison_27b5280c3643418ba883c44758f907c5_Out_2_Boolean);
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_a12ef1ba698947c6ae8131ac8169a4a7_A_4_Float = 0;
            float2 _Vector2_8ef44dafcfb54edcbaa52342e6034cc5_Out_0_Vector2 = float2(_Split_a12ef1ba698947c6ae8131ac8169a4a7_R_1_Float, _Split_a12ef1ba698947c6ae8131ac8169a4a7_G_2_Float);
            float _Property_d708c9e67c6f4d48a4a03eb95e476afa_Out_0_Float = _scale;
            float2 _Multiply_b435997128b945d5afac42bb55f27097_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_8ef44dafcfb54edcbaa52342e6034cc5_Out_0_Vector2, (_Property_d708c9e67c6f4d48a4a03eb95e476afa_Out_0_Float.xx), _Multiply_b435997128b945d5afac42bb55f27097_Out_2_Vector2);
            float4 _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.tex, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.samplerstate, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.GetTransformedUV(_Multiply_b435997128b945d5afac42bb55f27097_Out_2_Vector2) );
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_R_4_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.r;
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_G_5_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.g;
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_B_6_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.b;
            float _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_A_7_Float = _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4.a;
            float _Split_794c6c454f1d410abc00702bf781ed72_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_794c6c454f1d410abc00702bf781ed72_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_794c6c454f1d410abc00702bf781ed72_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_794c6c454f1d410abc00702bf781ed72_A_4_Float = 0;
            float2 _Vector2_76f4e243596e4276981e72e6228dd06d_Out_0_Vector2 = float2(_Split_794c6c454f1d410abc00702bf781ed72_R_1_Float, _Split_794c6c454f1d410abc00702bf781ed72_B_3_Float);
            float _Property_95a8d830ba944e54b6c56cf541ea1962_Out_0_Float = _scale;
            float2 _Multiply_2e1bb0357d3643b9936199eaeedc7e74_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_76f4e243596e4276981e72e6228dd06d_Out_0_Vector2, (_Property_95a8d830ba944e54b6c56cf541ea1962_Out_0_Float.xx), _Multiply_2e1bb0357d3643b9936199eaeedc7e74_Out_2_Vector2);
            float4 _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.tex, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.samplerstate, _Property_e8019f7f21bf42ecacb59e83b2019e6c_Out_0_Texture2D.GetTransformedUV(_Multiply_2e1bb0357d3643b9936199eaeedc7e74_Out_2_Vector2) );
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_R_4_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.r;
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_G_5_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.g;
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_B_6_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.b;
            float _SampleTexture2D_f53321c74a084f209f57180c35b97e11_A_7_Float = _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4.a;
            float4 _Branch_51a279b5c99e42809ee463fc8b6915f1_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_27b5280c3643418ba883c44758f907c5_Out_2_Boolean, _SampleTexture2D_c8d44ebbb9ff4d5e9d3b36a053bba7a4_RGBA_0_Vector4, _SampleTexture2D_f53321c74a084f209f57180c35b97e11_RGBA_0_Vector4, _Branch_51a279b5c99e42809ee463fc8b6915f1_Out_3_Vector4);
            float4 _Branch_fb9dddaca1074dcdaef96c4b2c8f8ee1_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_dba6d9902a954a4d8d71665cb86b9329_Out_2_Boolean, _SampleTexture2D_16f1d5efedb2456ca8aa450999d58972_RGBA_0_Vector4, _Branch_51a279b5c99e42809ee463fc8b6915f1_Out_3_Vector4, _Branch_fb9dddaca1074dcdaef96c4b2c8f8ee1_Out_3_Vector4);
            float4 _Property_993730a1a77846f5b4a0167d20dbbc2f_Out_0_Vector4 = _Color;
            float4 _Multiply_a9757b2ec5374ba9ae95e4275c82e034_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Branch_fb9dddaca1074dcdaef96c4b2c8f8ee1_Out_3_Vector4, _Property_993730a1a77846f5b4a0167d20dbbc2f_Out_0_Vector4, _Multiply_a9757b2ec5374ba9ae95e4275c82e034_Out_2_Vector4);
            surface.BaseColor = (_Multiply_a9757b2ec5374ba9ae95e4275c82e034_Out_2_Vector4.xyz);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
        
        
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphLitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}