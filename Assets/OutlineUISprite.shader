Shader "Shader Graphs/AdjustUIOutLine"
{
    Properties
    {
        _BaseMap("BaseMap", 2D) = "white" {}
        _OutLineWidth("OutLineWidth", Float) = 0.02
        _OutLineColor("OutLineColor", Color) = (0, 0, 0, 0)
        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalSpriteCustomLitSubTarget"
        }
//         Pass
//         {
//             Name "Sprite Lit"
//             Tags
//             {
//                 "LightMode" = "Universal2D"
//             }
        
//         // Render State
//         Cull Off
//         Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
//         ZTest [unity_GUIZTestMode]
//         ZWrite Off
        
//            Stencil
//             {
//                 Ref [_Stencil]
//                 Comp [_StencilComp]
//                 Pass [_StencilOp]
//                 ReadMask [_StencilReadMask]
//                 WriteMask [_StencilWriteMask]
//             }
//             ColorMask [_ColorMask]

//         // Debug
//         // <None>
        
//         // --------------------------------------------------
//         // Pass
        
//         HLSLPROGRAM
        
//         // Pragmas
//         #pragma target 2.0
//         #pragma exclude_renderers d3d11_9x
//         #pragma vertex vert
//         #pragma fragment frag
        
//         // Keywords
//         #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
//         #pragma multi_compile_fragment _ DEBUG_DISPLAY
//         // GraphKeywords: <None>
        
//         // Defines
        
//         #define ATTRIBUTES_NEED_NORMAL
//         #define ATTRIBUTES_NEED_TANGENT
//         #define ATTRIBUTES_NEED_TEXCOORD0
//         #define ATTRIBUTES_NEED_COLOR
//         #define VARYINGS_NEED_POSITION_WS
//         #define VARYINGS_NEED_TEXCOORD0
//         #define VARYINGS_NEED_COLOR
//         #define VARYINGS_NEED_SCREENPOSITION
//         #define FEATURES_GRAPH_VERTEX
//         /* WARNING: $splice Could not find named fragment 'PassInstancing' */
//         #define SHADERPASS SHADERPASS_SPRITELIT
//         #define ALPHA_CLIP_THRESHOLD 1
        
        
//         // custom interpolator pre-include
//         /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
//         // Includes
//         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
//         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
//         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
//         #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
//         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
//         // --------------------------------------------------
//         // Structs and Packing
        
//         // custom interpolators pre packing
//         /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
//         struct Attributes
//         {
//              float3 positionOS : POSITION;
//              float3 normalOS : NORMAL;
//              float4 tangentOS : TANGENT;
//              float4 uv0 : TEXCOORD0;
//              float4 color : COLOR;
//             #if UNITY_ANY_INSTANCING_ENABLED
//              uint instanceID : INSTANCEID_SEMANTIC;
//             #endif
//         };
//         struct Varyings
//         {
//              float4 positionCS : SV_POSITION;
//              float3 positionWS;
//              float4 texCoord0;
//              float4 color;
//              float4 screenPosition;
//             #if UNITY_ANY_INSTANCING_ENABLED
//              uint instanceID : CUSTOM_INSTANCE_ID;
//             #endif
//             #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
//              uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
//             #endif
//             #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
//              uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
//             #endif
//             #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
//              FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
//             #endif
//         };
//         struct SurfaceDescriptionInputs
//         {
//             float3 positionOS;
//              float4 uv0;
//         };
//         struct VertexDescriptionInputs
//         {
//              float3 ObjectSpaceNormal;
//              float3 ObjectSpaceTangent;
//              float3 ObjectSpacePosition;
//         };
//         struct PackedVaryings
//         {
//              float4 positionCS : SV_POSITION;
//              float4 texCoord0 : INTERP0;
//              float4 color : INTERP1;
//              float4 screenPosition : INTERP2;
//              float3 positionWS : INTERP3;
//             #if UNITY_ANY_INSTANCING_ENABLED
//              uint instanceID : CUSTOM_INSTANCE_ID;
//             #endif
//             #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
//              uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
//             #endif
//             #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
//              uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
//             #endif
//             #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
//              FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
//             #endif
//         };
        
//         PackedVaryings PackVaryings (Varyings input)
//         {
//             PackedVaryings output;
//             ZERO_INITIALIZE(PackedVaryings, output);
//             output.positionCS = input.positionCS;
//             output.texCoord0.xyzw = input.texCoord0;
//             output.color.xyzw = input.color;
//             output.screenPosition.xyzw = input.screenPosition;
//             output.positionWS.xyz = input.positionWS;
//             #if UNITY_ANY_INSTANCING_ENABLED
//             output.instanceID = input.instanceID;
//             #endif
//             #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
//             output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
//             #endif
//             #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
//             output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
//             #endif
//             #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
//             output.cullFace = input.cullFace;
//             #endif
//             return output;
//         }
        
//         Varyings UnpackVaryings (PackedVaryings input)
//         {
//             Varyings output;
//             output.positionCS = input.positionCS;
//             output.texCoord0 = input.texCoord0.xyzw;
//             output.color = input.color.xyzw;
//             output.screenPosition = input.screenPosition.xyzw;
//             output.positionWS = input.positionWS.xyz;
//             #if UNITY_ANY_INSTANCING_ENABLED
//             output.instanceID = input.instanceID;
//             #endif
//             #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
//             output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
//             #endif
//             #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
//             output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
//             #endif
//             #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
//             output.cullFace = input.cullFace;
//             #endif
//             return output;
//         }
        
        
//         // --------------------------------------------------
//         // Graph
        
//         // Graph Properties
//         CBUFFER_START(UnityPerMaterial)
//         float _OutLineWidth;
//         float4 _BaseMap_TexelSize;
//         float4 _BaseMap_ST;
//         float4 _OutLineColor;
//         CBUFFER_END
        
        
//         // Object and Global properties
//         SAMPLER(SamplerState_Linear_Repeat);
//         TEXTURE2D(_BaseMap);
//         SAMPLER(sampler_BaseMap);
        
//         // Graph Includes
//         // GraphIncludes: <None>
        
//         // -- Property used by ScenePickingPass
//         #ifdef SCENEPICKINGPASS
//         float4 _SelectionID;
//         #endif
        
//         // -- Properties used by SceneSelectionPass
//         #ifdef SCENESELECTIONPASS
//         int _ObjectId;
//         int _PassValue;
//         #endif
        
//         // -- Properties used by UnityUiClipRect
//         #ifdef UNITY_UI_CLIP_RECT
//         float4 _ClipRect;
//         float _UIMaskSoftnessX;
//         float _UIMaskSoftnessY;
//         #endif

//         // Graph Functions
        
//         void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
//         {
//             Out = lerp(A, B, T);
//         }
        
//         void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
//         {
//             Out = UV * Tiling + Offset;
//         }
        
//         void Unity_Add_float2(float2 A, float2 B, out float2 Out)
//         {
//             Out = A + B;
//         }
        
//         void Unity_Maximum_float(float A, float B, out float Out)
//         {
//             Out = max(A, B);
//         }
        
//         void Unity_Negate_float(float In, out float Out)
//         {
//             Out = -1 * In;
//         }
        
//         // Custom interpolators pre vertex
//         /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
//         // Graph Vertex
//         struct VertexDescription
//         {
//             float3 Position;
//             float3 Normal;
//             float3 Tangent;
//         };
        
//         VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
//         {
//             VertexDescription description = (VertexDescription)0;
//             description.Position = IN.ObjectSpacePosition;
//             description.Normal = IN.ObjectSpaceNormal;
//             description.Tangent = IN.ObjectSpaceTangent;
//             return description;
//         }
        
//         // Custom interpolators, pre surface
//         /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreSurface' */
        
//         // Graph Pixel
//         struct SurfaceDescription
//         {
//             float3 BaseColor;
//             float Alpha;
//             float4 SpriteMask;
//             float AlphaClipThreshold;
//         };
        
//         SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
//         {
//             SurfaceDescription surface = (SurfaceDescription)0;
//             float4 _Property_5dedf12e49a14cc3bb721f377a18be47_Out_0_Vector4 = _OutLineColor;
//             UnityTexture2D _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D = UnityBuildTexture2DStruct(_BaseMap);
//             float4 _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
//             float _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_R_4_Float = _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_RGBA_0_Vector4.r;
//             float _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_G_5_Float = _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_RGBA_0_Vector4.g;
//             float _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_B_6_Float = _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_RGBA_0_Vector4.b;
//             float _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_A_7_Float = _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_RGBA_0_Vector4.a;
//             float4 _Lerp_bce492cb069e4e59a2643049525877cc_Out_3_Vector4;
//             Unity_Lerp_float4(_Property_5dedf12e49a14cc3bb721f377a18be47_Out_0_Vector4, _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_RGBA_0_Vector4, (_SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_A_7_Float.xxxx), _Lerp_bce492cb069e4e59a2643049525877cc_Out_3_Vector4);
//             float2 _TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2;
//             Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (0, 0), _TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2);
//             float _Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float = _OutLineWidth;
//             float2 _Vector2_faa195bfacf1410eba2bf8a5b740b9c9_Out_0_Vector2 = float2(_Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float, 0);
//             float2 _Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2;
//             Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_faa195bfacf1410eba2bf8a5b740b9c9_Out_0_Vector2, _Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2);
//             float4 _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2) );
//             float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_R_4_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.r;
//             float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_G_5_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.g;
//             float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_B_6_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.b;
//             float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_A_7_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.a;
//             float2 _Vector2_c05740733dc34f81a45ae336f800b790_Out_0_Vector2 = float2(0, _Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float);
//             float2 _Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2;
//             Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_c05740733dc34f81a45ae336f800b790_Out_0_Vector2, _Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2);
//             float4 _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2) );
//             float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_R_4_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.r;
//             float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_G_5_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.g;
//             float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_B_6_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.b;
//             float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_A_7_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.a;
//             float _Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float;
//             Unity_Maximum_float(_SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_A_7_Float, _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_A_7_Float, _Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float);
//             float _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float;
//             Unity_Negate_float(_Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float, _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float);
//             float2 _Vector2_998698ce4be84faf89f72610d96ff181_Out_0_Vector2 = float2(_Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float, 0);
//             float2 _Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2;
//             Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_998698ce4be84faf89f72610d96ff181_Out_0_Vector2, _Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2);
//             float4 _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2) );
//             float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_R_4_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.r;
//             float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_G_5_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.g;
//             float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_B_6_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.b;
//             float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_A_7_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.a;
//             float2 _Vector2_27caf6eb3ecf4928bf0ce39ccdac2939_Out_0_Vector2 = float2(0, _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float);
//             float2 _Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2;
//             Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_27caf6eb3ecf4928bf0ce39ccdac2939_Out_0_Vector2, _Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2);
//             float4 _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2) );
//             float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_R_4_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.r;
//             float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_G_5_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.g;
//             float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_B_6_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.b;
//             float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_A_7_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.a;
//             float _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float;
//             Unity_Maximum_float(_SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_A_7_Float, _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_A_7_Float, _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float);
//             float _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float;
//             Unity_Maximum_float(_Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float, _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float, _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float);
//             surface.BaseColor = (_Lerp_bce492cb069e4e59a2643049525877cc_Out_3_Vector4.xyz);
// #ifdef UNITY_UI_CLIP_RECT
//  // ここに RectMask2D の処理を書く
//            const float2 pixel_size = 1 / float2(1, 1) / abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));
//            const float4 clamped_rect = clamp(_ClipRect, -2e10, 2e10);
//            const float2 mask_xy = IN.positionOS.xy * 2 - clamped_rect.xy - clamped_rect.zw;
//            const float2 mask_zw = 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixel_size.xy));
//            const float4 mask = float4(mask_xy, mask_zw);
//            const half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(mask.xy)) * mask.zw);
//            surface.Alpha =　_Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float * m.x * m.y;

// #else

//            surface.Alpha = _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float;
// #endif
//             surface.SpriteMask = IsGammaSpace() ? float4(1, 1, 1, 1) : float4 (SRGBToLinear(float3(1, 1, 1)), 1);
//             surface.AlphaClipThreshold = 0.5;
//             return surface;
//         }
        
//         // --------------------------------------------------
//         // Build Graph Inputs
//         #ifdef HAVE_VFX_MODIFICATION
//         #define VFX_SRP_ATTRIBUTES Attributes
//         #define VFX_SRP_VARYINGS Varyings
//         #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
//         #endif
//         VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
//         {
//             VertexDescriptionInputs output;
//             ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
//             output.ObjectSpaceNormal =                          input.normalOS;
//             output.ObjectSpaceTangent =                         input.tangentOS.xyz;
//             output.ObjectSpacePosition =                        input.positionOS;
        
//             return output;
//         }
//         SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
//         {
//             SurfaceDescriptionInputs output;
           
//             ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
//         #ifdef HAVE_VFX_MODIFICATION
//         #if VFX_USE_GRAPH_VALUES
//             uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
//             /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
//         #endif
//             /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
//         #endif
        
//             /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
        
        
        
        
        
//             #if UNITY_UV_STARTS_AT_TOP
//             #else
//             #endif
        
        
//             //追加
//             output.positionOS = mul(unity_WorldToObject, float4(input.positionWS, 1.0));
//             output.uv0 = input.texCoord0;

//         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
//         #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
//         #else
//         #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
//         #endif
//         #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
//                 return output;
//         }
        
//         // --------------------------------------------------
//         // Main
        
//         #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
//         // --------------------------------------------------
//         // Visual Effect Vertex Invocations
//         #ifdef HAVE_VFX_MODIFICATION
//         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
//         #endif
        
//         ENDHLSL
//         }
//         Pass
//         {
//             Name "Sprite Normal"
//             Tags
//             {
//                 "LightMode" = "NormalsRendering"
//             }
        
//         // Render State
//         Cull Off
//         Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
//         ZTest [unity_GUIZTestMode]
//         ZWrite Off
        
//         // Debug
//         // <None>
        
//         // --------------------------------------------------
//         // Pass
        
//         Stencil
//            {
//                 Ref [_Stencil]
//                 Comp [_StencilComp]
//                 Pass [_StencilOp]
//                 ReadMask [_StencilReadMask]
//                 WriteMask [_StencilWriteMask]
//             }
//             ColorMask [_ColorMask]


//         HLSLPROGRAM
        
//         // Pragmas
//         #pragma target 2.0
//         #pragma exclude_renderers d3d11_9x
//         #pragma vertex vert
//         #pragma fragment frag
        
//         // Keywords
//         // PassKeywords: <None>
//         // GraphKeywords: <None>
        
//         // Defines
        
//         #define ATTRIBUTES_NEED_NORMAL
//         #define ATTRIBUTES_NEED_TANGENT
//         #define ATTRIBUTES_NEED_TEXCOORD0
//         #define VARYINGS_NEED_NORMAL_WS
//         #define VARYINGS_NEED_TANGENT_WS
//         #define VARYINGS_NEED_TEXCOORD0
//         #define FEATURES_GRAPH_VERTEX
//         /* WARNING: $splice Could not find named fragment 'PassInstancing' */
//         #define SHADERPASS SHADERPASS_SPRITENORMAL
//         #define ALPHA_CLIP_THRESHOLD 1
        
        
//         // custom interpolator pre-include
//         /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
//         // Includes
//         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
//         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
//         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
//         #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
//         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
//         // --------------------------------------------------
//         // Structs and Packing
        
//         // custom interpolators pre packing
//         /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
//         struct Attributes
//         {
//              float3 positionOS : POSITION;
//              float3 normalOS : NORMAL;
//              float4 tangentOS : TANGENT;
//              float4 uv0 : TEXCOORD0;
//             #if UNITY_ANY_INSTANCING_ENABLED
//              uint instanceID : INSTANCEID_SEMANTIC;
//             #endif
//         };
//         struct Varyings
//         {
//              float4 positionCS : SV_POSITION;
//              float3 normalWS;
//              float4 tangentWS;
//              float4 texCoord0;
//             #if UNITY_ANY_INSTANCING_ENABLED
//              uint instanceID : CUSTOM_INSTANCE_ID;
//             #endif
//             #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
//              uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
//             #endif
//             #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
//              uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
//             #endif
//             #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
//              FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
//             #endif
//         };
//         struct SurfaceDescriptionInputs
//         {
//              float3 positionOS;
//              float3 TangentSpaceNormal;
//              float4 uv0;
//         };
//         struct VertexDescriptionInputs
//         {
//              float3 ObjectSpaceNormal;
//              float3 ObjectSpaceTangent;
//              float3 ObjectSpacePosition;
//         };
//         struct PackedVaryings
//         {
//              float4 positionCS : SV_POSITION;
//              float4 tangentWS : INTERP0;
//              float4 texCoord0 : INTERP1;
//              float3 normalWS : INTERP2;
//             #if UNITY_ANY_INSTANCING_ENABLED
//              uint instanceID : CUSTOM_INSTANCE_ID;
//             #endif
//             #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
//              uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
//             #endif
//             #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
//              uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
//             #endif
//             #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
//              FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
//             #endif
//         };
        
//         PackedVaryings PackVaryings (Varyings input)
//         {
//             PackedVaryings output;
//             ZERO_INITIALIZE(PackedVaryings, output);
//             output.positionCS = input.positionCS;
//             output.tangentWS.xyzw = input.tangentWS;
//             output.texCoord0.xyzw = input.texCoord0;
//             output.normalWS.xyz = input.normalWS;
//             #if UNITY_ANY_INSTANCING_ENABLED
//             output.instanceID = input.instanceID;
//             #endif
//             #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
//             output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
//             #endif
//             #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
//             output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
//             #endif
//             #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
//             output.cullFace = input.cullFace;
//             #endif
//             return output;
//         }
        
//         Varyings UnpackVaryings (PackedVaryings input)
//         {
//             Varyings output;
//             output.positionCS = input.positionCS;
//             output.tangentWS = input.tangentWS.xyzw;
//             output.texCoord0 = input.texCoord0.xyzw;
//             output.normalWS = input.normalWS.xyz;
//             #if UNITY_ANY_INSTANCING_ENABLED
//             output.instanceID = input.instanceID;
//             #endif
//             #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
//             output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
//             #endif
//             #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
//             output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
//             #endif
//             #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
//             output.cullFace = input.cullFace;
//             #endif
//             return output;
//         }
        
        
//         // --------------------------------------------------
//         // Graph
        
//         // Graph Properties
//         CBUFFER_START(UnityPerMaterial)
//         float _OutLineWidth;
//         float4 _BaseMap_TexelSize;
//         float4 _BaseMap_ST;
//         float4 _OutLineColor;
//         CBUFFER_END
        
        
//         // Object and Global properties
//         SAMPLER(SamplerState_Linear_Repeat);
//         TEXTURE2D(_BaseMap);
//         SAMPLER(sampler_BaseMap);
        
//         // Graph Includes
//         // GraphIncludes: <None>
        
//         // -- Property used by ScenePickingPass
//         #ifdef SCENEPICKINGPASS
//         float4 _SelectionID;
//         #endif
        
//         // -- Properties used by SceneSelectionPass
//         #ifdef SCENESELECTIONPASS
//         int _ObjectId;
//         int _PassValue;
//         #endif
        
//         // Graph Functions
        
//         void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
//         {
//             Out = UV * Tiling + Offset;
//         }
        
//         void Unity_Add_float2(float2 A, float2 B, out float2 Out)
//         {
//             Out = A + B;
//         }
        
//         void Unity_Maximum_float(float A, float B, out float Out)
//         {
//             Out = max(A, B);
//         }
        
//         void Unity_Negate_float(float In, out float Out)
//         {
//             Out = -1 * In;
//         }
        
//         // Custom interpolators pre vertex
//         /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
//         // Graph Vertex
//         struct VertexDescription
//         {
//             float3 Position;
//             float3 Normal;
//             float3 Tangent;
//         };
        
//         VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
//         {
//             VertexDescription description = (VertexDescription)0;
//             description.Position = IN.ObjectSpacePosition;
//             description.Normal = IN.ObjectSpaceNormal;
//             description.Tangent = IN.ObjectSpaceTangent;
//             return description;
//         }
        
//         // Custom interpolators, pre surface
//         /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreSurface' */
        
//         // Graph Pixel
//         struct SurfaceDescription
//         {
//             float Alpha;
//             float3 NormalTS;
//             float AlphaClipThreshold;
//         };
        
//         SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
//         {
//             SurfaceDescription surface = (SurfaceDescription)0;
//             UnityTexture2D _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D = UnityBuildTexture2DStruct(_BaseMap);
//             float2 _TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2;
//             Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (0, 0), _TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2);
//             float _Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float = _OutLineWidth;
//             float2 _Vector2_faa195bfacf1410eba2bf8a5b740b9c9_Out_0_Vector2 = float2(_Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float, 0);
//             float2 _Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2;
//             Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_faa195bfacf1410eba2bf8a5b740b9c9_Out_0_Vector2, _Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2);
//             float4 _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2) );
//             float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_R_4_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.r;
//             float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_G_5_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.g;
//             float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_B_6_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.b;
//             float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_A_7_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.a;
//             float2 _Vector2_c05740733dc34f81a45ae336f800b790_Out_0_Vector2 = float2(0, _Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float);
//             float2 _Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2;
//             Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_c05740733dc34f81a45ae336f800b790_Out_0_Vector2, _Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2);
//             float4 _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2) );
//             float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_R_4_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.r;
//             float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_G_5_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.g;
//             float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_B_6_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.b;
//             float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_A_7_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.a;
//             float _Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float;
//             Unity_Maximum_float(_SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_A_7_Float, _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_A_7_Float, _Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float);
//             float _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float;
//             Unity_Negate_float(_Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float, _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float);
//             float2 _Vector2_998698ce4be84faf89f72610d96ff181_Out_0_Vector2 = float2(_Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float, 0);
//             float2 _Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2;
//             Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_998698ce4be84faf89f72610d96ff181_Out_0_Vector2, _Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2);
//             float4 _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2) );
//             float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_R_4_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.r;
//             float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_G_5_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.g;
//             float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_B_6_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.b;
//             float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_A_7_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.a;
//             float2 _Vector2_27caf6eb3ecf4928bf0ce39ccdac2939_Out_0_Vector2 = float2(0, _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float);
//             float2 _Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2;
//             Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_27caf6eb3ecf4928bf0ce39ccdac2939_Out_0_Vector2, _Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2);
//             float4 _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2) );
//             float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_R_4_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.r;
//             float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_G_5_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.g;
//             float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_B_6_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.b;
//             float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_A_7_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.a;
//             float _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float;
//             Unity_Maximum_float(_SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_A_7_Float, _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_A_7_Float, _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float);
//             float _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float;
//             Unity_Maximum_float(_Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float, _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float, _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float);
            
// #ifdef UNITY_UI_CLIP_RECT
// // ここに RectMask2D の処理を書く
//            const float2 pixel_size = 1 / float2(1, 1) / abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));
//            const float4 clamped_rect = clamp(_ClipRect, -2e10, 2e10);
//            const float2 mask_xy = IN.positionOS.xy * 2 - clamped_rect.xy - clamped_rect.zw;
//            const float2 mask_zw = 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixel_size.xy));
//            const float4 mask = float4(mask_xy, mask_zw);
//            const half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(mask.xy)) * mask.zw);
//            surface.Alpha =　_Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float;

// #else
//            surface.Alpha = _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float;
// #endif
          
//            surface.NormalTS = IN.TangentSpaceNormal;
//            surface.AlphaClipThreshold = 0.5;
//            return surface;
//         }
        
//         // --------------------------------------------------
//         // Build Graph Inputs
//         #ifdef HAVE_VFX_MODIFICATION
//         #define VFX_SRP_ATTRIBUTES Attributes
//         #define VFX_SRP_VARYINGS Varyings
//         #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
//         #endif
//         VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
//         {
//             VertexDescriptionInputs output;
//             ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
//             output.ObjectSpaceNormal =                          input.normalOS;
//             output.ObjectSpaceTangent =                         input.tangentOS.xyz;
//             output.ObjectSpacePosition =                        input.positionOS;
        
//             return output;
//         }
//         SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
//         {
//             SurfaceDescriptionInputs output;
//             ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
//         #ifdef HAVE_VFX_MODIFICATION
//         #if VFX_USE_GRAPH_VALUES
//             uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
//             /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
//         #endif
//             /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
//         #endif
        
//             /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
        
        
//             output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
        
//             #if UNITY_UV_STARTS_AT_TOP
//             #else
//             #endif
        
//             output.positionOS = mul(unity_WorldToObject, float4(input.positionWS, 1.0));
//             output.uv0 = input.texCoord0;
//         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
//         #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
//         #else
//         #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
//         #endif
//         #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
//                 return output;
//         }
        
//         // --------------------------------------------------
//         // Main
        
//         #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
//         #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteNormalPass.hlsl"
        
//         // --------------------------------------------------
//         // Visual Effect Vertex Invocations
//         #ifdef HAVE_VFX_MODIFICATION
//         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
//         #endif
        
//         ENDHLSL
//         }
        // Pass
        // {
        //     Name "SceneSelectionPass"
        //     Tags
        //     {
        //         "LightMode" = "SceneSelectionPass"
        //     }
        
        // // Render State
        // Cull Off
        
        // // Debug
        // // <None>
        
        // // --------------------------------------------------
        // // Pass
        
        // HLSLPROGRAM
        
        // // Pragmas
        // #pragma target 2.0
        // #pragma exclude_renderers d3d11_9x
        // #pragma vertex vert
        // #pragma fragment frag
        
        // // Keywords
        // // PassKeywords: <None>
        // // GraphKeywords: <None>
        
        // // Defines
        
        // #define ATTRIBUTES_NEED_NORMAL
        // #define ATTRIBUTES_NEED_TANGENT
        // #define ATTRIBUTES_NEED_TEXCOORD0
        // #define VARYINGS_NEED_TEXCOORD0
        // #define FEATURES_GRAPH_VERTEX
        // /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        // #define SHADERPASS SHADERPASS_DEPTHONLY
        // #define SCENESELECTIONPASS 1
        
        // #define _ALPHATEST_ON 1
        
        
        // // custom interpolator pre-include
        // /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // // Includes
        // #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        // #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        // #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        // #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        // #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        // #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        // #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // // --------------------------------------------------
        // // Structs and Packing
        
        // // custom interpolators pre packing
        // /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        // struct Attributes
        // {
        //      float3 positionOS : POSITION;
        //      float3 normalOS : NORMAL;
        //      float4 tangentOS : TANGENT;
        //      float4 uv0 : TEXCOORD0;
        //     #if UNITY_ANY_INSTANCING_ENABLED
        //      uint instanceID : INSTANCEID_SEMANTIC;
        //     #endif
        // };
        // struct Varyings
        // {
        //      float4 positionCS : SV_POSITION;
        //      float4 texCoord0;
        //     #if UNITY_ANY_INSTANCING_ENABLED
        //      uint instanceID : CUSTOM_INSTANCE_ID;
        //     #endif
        //     #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        //      uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        //     #endif
        //     #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        //      uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        //     #endif
        //     #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        //      FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        //     #endif
        // };
        // struct SurfaceDescriptionInputs
        // {
        //      float3 positionOS;
        //      float4 uv0;
        // };
        // struct VertexDescriptionInputs
        // {
        //      float3 ObjectSpaceNormal;
        //      float3 ObjectSpaceTangent;
        //      float3 ObjectSpacePosition;
        // };
        // struct PackedVaryings
        // {
        //      float4 positionCS : SV_POSITION;
        //      float4 texCoord0 : INTERP0;
        //     #if UNITY_ANY_INSTANCING_ENABLED
        //      uint instanceID : CUSTOM_INSTANCE_ID;
        //     #endif
        //     #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        //      uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        //     #endif
        //     #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        //      uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        //     #endif
        //     #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        //      FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        //     #endif
        // };
        
        // PackedVaryings PackVaryings (Varyings input)
        // {
        //     PackedVaryings output;
        //     ZERO_INITIALIZE(PackedVaryings, output);
        //     output.positionCS = input.positionCS;
        //     output.texCoord0.xyzw = input.texCoord0;
        //     #if UNITY_ANY_INSTANCING_ENABLED
        //     output.instanceID = input.instanceID;
        //     #endif
        //     #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        //     output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        //     #endif
        //     #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        //     output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        //     #endif
        //     #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        //     output.cullFace = input.cullFace;
        //     #endif
        //     return output;
        // }
        
        // Varyings UnpackVaryings (PackedVaryings input)
        // {
        //     Varyings output;
        //     output.positionCS = input.positionCS;
        //     output.texCoord0 = input.texCoord0.xyzw;
        //     #if UNITY_ANY_INSTANCING_ENABLED
        //     output.instanceID = input.instanceID;
        //     #endif
        //     #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        //     output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        //     #endif
        //     #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        //     output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        //     #endif
        //     #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        //     output.cullFace = input.cullFace;
        //     #endif
        //     return output;
        // }
        
        
        // // --------------------------------------------------
        // // Graph
        
        // // Graph Properties
        // CBUFFER_START(UnityPerMaterial)
        // float _OutLineWidth;
        // float4 _BaseMap_TexelSize;
        // float4 _BaseMap_ST;
        // float4 _OutLineColor;
        // CBUFFER_END
        
        
        // // Object and Global properties
        // SAMPLER(SamplerState_Linear_Repeat);
        // TEXTURE2D(_BaseMap);
        // SAMPLER(sampler_BaseMap);
        
        // // Graph Includes
        // // GraphIncludes: <None>
        
        // // -- Property used by ScenePickingPass
        // #ifdef SCENEPICKINGPASS
        // float4 _SelectionID;
        // #endif
        
        // // -- Properties used by SceneSelectionPass
        // #ifdef SCENESELECTIONPASS
        // int _ObjectId;
        // int _PassValue;
        // #endif
        
        // // Graph Functions
        
        // void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        // {
        //     Out = UV * Tiling + Offset;
        // }
        
        // void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        // {
        //     Out = A + B;
        // }
        
        // void Unity_Maximum_float(float A, float B, out float Out)
        // {
        //     Out = max(A, B);
        // }
        
        // void Unity_Negate_float(float In, out float Out)
        // {
        //     Out = -1 * In;
        // }
        
        // // Custom interpolators pre vertex
        // /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // // Graph Vertex
        // struct VertexDescription
        // {
        //     float3 Position;
        //     float3 Normal;
        //     float3 Tangent;
        // };
        
        // VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        // {
        //     VertexDescription description = (VertexDescription)0;
        //     description.Position = IN.ObjectSpacePosition;
        //     description.Normal = IN.ObjectSpaceNormal;
        //     description.Tangent = IN.ObjectSpaceTangent;
        //     return description;
        // }
        
        // // Custom interpolators, pre surface
        // #ifdef FEATURES_GRAPH_VERTEX
        // Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        // {
        // return output;
        // }
        // #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        // #endif
        
        // // Graph Pixel
        // struct SurfaceDescription
        // {
        //     float Alpha;
        //     float AlphaClipThreshold;
        // };
        
        // SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        // {
        //     SurfaceDescription surface = (SurfaceDescription)0;
        //     UnityTexture2D _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D = UnityBuildTexture2DStruct(_BaseMap);
        //     float2 _TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2;
        //     Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (0, 0), _TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2);
        //     float _Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float = _OutLineWidth;
        //     float2 _Vector2_faa195bfacf1410eba2bf8a5b740b9c9_Out_0_Vector2 = float2(_Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float, 0);
        //     float2 _Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2;
        //     Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_faa195bfacf1410eba2bf8a5b740b9c9_Out_0_Vector2, _Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2);
        //     float4 _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2) );
        //     float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_R_4_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.r;
        //     float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_G_5_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.g;
        //     float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_B_6_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.b;
        //     float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_A_7_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.a;
        //     float2 _Vector2_c05740733dc34f81a45ae336f800b790_Out_0_Vector2 = float2(0, _Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float);
        //     float2 _Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2;
        //     Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_c05740733dc34f81a45ae336f800b790_Out_0_Vector2, _Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2);
        //     float4 _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2) );
        //     float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_R_4_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.r;
        //     float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_G_5_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.g;
        //     float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_B_6_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.b;
        //     float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_A_7_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.a;
        //     float _Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float;
        //     Unity_Maximum_float(_SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_A_7_Float, _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_A_7_Float, _Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float);
        //     float _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float;
        //     Unity_Negate_float(_Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float, _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float);
        //     float2 _Vector2_998698ce4be84faf89f72610d96ff181_Out_0_Vector2 = float2(_Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float, 0);
        //     float2 _Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2;
        //     Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_998698ce4be84faf89f72610d96ff181_Out_0_Vector2, _Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2);
        //     float4 _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2) );
        //     float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_R_4_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.r;
        //     float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_G_5_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.g;
        //     float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_B_6_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.b;
        //     float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_A_7_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.a;
        //     float2 _Vector2_27caf6eb3ecf4928bf0ce39ccdac2939_Out_0_Vector2 = float2(0, _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float);
        //     float2 _Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2;
        //     Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_27caf6eb3ecf4928bf0ce39ccdac2939_Out_0_Vector2, _Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2);
        //     float4 _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2) );
        //     float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_R_4_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.r;
        //     float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_G_5_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.g;
        //     float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_B_6_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.b;
        //     float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_A_7_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.a;
        //     float _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float;
        //     Unity_Maximum_float(_SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_A_7_Float, _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_A_7_Float, _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float);
        //     float _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float;
        //     Unity_Maximum_float(_Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float, _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float, _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float);
        //     surface.Alpha = _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float;
        //     surface.AlphaClipThreshold = 0.5;
        //     return surface;
        // }
        
        // // --------------------------------------------------
        // // Build Graph Inputs
        // #ifdef HAVE_VFX_MODIFICATION
        // #define VFX_SRP_ATTRIBUTES Attributes
        // #define VFX_SRP_VARYINGS Varyings
        // #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        // #endif
        // VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        // {
        //     VertexDescriptionInputs output;
        //     ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
        //     output.ObjectSpaceNormal =                          input.normalOS;
        //     output.ObjectSpaceTangent =                         input.tangentOS.xyz;
        //     output.ObjectSpacePosition =                        input.positionOS;
        
        //     return output;
        // }
        // SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        // {
        //     SurfaceDescriptionInputs output;
        //     ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        // #ifdef HAVE_VFX_MODIFICATION
        // #if VFX_USE_GRAPH_VALUES
        //     uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
        //     /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        // #endif
        //     /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        // #endif
        
            
        
        
        
        
        
        
        //     #if UNITY_UV_STARTS_AT_TOP
        //     #else
        //     #endif
        
        //     //output.positionOS = mul(unity_WorldToObject, float4(input.positionWS, 1.0));
        //     output.uv0 = input.texCoord0;
        // #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        // #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        // #else
        // #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        // #endif
        // #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
        //         return output;
        // }
        
        // // --------------------------------------------------
        // // Main
        
        // #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        // #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // // --------------------------------------------------
        // // Visual Effect Vertex Invocations
        // #ifdef HAVE_VFX_MODIFICATION
        // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        // #endif
        
        // ENDHLSL
        // }
        // Pass
        // {
        //     Name "ScenePickingPass"
        //     Tags
        //     {
        //         "LightMode" = "Picking"
        //     }
        
        // // Render State
        // Cull Back
        
        // // Debug
        // // <None>
        
        // // --------------------------------------------------
        // // Pass
        
        // HLSLPROGRAM
        
        // // Pragmas
        // #pragma target 2.0
        // #pragma exclude_renderers d3d11_9x
        // #pragma vertex vert
        // #pragma fragment frag
        
        // // Keywords
        // // PassKeywords: <None>
        // // GraphKeywords: <None>
        
        // // Defines
        
        // #define ATTRIBUTES_NEED_NORMAL
        // #define ATTRIBUTES_NEED_TANGENT
        // #define ATTRIBUTES_NEED_TEXCOORD0
        // #define VARYINGS_NEED_TEXCOORD0
        // #define FEATURES_GRAPH_VERTEX
        // /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        // #define SHADERPASS SHADERPASS_DEPTHONLY
        // #define SCENEPICKINGPASS 1
        
        // #define _ALPHATEST_ON 1
        
        
        // // custom interpolator pre-include
        // /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // // Includes
        // #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        // #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        // #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        // #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        // #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        // #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        // #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // // --------------------------------------------------
        // // Structs and Packing
        
        // // custom interpolators pre packing
        // /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        // struct Attributes
        // {
        //      float3 positionOS : POSITION;
        //      float3 normalOS : NORMAL;
        //      float4 tangentOS : TANGENT;
        //      float4 uv0 : TEXCOORD0;
        //     #if UNITY_ANY_INSTANCING_ENABLED
        //      uint instanceID : INSTANCEID_SEMANTIC;
        //     #endif
        // };
        // struct Varyings
        // {
        //      float4 positionCS : SV_POSITION;
        //      float4 texCoord0;
        //     #if UNITY_ANY_INSTANCING_ENABLED
        //      uint instanceID : CUSTOM_INSTANCE_ID;
        //     #endif
        //     #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        //      uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        //     #endif
        //     #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        //      uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        //     #endif
        //     #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        //      FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        //     #endif
        // };
        // struct SurfaceDescriptionInputs
        // { 
        //      float3 positionOS;
        //      float4 uv0;
        // };
        // struct VertexDescriptionInputs
        // {
        //      float3 ObjectSpaceNormal;
        //      float3 ObjectSpaceTangent;
        //      float3 ObjectSpacePosition;
        // };
        // struct PackedVaryings
        // {
        //      float4 positionCS : SV_POSITION;
        //      float4 texCoord0 : INTERP0;
        //     #if UNITY_ANY_INSTANCING_ENABLED
        //      uint instanceID : CUSTOM_INSTANCE_ID;
        //     #endif
        //     #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        //      uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        //     #endif
        //     #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        //      uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        //     #endif
        //     #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        //      FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        //     #endif
        // };
        
        // PackedVaryings PackVaryings (Varyings input)
        // {
        //     PackedVaryings output;
        //     ZERO_INITIALIZE(PackedVaryings, output);
        //     output.positionCS = input.positionCS;
        //     output.texCoord0.xyzw = input.texCoord0;
        //     #if UNITY_ANY_INSTANCING_ENABLED
        //     output.instanceID = input.instanceID;
        //     #endif
        //     #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        //     output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        //     #endif
        //     #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        //     output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        //     #endif
        //     #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        //     output.cullFace = input.cullFace;
        //     #endif
        //     return output;
        // }
        
        // Varyings UnpackVaryings (PackedVaryings input)
        // {
        //     Varyings output;
        //     output.positionCS = input.positionCS;
        //     output.texCoord0 = input.texCoord0.xyzw;
        //     #if UNITY_ANY_INSTANCING_ENABLED
        //     output.instanceID = input.instanceID;
        //     #endif
        //     #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        //     output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        //     #endif
        //     #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        //     output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        //     #endif
        //     #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        //     output.cullFace = input.cullFace;
        //     #endif
        //     return output;
        // }
        
        
        // // --------------------------------------------------
        // // Graph
        
        // // Graph Properties
        // CBUFFER_START(UnityPerMaterial)
        // float _OutLineWidth;
        // float4 _BaseMap_TexelSize;
        // float4 _BaseMap_ST;
        // float4 _OutLineColor;
        // CBUFFER_END
        
        
        // // Object and Global properties
        // SAMPLER(SamplerState_Linear_Repeat);
        // TEXTURE2D(_BaseMap);
        // SAMPLER(sampler_BaseMap);
        
        // // Graph Includes
        // // GraphIncludes: <None>
        
        // // -- Property used by ScenePickingPass
        // #ifdef SCENEPICKINGPASS
        // float4 _SelectionID;
        // #endif
        
        // // -- Properties used by SceneSelectionPass
        // #ifdef SCENESELECTIONPASS
        // int _ObjectId;
        // int _PassValue;
        // #endif
        
        // // Graph Functions
        
        // void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        // {
        //     Out = UV * Tiling + Offset;
        // }
        
        // void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        // {
        //     Out = A + B;
        // }
        
        // void Unity_Maximum_float(float A, float B, out float Out)
        // {
        //     Out = max(A, B);
        // }
        
        // void Unity_Negate_float(float In, out float Out)
        // {
        //     Out = -1 * In;
        // }
        
        // // Custom interpolators pre vertex
        // /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // // Graph Vertex
        // struct VertexDescription
        // {
        //     float3 Position;
        //     float3 Normal;
        //     float3 Tangent;
        // };
        
        // VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        // {
        //     VertexDescription description = (VertexDescription)0;
        //     description.Position = IN.ObjectSpacePosition;
        //     description.Normal = IN.ObjectSpaceNormal;
        //     description.Tangent = IN.ObjectSpaceTangent;
        //     return description;
        // }
        
        // // Custom interpolators, pre surface
        // #ifdef FEATURES_GRAPH_VERTEX
        // Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        // {
        // return output;
        // }
        // #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        // #endif
        
        // // Graph Pixel
        // struct SurfaceDescription
        // {
        //     float Alpha;
        //     float AlphaClipThreshold;
        // };
        
        // SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        // {
        //     SurfaceDescription surface = (SurfaceDescription)0;
        //     UnityTexture2D _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D = UnityBuildTexture2DStruct(_BaseMap);
        //     float2 _TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2;
        //     Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (0, 0), _TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2);
        //     float _Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float = _OutLineWidth;
        //     float2 _Vector2_faa195bfacf1410eba2bf8a5b740b9c9_Out_0_Vector2 = float2(_Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float, 0);
        //     float2 _Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2;
        //     Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_faa195bfacf1410eba2bf8a5b740b9c9_Out_0_Vector2, _Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2);
        //     float4 _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2) );
        //     float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_R_4_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.r;
        //     float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_G_5_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.g;
        //     float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_B_6_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.b;
        //     float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_A_7_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.a;
        //     float2 _Vector2_c05740733dc34f81a45ae336f800b790_Out_0_Vector2 = float2(0, _Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float);
        //     float2 _Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2;
        //     Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_c05740733dc34f81a45ae336f800b790_Out_0_Vector2, _Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2);
        //     float4 _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2) );
        //     float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_R_4_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.r;
        //     float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_G_5_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.g;
        //     float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_B_6_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.b;
        //     float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_A_7_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.a;
        //     float _Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float;
        //     Unity_Maximum_float(_SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_A_7_Float, _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_A_7_Float, _Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float);
        //     float _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float;
        //     Unity_Negate_float(_Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float, _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float);
        //     float2 _Vector2_998698ce4be84faf89f72610d96ff181_Out_0_Vector2 = float2(_Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float, 0);
        //     float2 _Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2;
        //     Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_998698ce4be84faf89f72610d96ff181_Out_0_Vector2, _Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2);
        //     float4 _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2) );
        //     float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_R_4_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.r;
        //     float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_G_5_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.g;
        //     float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_B_6_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.b;
        //     float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_A_7_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.a;
        //     float2 _Vector2_27caf6eb3ecf4928bf0ce39ccdac2939_Out_0_Vector2 = float2(0, _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float);
        //     float2 _Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2;
        //     Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_27caf6eb3ecf4928bf0ce39ccdac2939_Out_0_Vector2, _Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2);
        //     float4 _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2) );
        //     float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_R_4_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.r;
        //     float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_G_5_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.g;
        //     float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_B_6_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.b;
        //     float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_A_7_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.a;
        //     float _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float;
        //     Unity_Maximum_float(_SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_A_7_Float, _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_A_7_Float, _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float);
        //     float _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float;
        //     Unity_Maximum_float(_Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float, _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float, _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float);
        //     surface.Alpha = _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float;
        //     surface.AlphaClipThreshold = 0.5;
        //     return surface;
        // }
        
        // // --------------------------------------------------
        // // Build Graph Inputs
        // #ifdef HAVE_VFX_MODIFICATION
        // #define VFX_SRP_ATTRIBUTES Attributes
        // #define VFX_SRP_VARYINGS Varyings
        // #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        // #endif
        // VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        // {
        //     VertexDescriptionInputs output;
        //     ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
        //     output.ObjectSpaceNormal =                          input.normalOS;
        //     output.ObjectSpaceTangent =                         input.tangentOS.xyz;
        //     output.ObjectSpacePosition =                        input.positionOS;
        
        //     return output;
        // }
        // SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        // {
        //     SurfaceDescriptionInputs output;
        //     ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        // #ifdef HAVE_VFX_MODIFICATION
        // #if VFX_USE_GRAPH_VALUES
        //     uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
        //     /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        // #endif
        //     /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        // #endif
        
            
        
        
        
        
        
        
        //     #if UNITY_UV_STARTS_AT_TOP
        //     #else
        //     #endif
        
        //     //output.positionOS = mul(unity_WorldToObject, float4(input.positionWS, 1.0));
        //     output.uv0 = input.texCoord0;
        // #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        // #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        // #else
        // #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        // #endif
        // #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
        //         return output;
        // }
        
        // // --------------------------------------------------
        // // Main
        
        // #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        // #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // // --------------------------------------------------
        // // Visual Effect Vertex Invocations
        // #ifdef HAVE_VFX_MODIFICATION
        // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        // #endif
        
        // ENDHLSL
        // }
        Pass
        {
            Name "Sprite Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest [unity_GUIZTestMode]
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass

        Stencil
          {
                 Ref [_Stencil]
                 Comp [_StencilComp]
                 Pass [_StencilOp]
                 ReadMask [_StencilReadMask]
                 WriteMask [_StencilWriteMask]
             }
             ColorMask [_ColorMask]

        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEFORWARD
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 positionOS;
             float3 TangentSpaceNormal;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float3 positionWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _OutLineWidth;
        float4 _BaseMap_TexelSize;
        float4 _BaseMap_ST;
        float4 _OutLineColor;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_BaseMap);
        SAMPLER(sampler_BaseMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
         // -- Properties used by UnityUiClipRect
         #ifdef UNITY_UI_CLIP_RECT
         float4 _ClipRect;
         float _UIMaskSoftnessX;
         float _UIMaskSoftnessY;
         #endif

        // Graph Functions
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_Maximum_float(float A, float B, out float Out)
        {
            Out = max(A, B);
        }
        
        void Unity_Negate_float(float In, out float Out)
        {
            Out = -1 * In;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_5dedf12e49a14cc3bb721f377a18be47_Out_0_Vector4 = _OutLineColor;
            UnityTexture2D _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D = UnityBuildTexture2DStruct(_BaseMap);
            float4 _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_R_4_Float = _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_RGBA_0_Vector4.r;
            float _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_G_5_Float = _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_RGBA_0_Vector4.g;
            float _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_B_6_Float = _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_RGBA_0_Vector4.b;
            float _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_A_7_Float = _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_RGBA_0_Vector4.a;
            float4 _Lerp_bce492cb069e4e59a2643049525877cc_Out_3_Vector4;
            Unity_Lerp_float4(_Property_5dedf12e49a14cc3bb721f377a18be47_Out_0_Vector4, _SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_RGBA_0_Vector4, (_SampleTexture2D_4d9f157699df4559a4f0c30d3cef4a62_A_7_Float.xxxx), _Lerp_bce492cb069e4e59a2643049525877cc_Out_3_Vector4);
            float2 _TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (0, 0), _TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2);
            float _Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float = _OutLineWidth;
            float2 _Vector2_faa195bfacf1410eba2bf8a5b740b9c9_Out_0_Vector2 = float2(_Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float, 0);
            float2 _Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2;
            Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_faa195bfacf1410eba2bf8a5b740b9c9_Out_0_Vector2, _Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2);
            float4 _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_d225c79b443a4582ad8f82726af00f1f_Out_2_Vector2) );
            float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_R_4_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.r;
            float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_G_5_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.g;
            float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_B_6_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.b;
            float _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_A_7_Float = _SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_RGBA_0_Vector4.a;
            float2 _Vector2_c05740733dc34f81a45ae336f800b790_Out_0_Vector2 = float2(0, _Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float);
            float2 _Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2;
            Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_c05740733dc34f81a45ae336f800b790_Out_0_Vector2, _Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2);
            float4 _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_ad1eb13d048b4e7a9138865973889f0b_Out_2_Vector2) );
            float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_R_4_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.r;
            float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_G_5_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.g;
            float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_B_6_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.b;
            float _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_A_7_Float = _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_RGBA_0_Vector4.a;
            float _Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float;
            Unity_Maximum_float(_SampleTexture2D_b93a8461cf9a460f8dc796039dd026a6_A_7_Float, _SampleTexture2D_c7d629a6bc534da3b4b140192c3e37b3_A_7_Float, _Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float);
            float _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float;
            Unity_Negate_float(_Property_fbf405b95de948b7b555433b1427de0d_Out_0_Float, _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float);
            float2 _Vector2_998698ce4be84faf89f72610d96ff181_Out_0_Vector2 = float2(_Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float, 0);
            float2 _Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2;
            Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_998698ce4be84faf89f72610d96ff181_Out_0_Vector2, _Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2);
            float4 _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_2352b709b12447a1b23bf819659f451d_Out_2_Vector2) );
            float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_R_4_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.r;
            float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_G_5_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.g;
            float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_B_6_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.b;
            float _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_A_7_Float = _SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_RGBA_0_Vector4.a;
            float2 _Vector2_27caf6eb3ecf4928bf0ce39ccdac2939_Out_0_Vector2 = float2(0, _Negate_46dcb9751f914f9c86fa6c11992c8c61_Out_1_Float);
            float2 _Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2;
            Unity_Add_float2(_TilingAndOffset_3bf918681b2d446aa6be745578184fe1_Out_3_Vector2, _Vector2_27caf6eb3ecf4928bf0ce39ccdac2939_Out_0_Vector2, _Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2);
            float4 _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.tex, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.samplerstate, _Property_7df08032e08d46bca59878bb93465521_Out_0_Texture2D.GetTransformedUV(_Add_6c44a1a2b1fd41f69cf901a6695848da_Out_2_Vector2) );
            float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_R_4_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.r;
            float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_G_5_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.g;
            float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_B_6_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.b;
            float _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_A_7_Float = _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_RGBA_0_Vector4.a;
            float _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float;
            Unity_Maximum_float(_SampleTexture2D_e442b133031641a9aace1ecf5e6ffec8_A_7_Float, _SampleTexture2D_f9d6b1ddb38c44efabe82e60c5fb8348_A_7_Float, _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float);
            float _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float;
            Unity_Maximum_float(_Maximum_579c7bafb9904ffbbfff6b3347fa6917_Out_2_Float, _Maximum_86f5a881771648d0b84eed30fc086332_Out_2_Float, _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float);
             
            surface.BaseColor = (_Lerp_bce492cb069e4e59a2643049525877cc_Out_3_Vector4.xyz);

         #ifdef UNITY_UI_CLIP_RECT
        // ここに RectMask2D の処理を書く
          const float2 pixel_size = 1 / float2(1, 1) / abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));
          const float4 clamped_rect = clamp(_ClipRect, -2e10, 2e10);
          const float2 mask_xy = IN.positionOS.xy * 2 - clamped_rect.xy - clamped_rect.zw;
          const float2 mask_zw = 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixel_size.xy));
          const float4 mask = float4(mask_xy, mask_zw);
          const half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(mask.xy)) * mask.zw);
          surface.Alpha = _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float * m.x * m.y;
         #else        
            surface.Alpha = _Maximum_b5637e6f9c5447a3870c3bceb8baba20_Out_2_Float;
         #endif

            surface.NormalTS = IN.TangentSpaceNormal;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
            output.positionOS = mul(unity_WorldToObject, float4(input.positionWS, 1.0));
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteForwardPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}