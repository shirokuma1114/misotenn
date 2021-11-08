Shader "Custom/SH_CubeColor"
{
	Properties{
		// テクスチャ
		[Header(MainTexture)] [Space(5)]
		[Toggle(TEXTURE_MAIN)] _OnOff_Texture_Main("ON/OFF MainTexture", Float) = 1.0
		[NoScaleOffset]		_MainTex("Texture", 2D)		= "white" {}
							_Color	("Color", Color)	= (1,1,1,1)
		[Space(15)]

		// CubeColor
		[Header(CubeColor)][Space(5)]
		[Toggle(CUBE_COLOR)] _OnOff_CubeColor("ON/OFF CubeColor", Float) = 1.0

		_TopColor1		("Top 1",    Color) = (1, 1, 1, 1)
		_TopColor2		("Top 2",    Color) = (1, 1, 1, 1)
		[ShowAsVector3] _GradOrigin_T("Gradient Start Pos (Top)", Vector) = (0, 0, 0, 0)
		_GradHeight_T("Gradient Height (Top)", Float) = 0.0
		_GradRotate_T("Gradient Rotation (Top)", Range(0, 360)) = 0.0
		[Space(2)]

		_RightColor1	("Right 1",  Color) = (1, 1, 1, 1)
		_RightColor2	("Right 2",  Color) = (1, 1, 1, 1)
		[ShowAsVector3] _GradOrigin_R("Gradient Start Pos (Right)", Vector) = (0, 0, 0, 0)
		_GradHeight_R("Gradient Height (Right)", Float) = 0.0
		_GradRotate_R("Gradient Rotation (Right)", Range(0, 360)) = 0.0
		[Space(2)]

		_FrontColor1	("Front 1",  Color) = (1, 1, 1, 1)
		_FrontColor2	("Front 2",  Color) = (1, 1, 1, 1)
		[ShowAsVector3] _GradOrigin_F("Gradient Start Pos (Front)", Vector) = (0, 0, 0, 0)
		_GradHeight_F("Gradient Height (Front)", Float) = 0.0
		_GradRotate_F("Gradient Rotation (Front)", Range(0, 360)) = 0.0
		[Space(2)]

		_LeftColor1		("Left 1",   Color) = (1, 1, 1, 1)
		_LeftColor2		("Left 2",   Color) = (1, 1, 1, 1)
		[ShowAsVector3] _GradOrigin_L("Gradient Start Pos (Left)", Vector) = (0, 0, 0, 0)
		_GradHeight_L("Gradient Height (Left)", Float) = 0.0
		_GradRotate_L("Gradient Rotation (Left)", Range(0, 360)) = 0.0
		[Space(2)]

		_BackColor1		("Back 1",   Color) = (1, 1, 1, 1)
		_BackColor2		("Back 2",   Color) = (1, 1, 1, 1)
		[ShowAsVector3] _GradOrigin_B("Gradient Start Pos (Back)", Vector) = (0, 0, 0, 0)
		_GradHeight_B("Gradient Height (Back)", Float) = 0.0
		_GradRotate_B("Gradient Rotation (Back)", Range(0, 360)) = 0.0
		[Space(2)]

		_BottomColor1	("Bottom 1", Color) = (1, 1, 1, 1)
		_BottomColor2	("Bottom 2", Color) = (1, 1, 1, 1)
		[ShowAsVector3] _GradOrigin_D("Gradient Start Pos (Bottom)", Vector) = (0, 0, 0, 0)
		_GradHeight_D("Gradient Height (Bottom)", Float) = 0
		_GradRotate_D("Gradient Rotation (Bottom)", Range(0, 360)) = 0.0
		[Space(15)]

		// RimLight
		[Header(RimLighting)][Space(5)]
		[Toggle(RIM_LIGHT)] _OnOff_RimLight("ON/OFF RimLight", Float) = 1.0
							_RimLight_Color		("Color", Color)	= (1,1,1,1)
							_RimLight_Strength	("Strength", Float) = 2
		[Space(15)]

		// HeightFog
		[Header(HeightFog)] [Space(5)]
		[Toggle(HEIGHT_FOG)] _OnOff_HeightFog ("ON/OFF HeightFog", Float) = 1.0
							_HeightFog_Color			("Color", Color)	= (1, 1, 1, 1)
							_HeightFog_StartDistance	("Distance", Float)	= 50
							_HeightFog_Length			("Length", Float)	= 10
		[Space(15)]

		// Fog
		[Header(Fog)][Space(5)]
		[Toggle(FOG)]		_OnOff_Fog	("ON/OFF Fog", Float) = 1.0
							_Fog_Color	("Color", Color)	= (1, 1, 1, 1)
							_Fog_Near	("Near", Float)		= 0.1
							_Fog_Far	("Far", Float)		= 10
		[Space(15)]

		// ShadowColor
		[Header(ShadowColor)][Space(5)]
		[Toggle(SHADOW_COLOR)]		_OnOff_ShadowColor("ON/OFF ShadowColor", Float) = 1.0
		[Toggle(WITHOUT_SHADOW)]	_OnOff_WithoutShadow("ON/OFF WithoutShadow", Float) = 1.0
							_Shadow_Color	("ShadowColor", Color)	= (1,1,0,1)
							_Shadow_Strength("Strength", Float)		= 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf ColorShadow
        #pragma target 3.0

		#pragma shader_feature  _ TEXTURE_MAIN
		#pragma shader_feature	_ CUBE_COLOR
		#pragma shader_feature  _ RIM_LIGHT
		#pragma shader_feature  _ HEIGHT_FOG
		#pragma shader_feature  _ FOG
		#pragma shader_feature  _ SHADOW_COLOR
		#pragma shader_feature  _ WITHOUT_SHADOW

        struct Input
        {
            float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
			float3 worldPos;
        };

		// MainTexture
		sampler2D	_MainTex;
		fixed4		_Color;

		// CubeColor
		fixed4	_TopColor1;
		fixed4	_TopColor2;
		fixed4	_RightColor1;
		fixed4	_RightColor2;
		fixed4	_FrontColor1;
		fixed4	_FrontColor2;
		fixed4	_LeftColor1;
		fixed4	_LeftColor2;
		fixed4	_BackColor1;
		fixed4	_BackColor2;
		fixed4	_BottomColor1;
		fixed4	_BottomColor2;
		float3	_GradOrigin_T;
		float3	_GradOrigin_R;
		float3	_GradOrigin_F;
		float3	_GradOrigin_L;
		float3	_GradOrigin_B;
		float3	_GradOrigin_D;
		float	_GradHeight_T;
		float	_GradHeight_R;
		float	_GradHeight_F;
		float	_GradHeight_L;
		float	_GradHeight_B;
		float	_GradHeight_D;
		float	_GradRotate_T;
		float	_GradRotate_R;
		float	_GradRotate_F;
		float	_GradRotate_L;
		float	_GradRotate_B;
		float	_GradRotate_D;
		static const half3 VecTop = half3(0, 1, 0);
		static const half3 VecBottom = half3(0, -1, 0);
		static const half3 VecRight = half3(1, 0, 0);
		static const half3 VecFront = half3(0, 0, -1);
		static const half3 VecLeft = half3(-1, 0, 0);
		static const half3 VecBack = half3(0, 0, 1);

		// RimLight
		fixed4 _RimLight_Color;
		float _RimLight_Strength;
		
		// HeightFog
		fixed4 _HeightFog_Color;
		float _HeightFog_StartDistance;
		float _HeightFog_Length;

		// Fog
		fixed4 _Fog_Color;
		float _Fog_Near;
		float _Fog_Far;

		// ShadowColor
		fixed4 _Shadow_Color;
		float _Shadow_Strength;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
			fixed4 albedo = 1;
			float distance_pixel;
			float a, b;

#ifdef TEXTURE_MAIN
			albedo = _Color;//tex2D(_MainTex, IN.uv_MainTex) * _Color;
#endif

#ifdef CUBE_COLOR
			half3 N = IN.worldNormal;
			half dirTop = max(0, dot(N, VecTop));
			half dirBottom = max(0, dot(N, VecBottom));
			half dirRight = max(0, dot(N, VecRight));
			half dirFront = max(0, dot(N, VecFront));
			half dirLeft = max(0, dot(N, VecLeft));
			half dirBack = max(0, dot(N, VecBack));

			half3 pos = IN.worldPos;

			float s, c;

			// Top
			sincos(_GradRotate_T, s, c);
			half rot_T = (pos.x - _GradOrigin_T.x) * -s
				+ (pos.z - _GradOrigin_T.z) * c;
			half grad_T = saturate(rot_T / -_GradHeight_T);
			half3 color_T = lerp(_TopColor1, _TopColor2, grad_T);

			// Right
			sincos(_GradRotate_R, s, c);
			half rot_R = (pos.z - _GradOrigin_R.z) * -s
				+ (pos.y - _GradOrigin_R.y) * c;
			half grad_R = saturate(rot_R / -_GradHeight_R);
			half3 color_R = lerp(_RightColor1, _RightColor2, grad_R);

			// Front
			sincos(_GradRotate_F, s, c);
			half rot_F = (pos.x - _GradOrigin_F.x) * -s
				+ (pos.y - _GradOrigin_F.y) * c;
			half grad_F = saturate(rot_F / -_GradHeight_F);
			half3 color_F = lerp(_FrontColor1, _FrontColor2, grad_F);

			// Left
			sincos(_GradRotate_L, s, c);
			half rot_L = (pos.z - _GradOrigin_L.z) * s
				+ (pos.y - _GradOrigin_L.y) * c;
			half grad_L = saturate(rot_L / -_GradHeight_L);
			half3 color_L = lerp(_LeftColor1, _LeftColor2, grad_L);

			// Back
			sincos(_GradRotate_B, s, c);
			half rot_B = (pos.x - _GradOrigin_B.x) * s
				+ (pos.y - _GradOrigin_B.y) * c;
			half grad_B = saturate(rot_B / -_GradHeight_B);
			half3 color_B = lerp(_BackColor1, _BackColor2, grad_B);

			// Bottom
			sincos(_GradRotate_D, s, c);
			half rot_D = -(pos.x - _GradOrigin_D.x) * s
				+ -(pos.z - _GradOrigin_D.z) * c;
			half grad_D = saturate(rot_D / -_GradHeight_D);
			half3 color_D = lerp(_BottomColor1, _BottomColor2, grad_D);

			half3 color = (color_T * dirTop) + (color_D * dirBottom)
						+ (color_R * dirRight) + (color_L * dirLeft)
						+ (color_F * dirFront) + (color_B * dirBack);

			albedo.rgb *= color;
#endif

#ifdef RIM_LIGHT
			float rim = 1 - saturate(dot(IN.viewDir, o.Normal));
			o.Emission = _RimLight_Color * pow(rim, _RimLight_Strength);
#endif

#ifdef HEIGHT_FOG
			distance_pixel = distance(float3(0, 0, 0), IN.worldPos);
			a = distance_pixel - _HeightFog_StartDistance;
			albedo.rgb = lerp(_HeightFog_Color, albedo.rgb, saturate(a / _HeightFog_Length));
#endif

#ifdef FOG
			float linerDepth = 1.0 / (_Fog_Far - _Fog_Near);
			float linerPos = distance(_WorldSpaceCameraPos, IN.worldPos) * linerDepth;
			float fogFactor = clamp((_Fog_Far - linerPos) / (_Fog_Far - _Fog_Near), 0.0, 1.0);
			albedo.rgb = lerp(_Fog_Color.rgb, albedo.rgb, fogFactor);
#endif

			// Output
			o.Albedo = albedo.rgb;
			o.Alpha = albedo.a;
        }

		half4 LightingColorShadow(SurfaceOutput IN, half3 lightDir, half atten)
		{
			float4 o;
			float light = 1 - 0.5 * dot(IN.Normal, lightDir);

#ifdef SHADOW_COLOR
			o.rgb = IN.Albedo * _LightColor0.rgb * (light * min((atten + _Shadow_Color.rgb), 1.5));
#elif WITHOUT_SHADOW
			o.rgb = IN.Albedo;

#else
			_Shadow_Color.rgb = 0;
			o.rgb = IN.Albedo * _LightColor0.rgb * (light * min((atten + _Shadow_Color.rgb), 1.5));

#endif
			
			o.a = IN.Alpha;

			return o;
		}
        ENDCG
    }
    FallBack "Diffuse"
}
