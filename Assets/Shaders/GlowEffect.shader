﻿Shader "Custom/GlowEffect" {
	Properties {
		_ColorTint ("Color Tint", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimPower ("Rim Power", Range(1.0, 6.0)) = 3.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float4 color:Color;
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};

		float4 _ColorTint;
		sampler2D _MainTex;
		sampler2D _BumpTex;
		sampler2D _BumpMap;
		float4 _RimColor;
		float _RimPower;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			IN.color = _ColorTint;
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * IN.color;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

			// create a color based on camera's position
			// color only outside edges...
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow(rim, _RimPower);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
