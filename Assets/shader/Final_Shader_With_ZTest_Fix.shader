Shader "Custom/ToonShaderWithLightShadowMetalEmissionOutlinePixelation"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _LightTex ("Light Texture", 2D) = "white" {}
        _ShadowTex ("Shadow Texture", 2D) = "white" {}
        _ToonSteps ("Toon Steps", Range(1, 10)) = 5
        _LightColor ("Light Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 1)
        _ShadowThreshold ("Shadow Threshold", Range(0, 1)) = 0.4
        _Metallic ("Metallic", Range(0, 1)) = 0.0
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _EmissionColor ("Emission Color", Color) = (0, 0, 0, 1)
        
        _PixelationSize ("Pixelation Size", Range(1, 100)) = 10
            _VertexPixelationSize ("Vertex Pixelation Size", Range(1, 100)) = 10
        _FragmentPixelationSize ("Fragment Pixelation Size", Range(1, 100)) = 10
        _ColliderCenter ("Collider Center", Vector) = (0, 0, 0, 0)
        _ColliderSize ("Collider Size", Vector) = (1, 1, 1, 0)
    }

    SubShader
    {


        Tags { "RenderType"="Opaque" }
         Pass
        {
            Name "ForwardLit"
            Tags { "Queue" = "Geometry" "LightMode"="UniversalForward" }
             Cull Off
            ZWrite On
            ZTest LEqual
            HLSLPROGRAM
            #pragma vertex vert
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _LightTex;
            sampler2D _ShadowTex;
            float4 _MainTex_ST;
            float _ToonSteps;
            float _ShadowThreshold;
            float _Metallic;
            float _Smoothness;
            float4 _EmissionColor;
            float4 _LightColor;
            float4 _ShadowColor;
            float3 _ColliderCenter;
            float3 _ColliderSize;
            float _PixelationSize;
             float _VertexPixelationSize;
             float  _FragmentPixelationSize;
Varyings vert(Attributes IN)
{
    Varyings OUT;

    // TransformObjectToWorld�� float3�� ��ȯ�ϹǷ�, w ���� �߰��Ͽ� float4�� ��ȯ
    float3 worldPos3 = TransformObjectToWorld(IN.positionOS);  // ���� ��ǥ (float3)
    float4 worldPos = float4(worldPos3, 1.0);  // float4�� ��ȯ�Ͽ� w ���� 1.0���� ����

    // �浹 ���� Ȯ��
    float3 halfSize = _ColliderSize * 0.5;
    float3 minBound = _ColliderCenter - halfSize;
    float3 maxBound = _ColliderCenter + halfSize;
    bool insideCollider = (worldPos3.x >= minBound.x && worldPos3.x <= maxBound.x) &&
                          (worldPos3.y >= minBound.y && worldPos3.y <= maxBound.y) &&
                          (worldPos3.z >= minBound.z && worldPos3.z <= maxBound.z);

    // ���� ��ǥ�� Ŭ�� �������� ��ȯ
    float4 clipPos = TransformWorldToHClip(worldPos);

    // �浹 ���� �ȿ� ���� ���� �ȼ�ȭ ó��
    if (insideCollider)
    {
        // Ŭ�� ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ (Normalized Device Coordinates)
        float2 screenPos = clipPos.xy / clipPos.w;

        // ��ũ�� ��ǥ�� �ȼ�ȭ ũ��� ���� (�ش����� �ȼ�ȭ)
        screenPos = floor(screenPos * _VertexPixelationSize * 16.0) / (_VertexPixelationSize * 16.0);

        // ��ũ�� ��ǥ�� �ٽ� Ŭ�� �������� ��ȯ (w ���� ����)
        clipPos.xy = screenPos * clipPos.w;
    }

    // ������ Ŭ�� ���� ��ǥ�� ����Ͽ� ���� ��ġ ����
    OUT.positionHCS = clipPos;  // float4�� ����

    OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
    OUT.worldPos = worldPos.xyz;  // float3�� ��ȯ�Ͽ� ����
    OUT.worldNormal = normalize(mul((float3x3)GetObjectToWorldMatrix(), IN.normal));

    return OUT;
}


            half4 frag(Varyings IN) : SV_Target
            {
                // �浹 ���� Ȯ��
                float3 halfSize = _ColliderSize * 0.5;
                float3 minBound = _ColliderCenter - halfSize;
                float3 maxBound = _ColliderCenter + halfSize;
                bool insideCollider = (IN.worldPos.x >= minBound.x && IN.worldPos.x <= maxBound.x) &&
                                      (IN.worldPos.y >= minBound.y && IN.worldPos.y <= maxBound.y) &&
                                      (IN.worldPos.z >= minBound.z && IN.worldPos.z <= maxBound.z);

                // �ȼ�ȭ ó��
                float2 uv = IN.uv;
                if (insideCollider)
                {
                    uv = floor(uv * _FragmentPixelationSize) / _FragmentPixelationSize;
                }

                // URP ���� �Լ� ���
                Light mainLight = GetMainLight();
                half3 lightDir = normalize(mainLight.direction);
                float3 normal = normalize(IN.worldNormal);
                float lightIntensity = max(0.0, dot(normal, lightDir));
                lightIntensity = round(lightIntensity * _ToonSteps) / _ToonSteps;

                // �׸��� ��� ����
                if (lightIntensity < _ShadowThreshold)
                {
                    lightIntensity = 0.0;
                }

                // ���� ���� �ݿ�
                half3 lightColor = mainLight.color.rgb;
                
                // �� �ؽ�ó ����
                half4 lightTexColor = tex2D(_LightTex, uv) * half4(lightColor, 1.0);
                // �׸��� �ؽ�ó ����
                half4 shadowTexColor = tex2D(_ShadowTex, uv) * _ShadowColor;

                // ��� �ܰ迡 ���� �ؽ�ó ����
                half4 finalColor;
                if (lightIntensity > 0.5)
                {
                    finalColor = lightTexColor * _LightColor;
                }
                else
                {
                    finalColor = shadowTexColor;
                }

                // �ݼӼ� �� �Ų����� ���
                float3 reflectDir = reflect(-lightDir, normal);
                float specular = pow(max(dot(reflectDir, lightDir), 0.0), 64.0);
                finalColor.rgb = lerp(finalColor.rgb, specular, _Metallic);

                // Emission ����
                finalColor.rgb += _EmissionColor.rgb;

                return finalColor;
            }

            ENDHLSL
        }
 

       
    }
    FallBack "Diffuse"
}
