// Made with Amplify Shader Editor v1.9.5.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader_Portal_01"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Texture_Noise01("Texture_Noise01", 2D) = "white" {}
		_Texture_Noise02("Texture_Noise02", 2D) = "white" {}
		_Power("Power", Range( 0 , 22)) = 22
		_Emission("Emission", Float) = 2
		_Mask_Center("Mask_Center", Vector) = (0.56,0.54,0,0)

	}


	Category 
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {
			
				CGPROGRAM
				
				#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
				#endif
				
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"
				#define ASE_NEEDS_FRAG_COLOR


				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					float4 ase_texcoord1 : TEXCOORD1;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					float4 ase_texcoord3 : TEXCOORD3;
				};
				
				
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				//Don't delete this comment
				// uniform sampler2D_float _CameraDepthTexture;

				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				uniform sampler2D _Texture_Noise01;
				uniform sampler2D _Texture_Noise02;
				uniform float _Emission;
				uniform float _Power;
				uniform float2 _Mask_Center;


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					o.ase_texcoord3 = v.ase_texcoord1;

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID( i );
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float2 texCoord64 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 temp_output_1_0_g3 = texCoord64;
					float2 temp_output_11_0_g3 = ( temp_output_1_0_g3 - float2( 0.5,0.5 ) );
					float2 break18_g3 = temp_output_11_0_g3;
					float2 appendResult19_g3 = (float2(break18_g3.y , -break18_g3.x));
					float dotResult12_g3 = dot( temp_output_11_0_g3 , temp_output_11_0_g3 );
					float2 temp_cast_0 = (4.0).xx;
					float2 temp_output_76_0 = ( temp_output_1_0_g3 + ( appendResult19_g3 * ( dotResult12_g3 * temp_cast_0 ) ) + float2( 0,0 ) );
					float2 panner63 = ( 1.0 * _Time.y * float2( 0.1,0.1 ) + temp_output_76_0);
					float2 temp_output_66_0 = ( temp_output_76_0 + tex2D( _Texture_Noise02, panner63 ).r );
					float2 panner85 = ( 1.0 * _Time.y * float2( -0.1,-0.1 ) + temp_output_66_0);
					float4 tex2DNode1 = tex2D( _Texture_Noise01, panner85 );
					float4 texCoord127 = i.ase_texcoord3;
					texCoord127.xy = i.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
					float2 temp_output_34_0_g8 = ( i.texcoord.xy - _Mask_Center );
					float2 break39_g8 = temp_output_34_0_g8;
					float2 appendResult50_g8 = (float2(( texCoord127.x * ( length( temp_output_34_0_g8 ) * 2.0 ) ) , ( ( atan2( break39_g8.x , break39_g8.y ) * ( 1.0 / 6.28318548202515 ) ) * 0.0 )));
					float2 break53_g8 = appendResult50_g8;
					float4 appendResult29 = (float4(( saturate( ( i.color * tex2DNode1.r ) ) * _Emission ).rgb , ( saturate( ( pow( tex2DNode1.r , _Power ) - ( 1.0 - pow( ( 1.0 - break53_g8.x ) , _Power ) ) ) ) * i.color.a )));
					

					fixed4 col = appendResult29;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19501
Node;AmplifyShaderEditor.TextureCoordinatesNode;64;-989.5674,-253.4202;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;77;-920.9855,-24.19851;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;76;-767.6491,-74.53769;Inherit;False;Radial Shear;-1;;3;c6dc9fc7fa9b08c4d95138f2ae88b526;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;63;-585.1965,320.8318;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;61;-873.4581,212.9472;Inherit;True;Property;_Texture_Noise02;Texture_Noise02;1;0;Create;True;0;0;0;False;0;False;190a07dc9f11e924ebfd8a5dbd4f8666;190a07dc9f11e924ebfd8a5dbd4f8666;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;127;-100.6272,580.0116;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;62;-345.5497,238.8495;Inherit;True;Global;Texture1;Texture;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.Vector2Node;123;165.2635,685.5626;Inherit;False;Property;_Mask_Center;Mask_Center;4;0;Create;True;0;0;0;False;0;False;0.56,0.54;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;108;533.4493,352.5862;Inherit;True;Polar Coordinates;-1;;8;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;0;False;3;FLOAT2;0;FLOAT;55;FLOAT;56
Node;AmplifyShaderEditor.SimpleAddOpNode;66;10.11967,198.7502;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;85;394.332,27.55331;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.1,-0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;32;430.6157,761.6602;Inherit;False;Property;_Power;Power;2;0;Create;True;0;0;0;False;0;False;22;0.5;0;22;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;13;321.3838,-212.735;Inherit;True;Property;_Texture_Noise01;Texture_Noise01;0;0;Create;True;0;0;0;False;0;False;6d0c4762bf49f9c4f83e0e1f8fa6d5bb;6d0c4762bf49f9c4f83e0e1f8fa6d5bb;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.OneMinusNode;128;843.1382,474.849;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;564.2252,-58.96111;Inherit;True;Global;Texture;Texture;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.PowerNode;119;833.1009,628.5082;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;22;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;122;1024.337,525.9178;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;89;996.7728,733.0007;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;6.59;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;28;1032.274,152.2205;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;1236.037,-19.66393;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;121;1188.892,515.4348;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;34;1430.697,-36.79883;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;59;1453.32,100.0126;Inherit;False;Property;_Emission;Emission;3;0;Create;True;0;0;0;False;0;False;2;222;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;90;1350.649,515.0327;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;1757.203,26.31075;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;1958.987,230.1623;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;113;365.1414,642.4033;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;116;281.5446,320.5138;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;29;2137.286,71.29469;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;112;902.2332,163.1381;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;117;874.037,298.2521;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;120;364.1439,456.3045;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;642.8199,640.7844;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;126;2475.385,67.50301;Float;False;True;-1;2;ASEMaterialInspector;0;11;Shader_Portal_01;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;2;5;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;False;0;False;;False;False;False;False;False;False;False;False;False;True;2;False;;True;3;False;;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;76;1;64;0
WireConnection;76;3;77;0
WireConnection;63;0;76;0
WireConnection;62;0;61;0
WireConnection;62;1;63;0
WireConnection;108;2;123;0
WireConnection;108;3;127;1
WireConnection;66;0;76;0
WireConnection;66;1;62;1
WireConnection;85;0;66;0
WireConnection;128;0;108;55
WireConnection;1;0;13;0
WireConnection;1;1;85;0
WireConnection;119;0;128;0
WireConnection;119;1;32;0
WireConnection;122;0;119;0
WireConnection;89;0;1;1
WireConnection;89;1;32;0
WireConnection;30;0;28;0
WireConnection;30;1;1;1
WireConnection;121;0;89;0
WireConnection;121;1;122;0
WireConnection;34;0;30;0
WireConnection;90;0;121;0
WireConnection;33;0;34;0
WireConnection;33;1;59;0
WireConnection;48;0;90;0
WireConnection;48;1;28;4
WireConnection;113;0;127;1
WireConnection;29;0;33;0
WireConnection;29;3;48;0
WireConnection;112;0;66;0
WireConnection;112;1;108;0
WireConnection;117;0;66;0
WireConnection;120;0;127;1
WireConnection;114;0;113;0
WireConnection;114;1;32;0
WireConnection;126;0;29;0
ASEEND*/
//CHKSM=568833DBA1B96DA67BDF1B6DD250C81845D142EC