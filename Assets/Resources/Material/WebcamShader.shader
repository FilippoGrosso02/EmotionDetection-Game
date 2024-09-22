Shader "Unlit/WebcamShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GreenTintAmount ("Green tint amount", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _GreenTintAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                int2 pixels = int2(i.uv.x * 1024, i.uv.y * 768);

                if (pixels.x % 2 == 0 && pixels.y % 2 == 0)
                {
                    i.uv.x += 0.01;
                }


                fixed4 col = tex2D(_MainTex, i.uv);

                // give green tint:
                col.y *= (1 + _GreenTintAmount);



                return col;
            }
            ENDCG
        }
    }
}
