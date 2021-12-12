
Shader "Custom/Barrier"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_MainColor("Main Color", Color) = (1,1,1,1) // モデルの色
		_MainAlpha("MainAlpha", float) = 0.0
		_RimColor("RimColor", Color) = (1,1,1,1)
		_RimAlpha("RimAlpha", float) = 0.0
		_DissolveColor("DissolveColor", Color) = (1,1,1,1)
		_CutOff("CutOff Range", Range(0,1)) = 0     // 分解のしきい値
		_Width("Width", Range(0,1)) = 0.001         // しきい値の幅
		_ColorIntensity("Intensity", Float) = 1     // 燃え尽きる部分の明るさの強度（Bloom+HDRを使わない場合は不要）
		_Roughness("Roughness", Float) = 0.01
		_Octave("Octave", INT) = 4
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
		Cull off
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100
		ZWrite OFF

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

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
				//float3 normal : NORMAL;
				float4 vertex : SV_POSITION;
				float3 viewDir : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
			};

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			half _RimAlpha;
			half _MainAlpha;
			fixed4 _MainColor;
			fixed4 _RimColor;
			fixed4 _DissolveColor;
			fixed _CutOff;
			fixed _Width;
			fixed _ColorIntensity;
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
					value += amplitube * perlinNoise3(vec + offset);
					vec *= 2.0;
					amplitube *= 0.5;

				}
				return value;
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normalDir = normalize(mul(unity_ObjectToWorld, v.normal)).xyz;
				o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld,v.vertex).xyz);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _MainColor;
				
				// パーリンノイズ
				fixed perlin = fbm3(i.vertex.xyz*_Roughness, _Octave);

				// 燃える切れ端表現（aの値を、しきい値～しきい値＋幅の範囲を0～1として丸める）
				fixed a = i.uv.y+perlin+ _Width;
				a = pow(a, 0.5);
				fixed b = smoothstep(_CutOff, _CutOff + _Width, a);
				col.rgb += _DissolveColor.rgb * b * _ColorIntensity;

				// リムライティング
				half alpha = 1.0 - abs(dot(i.viewDir, i.normalDir));
				alpha = clamp(alpha * _RimAlpha, 0.1, 1.0);
				col.rgb += alpha* _RimColor.rgb;

				// 消失する範囲を求める　(_CutOff + _Width * 2.0 >= a) ? 1 : 0
				fixed b2 = step(a, _CutOff + _Width*2.0);
				col.a = b2 *alpha * _MainAlpha;

				// 波のアニメーション ( 0.0～1.0 )
				const half PI = 3.14159;
				half uv_y = 1.0 - i.uv.y;
				float amp = 1.0-saturate(0.5 - sin(_Time*100.0 + uv_y *PI)*0.5-0.99)*10.0;
				float amp2 = 1.0-saturate(0.5 - sin(_Time*100.0 + uv_y*PI+ PI/2.0)*0.5-0.99)*10.0;
				float amp3 = 1.0-saturate(0.5 - sin(_Time*100.0 + uv_y*PI+ PI)*0.5-0.99)*10.0;
				float amp4 = 1.0-saturate(0.5 - sin(_Time*100.0 + uv_y*PI+ PI*3/2)*0.5-0.99)*10.0;
				col.rgb *= amp*amp2*amp3*amp4;

				return col;
			}
			ENDCG
		}
	}
}