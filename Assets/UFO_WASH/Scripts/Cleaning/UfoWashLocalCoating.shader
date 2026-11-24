Shader "UFO WASH/Local Coating"
{
    Properties { _DirtColor("Contamination", Color) = (0.48,0.30,0.16,1) }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "Queue"="Transparent+10" "RenderType"="Transparent" }
        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            CBUFFER_START(UnityPerMaterial)
                half4 _DirtColor;
            CBUFFER_END
            struct Attributes { float4 positionOS:POSITION; float3 normalOS:NORMAL; half4 color:COLOR; };
            struct Varyings { float4 positionCS:SV_POSITION; float3 positionWS:TEXCOORD0; half3 normalWS:TEXCOORD1; half4 state:COLOR; };
            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.state = input.color;
                return output;
            }
            half4 Frag(Varyings input):SV_Target
            {
                // World-space grain needs no UVs and gives dirt a different surface
                // character from the pale, persistent foam layer.
                float grain = frac(sin(dot(floor(input.positionWS * 65), float3(12.9898,78.233,37.719))) * 43758.5453);
                half dirt = saturate(input.state.r);
                half foam = saturate(input.state.g);
                half3 baseColor = lerp(_DirtColor.rgb * lerp(0.65,1.15,grain), half3(0.83,0.96,0.98), foam);
                Light light = GetMainLight();
                half shade = 0.5 + 0.5 * saturate(dot(normalize(input.normalWS), light.direction));
                return half4(baseColor * shade, saturate(dirt * 1.5 + foam));
            }
            ENDHLSL
        }
    }
}
