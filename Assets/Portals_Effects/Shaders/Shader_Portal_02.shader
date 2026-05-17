// Made with Amplify Shader Editor v1.9.5.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "M_Portal_02_Main"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_Texture02("Texture 02", 2D) = "white" {}
		_Noise01("Noise01", 2D) = "white" {}
		_Step("Step", Float) = 0.08
		_Speed_01("Speed_01", Vector) = (-0.2,-0.4,0,0)
		_Speed_02("Speed_02", Vector) = (0.1,0,0,0)
		_Emissive("Emissive", Float) = 1

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One One
		
		
		Pass
		{
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_COLOR


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord1 : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord1 : TEXCOORD1;
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float _Emissive;
			uniform sampler2D _Texture02;
			uniform float2 _Speed_02;
			uniform sampler2D _Noise01;
			uniform float2 _Speed_01;
			uniform float _Step;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.ase_texcoord1 = IN.ase_texcoord1;
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 texCoord197 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner196 = ( 1.0 * _Time.y * _Speed_02 + texCoord197);
				float4 tex2DNode193 = tex2D( _Texture02, panner196 );
				float2 panner195 = ( 1.0 * _Time.y * _Speed_01 + ( texCoord197 * 4.0 ));
				float4 tex2DNode194 = tex2D( _Noise01, panner195 );
				float2 texCoord225 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_218_0 = (texCoord225).y;
				float temp_output_228_0 = ( tex2DNode194.r * 0.08 );
				float4 texCoord252 = IN.ase_texcoord1;
				texCoord252.xy = IN.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float4 appendResult53 = (float4(( IN.color * _Emissive ).rgb , saturate( ( pow( ( tex2DNode193.r * tex2DNode194.r ) , _Step ) - ( 1.0 - ( pow( ( ( 1.0 - temp_output_218_0 ) - temp_output_228_0 ) , texCoord252.z ) * pow( ( temp_output_218_0 - temp_output_228_0 ) , texCoord252.w ) ) ) ) )));
				
				fixed4 c = appendResult53;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19501
Node;AmplifyShaderEditor.RangedFloatNode;210;448,144;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;197;432,16;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;209;688,176;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;232;688,336;Inherit;False;Property;_Speed_01;Speed_01;6;0;Create;True;0;0;0;False;0;False;-0.2,-0.4;-0.2,-0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;225;992,736;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;195;912,288;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.2,-0.4;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;218;1200,736;Inherit;False;False;True;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;229;1248,656;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;0.08;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;194;1104,272;Inherit;True;Property;_Noise01;Noise01;1;0;Create;True;0;0;0;False;0;False;-1;190a07dc9f11e924ebfd8a5dbd4f8666;a8548ae6ad94c364cb0c05dd7abc036c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;228;1456,688;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;219;1472,528;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;233;1536,208;Inherit;False;Property;_Speed_02;Speed_02;7;0;Create;True;0;0;0;False;0;False;0.1,0;0.2,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleSubtractOpNode;230;1616.048,533.4166;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;227;1616,752;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;196;1744,112;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;252;1280,960;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;239;2032,592;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;240;2080,832;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;193;1920,96;Inherit;True;Property;_Texture02;Texture 02;0;0;Create;True;0;0;0;False;0;False;-1;None;7d65801362931f445b9c02034768a937;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;220;2304,624;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;208;2384,176;Inherit;False;Property;_Step;Step;4;0;Create;True;0;0;0;False;0;False;0.08;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;246;2224,304;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;245;2560,256;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;212;2592,544;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;198;2784,464;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;249;2755.045,195.0791;Inherit;False;Property;_Emissive;Emissive;8;0;Create;True;0;0;0;False;0;False;1;22;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;250;2288.905,-207.8129;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;247;2944,336;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;248;2909.314,40.80925;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;216;1824,848;Inherit;False;Property;_Mask_Out;Mask_Out;3;0;Create;True;0;0;0;False;0;False;1;-0.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;222;1840,624;Inherit;False;Property;_Mask_In;Mask_In;2;0;Create;True;0;0;0;False;0;False;1;1.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;231;2356.548,-12.03646;Inherit;False;Property;_Color0;Color 0;5;0;Create;True;0;0;0;False;0;False;0.9528302,0.9393467,0.9393467,1;0.7965468,0.9106523,0.9433962,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;251;2684.131,57.3407;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;224;2224,112;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;53;3120,272;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;115;3360,272;Float;False;True;-1;2;ASEMaterialInspector;0;10;M_Portal_02_Main;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;True;4;1;False;;1;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;209;0;197;0
WireConnection;209;1;210;0
WireConnection;195;0;209;0
WireConnection;195;2;232;0
WireConnection;218;0;225;0
WireConnection;194;1;195;0
WireConnection;228;0;194;1
WireConnection;228;1;229;0
WireConnection;219;0;218;0
WireConnection;230;0;219;0
WireConnection;230;1;228;0
WireConnection;227;0;218;0
WireConnection;227;1;228;0
WireConnection;196;0;197;0
WireConnection;196;2;233;0
WireConnection;239;0;230;0
WireConnection;239;1;252;3
WireConnection;240;0;227;0
WireConnection;240;1;252;4
WireConnection;193;1;196;0
WireConnection;220;0;239;0
WireConnection;220;1;240;0
WireConnection;246;0;193;1
WireConnection;246;1;194;1
WireConnection;245;0;246;0
WireConnection;245;1;208;0
WireConnection;212;0;220;0
WireConnection;198;0;245;0
WireConnection;198;1;212;0
WireConnection;247;0;198;0
WireConnection;248;0;250;0
WireConnection;248;1;249;0
WireConnection;251;0;231;0
WireConnection;251;1;250;0
WireConnection;224;0;193;1
WireConnection;53;0;248;0
WireConnection;53;3;247;0
WireConnection;115;0;53;0
ASEEND*/
//CHKSM=0A7FD979E98F63631792D5D3CF3898A0237FF00B