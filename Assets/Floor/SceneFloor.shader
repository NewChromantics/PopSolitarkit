// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Analog/SceneFloor"
{
	Properties
	{
		ColourCalibration("ColourCalibration", COLOR ) = (1,1,1,1)
		ColourCheckA("ColourCheckA", COLOR ) = ( 0,0.4,0,1 )
		ColourCheckB("ColourCheckB", COLOR ) = ( 0,0.6,0,1 )
		CheckSize("CheckSize", Range(0.001,4) ) = 0.1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			//#include "../PopUnityCommon/PopCommon.cginc"
			#define unity_WorldToClip	UNITY_MATRIX_VP

			struct appdata
			{
				float4 LocalPos : POSITION;
			};

			struct v2f
			{
				float4 ClipPos : SV_POSITION;
				float4 WorldPos : TEXCOORD1;
			};

			float4 ColourCalibration;
			float4 ColourCheckA;
			float4 ColourCheckB;
			float CheckSize;


			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.WorldPos = mul( unity_ObjectToWorld, v.LocalPos );

				o.ClipPos = mul( unity_WorldToClip, o.WorldPos );
				return o;
			}


			float4 GetCheckerColour(float3 WorldPos)
			{
				//	-0.2 == 0.2, but we need pattern to repeat so -0.2 is 1.2
				if ( WorldPos.x < 0 )	WorldPos.x = (-WorldPos.x) + CheckSize;
				if ( WorldPos.z < 0 )	WorldPos.z = (-WorldPos.z) + CheckSize;

				//	get mod of double size, so half(which we colour with) becomes 1 unit
				int x = fmod( WorldPos.x, CheckSize*2 ) / CheckSize;
				int y = fmod( WorldPos.z, CheckSize*2 ) / CheckSize;
				bool UseColourA = (x==y);
				return UseColourA ? ColourCheckA : ColourCheckB;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 Colour = i.WorldPos;
				Colour.w = 1;

				Colour = GetCheckerColour(i.WorldPos);

				return Colour;
			}
			ENDCG
		}
	}
}
