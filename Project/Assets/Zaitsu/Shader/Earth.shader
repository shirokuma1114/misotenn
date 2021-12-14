Shader "Custom/Earth_Sea" 
{
	Properties{
		_Color_1("Color_1", Color) = (1, 1, 1, 1)
		_Color_2("Color_2", Color) = (1, 1, 1, 1)
	}
		SubShader{
			Tags { "RenderType" = "Transparent" }
			Cull Front
			ZWrite On Blend SrcAlpha OneMinusSrcAlpha
			
		Pass {
				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct v2f {
					float4 pos : SV_POSITION;
					float3 worldPos : TEXCOORD0;
				};

				float4 _Color_1;
				float4 _Color_2;

				v2f vert(appdata_base v)
				{
					v2f o;
					float3 n = UnityObjectToWorldNormal(v.normal);
					o.pos = UnityObjectToClipPos(v.vertex);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					return lerp(_Color_1, _Color_2, i.worldPos.y*0.2);
				}
				ENDCG
			}
	}
		FallBack Off
}