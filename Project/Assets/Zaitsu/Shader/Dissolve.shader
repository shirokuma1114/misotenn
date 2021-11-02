﻿
Shader "Custom/Dissolve" 
{
	Properties{
		_MainColor("Main Color", Color) = (1,1,1,1) // モデルの色
		_MainTex("Base (RGB)", 2D) = "white" {}     // モデルのテクスチャー
		_Mask("Mask To Dissolve", 2D) = "white" {}  // 分解用のマスク
		_CutOff("CutOff Range", Range(0,1)) = 0     // 分解のしきい値
		_Width("Width", Range(0,1)) = 0.001         // しきい値の幅
		_ColorIntensity("Intensity", Float) = 1     // 燃え尽きる部分の明るさの強度（Bloom+HDRを使わない場合は不要）
		_Color("Line Color", Color) = (1,1,1,1)     // 燃え尽きる部分の色
		_BumpMap("Normalmap", 2D) = "bump" {}       // モデルのバンプマッピング

		_RimColor("RimColor", Color) = (1,1,1,1)
		_RimPower("RimPower", float) = 0.0
	}

		SubShader{

			Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			LOD 300

			Blend SrcAlpha OneMinusSrcAlpha //重なったオブジェクトの画素の色とのブレンド方法の指定

		// モデルの描画順がおかしくなる問題の対処
		Pass{
			ZWrite On
			ColorMask 0
		}

		CGPROGRAM
		#pragma target 2.0
		#include "UnityCG.cginc"
		#pragma surface surf Lambert alpha 
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Mask;
		fixed4 _Color;
		fixed4 _MainColor;
		fixed _CutOff;
		fixed _Width;
		fixed _ColorIntensity;
		float4 _RimColor;
		float _RimPower;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};

		void surf(Input IN, inout SurfaceOutput o) 
		{
			// テクスチャの色を取得
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _MainColor;
			// バンプマッピング
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

			// マスク用テクスチャから濃度を取得（モノクロなので赤チャンネルの値だけ使用する）
			fixed a = tex2D(_Mask, IN.uv_MainTex).r;

			// 燃える切れ端表現（aの値を、しきい値～しきい値＋幅の範囲を0～1として丸める）
			fixed b = smoothstep(_CutOff, _CutOff + _Width, a);
			o.Emission = _Color * b * _ColorIntensity;

			// リムライティング
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			fixed3 emission = _RimColor.rgb * pow(rim, _RimPower) * _RimPower;
			o.Emission += emission;

			// 消失する範囲を求める　(_CutOff + _Width * 2.0 >= a) ? 1 : 0
			fixed b2 = step(a, _CutOff + _Width * 2.0);
			o.Alpha = b2 * _MainColor.a;
		}
		ENDCG
	}
		Fallback "Diffuse"
}