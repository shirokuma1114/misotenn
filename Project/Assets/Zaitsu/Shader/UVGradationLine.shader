Shader "Custom/UVGradationLine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_LeftColor("Left Color", Color) = (0.5,0.5,0.5,0.5)
		_RightColor("Right Color", Color) = (0.5,0.5,0.5,0.5)
		_Range("Range Col", Range(0.0, 1.0))=0.5
		_LineColor("Line Color", Color) = (0.0,0.0,0.0,0.0)
		_LineRange("LineRange Col", Range(0.0, 0.25))=0.1
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			LOD 300
			Blend SrcAlpha OneMinusSrcAlpha //重なったオブジェクトの画素の色とのブレンド方法の指定


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

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _LeftColor;
			fixed4 _RightColor;
			fixed4 _LineColor;
			float _Range, _LineRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				if (i.uv.x > 1.0 - _LineRange || i.uv.x< _LineRange || i.uv.y> 1.0 - _LineRange || i.uv.y < _LineRange)
				return _LineColor;
                fixed4 col = tex2D(_MainTex, i.uv)*(_LeftColor*(1.0f- i.uv.x) + _RightColor * i.uv.x);
                return col;

            }
            ENDCG
        }
    }
			Fallback "Diffuse"
}
