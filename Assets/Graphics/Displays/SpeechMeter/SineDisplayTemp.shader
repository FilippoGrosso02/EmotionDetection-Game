Shader "Unlit/SineDisplayTemp"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Period ("Period", float) = 1
        _Amplitude ("Amplitude", float) = 1
        _ColorA ("Color A", Color) = (0, 0, 0, 1)
        _ColorB ("Color B", Color) = (0, 0, 0, 1)
        _Thinness ("Thinness", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            float _Period;
            float _Amplitude;

            float4 _ColorA;
            float4 _ColorB;

            float _Thinness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float curve = _Amplitude * sin((_Period * i.uv.x) + (2.0 * _Time.y));

                float lineAShape = smoothstep(1 - clamp(distance(curve + i.uv.y, 0.5) * _Thinness, 0.0, 1.0), 1, 0.99);
                float3  lineACol = (1 - lineAShape) * float3(lerp(_ColorA, _ColorB, lineAShape).xyz);

                /*
                float ySineResult = sin(i.uv.y * _Period);
                float xSineResult = _Amplitude * sin((i.uv.x + ySineResult) * _Period);
                
                float4 col = float4(xSineResult.xxx, 1) * 0.1;
                */

                return float4(lineACol, 1);
            }
            ENDCG
        }
    }
}
