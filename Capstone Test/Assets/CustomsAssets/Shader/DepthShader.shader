// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DepthShader" {
	SubShader {
	Tags { "RenderType"="Opaque"}

	Pass {
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"

	sampler2D _CameraDepthTexture;

	struct v2f {
		float4 pos : SV_POSITION;
		float4 scrPos: TEXCOORD1;
	};


	//Vertex Shader
	v2f vert (appdata_base v) {
		v2f o;
		o.pos = UnityObjectToClipPos (v.vertex);
		o.scrPos = ComputeScreenPos(o.pos);
		return o;
	}

	//Frag Shader
	half4 frag (v2f i) : COLOR {
		float depthValue = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
		half4 depth;

		depth.r = 1-depthValue;
		depth.g = 1-depthValue;
		depth.b = 1-depthValue;

		depth.a = 1;
		return depth;
	}
	ENDCG
	}
	}
	FallBack "Diffuse"
}

