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

    // TransformObjectToWorld는 float3를 반환하므로, w 값을 추가하여 float4로 변환
    float3 worldPos3 = TransformObjectToWorld(IN.positionOS);  // 월드 좌표 (float3)
    float4 worldPos = float4(worldPos3, 1.0);  // float4로 변환하여 w 값을 1.0으로 설정

    // 충돌 범위 확인
    float3 halfSize = _ColliderSize * 0.5;
    float3 minBound = _ColliderCenter - halfSize;
    float3 maxBound = _ColliderCenter + halfSize;
    bool insideCollider = (worldPos3.x >= minBound.x && worldPos3.x <= maxBound.x) &&
                          (worldPos3.y >= minBound.y && worldPos3.y <= maxBound.y) &&
                          (worldPos3.z >= minBound.z && worldPos3.z <= maxBound.z);

    // 월드 좌표를 클립 공간으로 변환
    float4 clipPos = TransformWorldToHClip(worldPos);

    // 충돌 범위 안에 있을 때만 픽셀화 처리
    if (insideCollider)
    {
        // 클립 공간 좌표를 스크린 좌표로 변환 (Normalized Device Coordinates)
        float2 screenPos = clipPos.xy / clipPos.w;

        // 스크린 좌표를 픽셀화 크기로 적용 (극단적인 픽셀화)
        screenPos = floor(screenPos * _VertexPixelationSize * 16.0) / (_VertexPixelationSize * 16.0);

        // 스크린 좌표를 다시 클립 공간으로 변환 (w 값은 유지)
        clipPos.xy = screenPos * clipPos.w;
    }

    // 변형된 클립 공간 좌표를 사용하여 정점 위치 설정
    OUT.positionHCS = clipPos;  // float4로 전달

    OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
    OUT.worldPos = worldPos.xyz;  // float3로 변환하여 저장
    OUT.worldNormal = normalize(mul((float3x3)GetObjectToWorldMatrix(), IN.normal));

    return OUT;
}


            half4 frag(Varyings IN) : SV_Target
            {
                // 충돌 범위 확인
                float3 halfSize = _ColliderSize * 0.5;
                float3 minBound = _ColliderCenter - halfSize;
                float3 maxBound = _ColliderCenter + halfSize;
                bool insideCollider = (IN.worldPos.x >= minBound.x && IN.worldPos.x <= maxBound.x) &&
                                      (IN.worldPos.y >= minBound.y && IN.worldPos.y <= maxBound.y) &&
                                      (IN.worldPos.z >= minBound.z && IN.worldPos.z <= maxBound.z);

                // 픽셀화 처리
                float2 uv = IN.uv;
                if (insideCollider)
                {
                    uv = floor(uv * _FragmentPixelationSize) / _FragmentPixelationSize;
                }

                // URP 조명 함수 사용
                Light mainLight = GetMainLight();
                half3 lightDir = normalize(mainLight.direction);
                float3 normal = normalize(IN.worldNormal);
                float lightIntensity = max(0.0, dot(normal, lightDir));
                lightIntensity = round(lightIntensity * _ToonSteps) / _ToonSteps;

                // 그림자 경계 조정
                if (lightIntensity < _ShadowThreshold)
                {
                    lightIntensity = 0.0;
                }

                // 빛의 색상 반영
                half3 lightColor = mainLight.color.rgb;
                
                // 빛 텍스처 적용
                half4 lightTexColor = tex2D(_LightTex, uv) * half4(lightColor, 1.0);
                // 그림자 텍스처 적용
                half4 shadowTexColor = tex2D(_ShadowTex, uv) * _ShadowColor;

                // 밝기 단계에 따른 텍스처 선택
                half4 finalColor;
                if (lightIntensity > 0.5)
                {
                    finalColor = lightTexColor * _LightColor;
                }
                else
                {
                    finalColor = shadowTexColor;
                }

                // 금속성 및 매끄러움 계산
                float3 reflectDir = reflect(-lightDir, normal);
                float specular = pow(max(dot(reflectDir, lightDir), 0.0), 64.0);
                finalColor.rgb = lerp(finalColor.rgb, specular, _Metallic);

                // Emission 적용
                finalColor.rgb += _EmissionColor.rgb;

                return finalColor;
            }

            ENDHLSL
        }
 

       
    }
    FallBack "Diffuse"
}
