Shader "Unlit/TwoSideSprite"
{
    Properties
    {
        _FrontTex ("TextureFront", 2D) = "white" {}
		_BuckTex("TextureBuck", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
		Cull off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _FrontTex;
            sampler2D _BuckTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			fixed4 frag(v2f i, fixed facing : VFACE) : SV_Target
			{
				return (facing > 0) ? tex2D(_FrontTex, i.uv) : tex2D(_BuckTex, i.uv);
			}
		ENDCG
        }
    }
}
