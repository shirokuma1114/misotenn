Shader "Unlit/SquareLineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

		_RoadColor("RoadColor",Color) = (1,1,1,1)
		_RoadAlpha("RoadAlpha",Range(0,1.0)) = 0.2
		_GuidColor("GuidColor",Color) = (1,0,0,1)
		_GuidLength("GuidLecgth",Range(0,1.0)) = 0.5
		_TimeScale("TimeScale",float) = 10.0
		_Power("Power",float) = 1.0
	}
		SubShader
		{
			Tags
			{
				"RenderType" = "Opaque"
				"Queue" = "AlphaTest"
			}
			LOD 100
			Blend SrcAlpha OneMinusSrcAlpha
			

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
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float3 viewDir : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _RoadColor;
			float _RoadAlpha;
			fixed4 _GuidColor;
			float _GuidLength;
			float _TimeScale;
			float _Power;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
				o.normalDir = normalize(mul(unity_ObjectToWorld, v.normal)).xyz;
				o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _GuidColor;
					
				col.a = (_Time * _TimeScale - i.uv.x) % 1.0;
				if (col.a < _GuidLength)
				{
					col = fixed4(_RoadColor.rgb, _RoadAlpha);
				}

				/*half alpha = 1.0 - abs(dot(i.viewDir, i.normalDir));
				col = fixed4(col.rgb, alpha);*/
				
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
