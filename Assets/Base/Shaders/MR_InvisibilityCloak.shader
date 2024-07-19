Shader "Custom/MR_InvisibilityCloak"
{
    Properties {}
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue"="Geometry-1" }
        LOD 100

        Pass
        {
            ZWrite On   // Enable depth writing to ensure the object is considered in depth calculations
            ColorMask 0 // Disable color writes to make the object invisible

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes{  float4 positionOS : POSITION;       };
            struct Varyings{    float4 positionHCS : SV_POSITION;   };

            Varyings vert(Attributes input){
                Varyings output;
                // Transform object space position to homogeneous clip space
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                return output;
            }

            float4 frag(Varyings input) : SV_Target{
                // Fragment shader returns a transparent color, though it won't be written due to ColorMask 0
                return float4(0, 0, 0, 0);
            }
            ENDHLSL
        }
    }
}
