Shader "Unlit/BackgroundShader"
{
    Properties
    {
        _Frame ("Frame", 2D) = "white" {}
        _AlphaThreshold ("Alpha Theshold", float) = 0.9
    }
    SubShader
    {
        Tags 
        {
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }

        Pass
        {
            Cull Off
            Blend One OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _Frame;
            float4 _Frame_ST;

            float _AlphaThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Frame);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_Frame, i.uv);

                if (col.w > _AlphaThreshold)
                {
                    col.w = 0;
                }


                return col;
            }
            ENDCG
        }
    }
}
