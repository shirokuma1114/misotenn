Shader "Custom/Cloud"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Alpha("Alpha", float) = 0.0
		_Flow("Flow Speed", Float) = 1.0
		_Roughness("Roughness", Float) = 0.01
		_Octave("Octave", INT) = 4
		_LineColor("Line Color", Color) = (1, 1, 1, 1)
		_LineWidth("Line Width", Range(0.001, 1.0)) = 0.01
		_RimPower("RimPower", float) = 0.0
    }
    SubShader
    {
		//Tags { "RenderType" = "Transparent" "Queue" = "Overlay"}
	    Tags {"Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull off
		// デプスバッファのみにレンダリングする追加パス
		Pass {
			ZWrite On
			ColorMask 0
		}

		
		LOD 100

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
                float4 vertex : SV_POSITION;
				float3 worldPos : WORLD_POS;
				float3 viewDir : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _Color;
			half _Alpha;
			half _Flow;
			half _Roughness;
			int _Octave;

			// 3D→3D疑似乱数
			float3 random33(in float3 vec)
			{
				vec = float3(dot(vec, float3(127.1, 311.7, 245.4)),
					dot(vec, float3(269.5, 183.3, 131.2)),
					dot(vec, float3(522.3, 243.1, 532.4)));

				return frac(sin(vec)*4378.545);
			}

			// パーリンノイズ3D
			float perlinNoise3(in float3 vec)
			{
				float3 ivec = floor(vec);
				float3 fvec = frac(vec);

				float3 a = dot(random33(ivec + float3(0.0, 0.0, 0.0))*2.0 - 1.0, fvec - float3(0.0, 0.0, 0.0));
				float3 b = dot(random33(ivec + float3(1.0, 0.0, 0.0))*2.0 - 1.0, fvec - float3(1.0, 0.0, 0.0));
				float3 c = dot(random33(ivec + float3(0.0, 1.0, 0.0))*2.0 - 1.0, fvec - float3(0.0, 1.0, 0.0));
				float3 d = dot(random33(ivec + float3(1.0, 1.0, 0.0))*2.0 - 1.0, fvec - float3(1.0, 1.0, 0.0));
				float3 e = dot(random33(ivec + float3(0.0, 0.0, 1.0))*2.0 - 1.0, fvec - float3(0.0, 0.0, 1.0));
				float3 f = dot(random33(ivec + float3(1.0, 0.0, 1.0))*2.0 - 1.0, fvec - float3(1.0, 0.0, 1.0));
				float3 g = dot(random33(ivec + float3(0.0, 1.0, 1.0))*2.0 - 1.0, fvec - float3(0.0, 1.0, 1.0));
				float3 h = dot(random33(ivec + float3(1.0, 1.0, 1.0))*2.0 - 1.0, fvec - float3(1.0, 1.0, 1.0));

				fvec = smoothstep(0.0, 1.0, fvec);

				return lerp(lerp(lerp(a, b, fvec.x), lerp(c, d, fvec.x), fvec.y),
					lerp(lerp(e, f, fvec.x), lerp(g, h, fvec.x), fvec.y), fvec.z);
			}

			//非整数ブラウン運動3D（フラクタルパーリンノイズ）
			float fbm3(in float3 vec, int octave, float3 offset = 0.0)
			{
				float value = 0.0;
				float amplitube = 1.0;

				for (int i = 0; i < octave; i++)
				{
					value += amplitube * perlinNoise3(vec+ offset);
					vec *= 2.0;
					amplitube *= 0.5;

				}
				return value;
			}

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex); //ローカル座標系をワールド座標系に変換
				o.normalDir = normalize(mul(unity_ObjectToWorld, v.normal)).xyz;
				o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
				return o;
            }

            fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = _Color;

				half alpha = dot(i.viewDir, i.normalDir);
				alpha = clamp((1.0-alpha) * _Alpha, 0.1, 1.0);

				col = fixed4(col.rgb, alpha);

				if (alpha > 0)alpha = 1;
				else alpha = -1;

				col.a *= fbm3(i.vertex.xyz*_Roughness, _Octave, alpha*_Time.y*_Flow)*1.5;
				return col;
			}
            ENDCG
        }

		//Line
		Pass
		{
			//Back Side
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 viewDir : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _LineWidth;
			fixed4 _LineColor;
			fixed4 _RimColor;
			half _RimPower;

			v2f vert(appdata v)
			{
				//offset方向の計算
				float3 normal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
				float2 offset = TransformViewToProjection(normal.xy);

				//OutLineとしてレンダリングされるポリゴン
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex.xy = o.vertex.xy + offset * _LineWidth;
				o.normalDir = normalize(mul(unity_ObjectToWorld, v.normal)).xyz;
				o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// リムシェーディング	
				half alpha = dot(i.viewDir, -i.normalDir);
				half emission = pow(alpha, _RimPower) * _RimPower;
				emission = saturate(emission);
				fixed4 col = fixed4(_LineColor.rgb,emission);
				return col;
			}
			ENDCG
		}
    }
}
