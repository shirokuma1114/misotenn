Shader "PostEffect/SH_Contrast"
{
	Properties{
		_MainTex	("MainTex", 2D)				= ""{}
		_Contrast	("Contrast", Range(1, 20))	= 5
	}

	SubShader
	{
		Pass{
			CGPROGRAM
				#include "UnityCg.cginc"
				#pragma vertex vert_img
				#pragma fragment frag

				sampler2D _MainTex;
				float _Contrast;

				float4 frag(v2f_img i) : COLOR
				{
					float4 c = tex2D(_MainTex, i.uv);
					if (i.uv.x > 0)
					{
						c = 1 / (1 + exp(-_Contrast * (c - 0.5)));
					}
					return c;
				}
			ENDCG
		}
	}
}
