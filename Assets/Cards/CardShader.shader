// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

Shader "PopSolitaire/CardShader"
{
	Properties
	{
		CardFront ("CardFront", 2D) = "white" {}
		CardBack ("CardBack", 2D) = "white" {}
		EdgeColour("EdgeColour", COLOR ) = (0,0,0,1)
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

			#pragma multi_compile_instancing

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 Normal : NORMAL;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				int FaceType : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			sampler2D CardFront;
			sampler2D CardBack;
			float4 EdgeColour;

			#define FACETYPE_FRONT	0
			#define FACETYPE_BACK	1
			#define FACETYPE_EDGE	2

			UNITY_INSTANCING_BUFFER_START(Props)
			//UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
			UNITY_INSTANCING_BUFFER_END(Props)


			int GetFaceTypeFromNormal(float3 Normal)
			{
				#define NORMAL_FORWARD	float3(0,0,1)
				#define NORMAL_BACKWARD	float3(0,0,-1)

				if ( dot(Normal,NORMAL_FORWARD) > 0.5f )
					return FACETYPE_FRONT;

				if ( dot(Normal,NORMAL_BACKWARD) > 0.5f )
					return FACETYPE_BACK;

				return FACETYPE_EDGE;
			}

			v2f vert (appdata v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID (v);
                UNITY_TRANSFER_INSTANCE_ID (v, o);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.FaceType = GetFaceTypeFromNormal( v.Normal );
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				if ( i.FaceType == FACETYPE_FRONT )
				{
					return tex2D(CardFront, i.uv);
				}
				else if ( i.FaceType == FACETYPE_BACK )
				{
					return tex2D(CardBack, float2( i.uv.x, 1-i.uv.y) );
				}
				else if ( i.FaceType == FACETYPE_EDGE )
				{
					return EdgeColour;
				}
				else
				{
					return float4(1,0,1,1);
				}
		
			}
			ENDCG
		}
	}
}
