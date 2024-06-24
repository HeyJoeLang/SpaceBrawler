Shader "Custom/PlaneHide"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue"="Geometry-1" }
        LOD 100

        Pass
        {
            // Ensure depth write is enabled
            ZWrite On
            // Disable color writes
            ColorMask 0

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                // This fragment shader doesn't need to return any value as ColorMask 0 is set
                return float4(0, 0, 0, 0);
            }
            ENDHLSL
        }
    }
}