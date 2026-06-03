Shader "Hidden/DiffusionShader"
{
    Properties
    {
        _BackTex ("Back Texture", 2D) = "black" {}
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
        }

        ZWrite Off
        ZTest Always
        Cull Off

        // ------------------------------------------------------------
        // 0. Blur
        // ------------------------------------------------------------
        Pass
        {
          
            Name "Blur"
            Blend One Zero

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float _Dispersion;
            int _SmaplingTexelAmount;
            float4 _Direction;

            float GetGaussianWeight(float distance)
            {
                float dispersion = max(_Dispersion, 0.0001);
                return exp((-distance * distance) / (2.0 * dispersion * dispersion)) / dispersion;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float4 color = 0.0;
                float totalWeight = 0.0;

                int sampleCount = max(_SmaplingTexelAmount, 1);

                for (int k = 0; k < sampleCount; k++)
                {
                    int x = k - sampleCount / 2;
                    float weight = GetGaussianWeight(x);

                    float2 uv = input.texcoord + _Direction.xy * x;

                    float3 sampleColor = SAMPLE_TEXTURE2D_X(
                        _BlitTexture,
                        sampler_LinearClamp,
                        uv
                    ).rgb;

                    color.rgb += sampleColor * weight;
                    totalWeight += weight;
                }

                color.rgb /= max(totalWeight, 0.0001);
                color.a = 1.0;

                return color;
            }

            ENDHLSL
        }

        // ------------------------------------------------------------
        // 1. Screen
        // ------------------------------------------------------------
        Pass
        {
        Name "Screen"
        Blend One Zero

        HLSLPROGRAM
        #pragma vertex Vert
        #pragma fragment Frag

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

        TEXTURE2D_X(_BackTex);

        float _Blend;

        float4 Frag(Varyings input) : SV_Target
        {
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

            float4 back = SAMPLE_TEXTURE2D_X(
                _BackTex,
                sampler_LinearClamp,
                input.texcoord
            );

            back.a = 1.0;
            return back;
        }

            ENDHLSL
        }

        // ------------------------------------------------------------
        // 2. ComparisonBright
        // ------------------------------------------------------------
        Pass
        {
            Name "ComparisonBright"
            Blend One Zero

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            TEXTURE2D_X(_BackTex);
            SAMPLER(sampler_BackTex);

            float4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord;

                float4 main = SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv
                );

                float4 back = SAMPLE_TEXTURE2D_X(
                    _BackTex,
                    sampler_BackTex,
                    uv
                );

                float4 result = max(main, back);
                result.a = 1.0;

                return result;
            }

            ENDHLSL
        }

        // ------------------------------------------------------------
        // 3. SelfMultiply  
        // ------------------------------------------------------------
            Pass
        {
            Name "SelfMultiply"
            Blend One Zero

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float4 main = SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    input.texcoord
                );

                // 元の main * main は暗くなりすぎるので、弱めにする
                float3 multiplied = main.rgb * main.rgb;

                // 0.25 ～ 0.4 くらいがおすすめ
                main.rgb = lerp(main.rgb, multiplied, 0.3);

                main.a = 1.0;
                return main;
            }

            ENDHLSL

        }

        // ------------------------------------------------------------
        // 4. AdditiveDiffusion
        // ------------------------------------------------------------
        Pass
        {
            Name "AdditiveDiffusion"

            // 既に描かれている finalRT に加算
            Blend One One

            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float _Blend;

            float4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float4 blur = SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    input.texcoord
                );

                blur.rgb *= saturate(_Blend);
                blur.a = 1.0;

                return blur;
            }

            ENDHLSL
        }
    }
}



// Shader "Hidden/DiffusionShader"
// {
//     Properties
//     {
//         _MainTex ("Texture", 2D) = "white" {}
//     }
//     SubShader
//     {
//         //Passは上から0になる
//         //Pass指定しなかったら０番目ができる
//         //0,Blur
//         Pass
//         {
//             Name "Blur"

//             Blend One Zero

//             HLSLPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
         
//             #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

//             //頂点シェーダーの入力形式
//             struct appdata
//             {
//                 float4 vertex : POSITION;
//                 float2 uv : TEXCOORD0;
//             };

//             //頂点シェーダーからの出力→ピクセルシェーダーの入力データ
//             struct v2f
//             {
//                 float4 pos : SV_POSITION;
//                 float2 uv : TEXCOORD0;
//             };

//             //データ
//             TEXTURE2D(_MainTex);
//             SAMPLER(sampler_MainTex);

//             //ブラー用のパラメーター
//             float _Dispersion;
//             int _SmaplingTexelAmount;
//             float _TexelInterval;
//             float4 _Direction;

//             float GetGaussianWeight(float distance)
//             {
//                 return exp(( -distance * distance )/(2 * _Dispersion * _Dispersion))/_Dispersion; 
//             }

//             //頂点シェーダー
//             v2f vert (appdata v)
//             {
//                 v2f o;
//                 o.pos = TransformObjectToHClip(v.vertex.xyz);
//                 o.uv = v.uv;
//                 return o;
//             }

//             //ピクセルシェーダー
//             float4 frag (v2f i) : SV_Target
//             {
//                 float4 color=0;
//                 float totalW=0;

//                 for(int k=0;k<_SmaplingTexelAmount;k++)
//                 {
//                     //中心（0）からの距離を求める
//                     int x=(k-_SmaplingTexelAmount/2);
//                     //その位置のウェイトをガウス式で計算
//                     float weight = GetGaussianWeight(x);
//                     //距離をUVへ
//                     float2 offset=_Direction.xy*x;
//                     //テクスチャから色を取得
//                     color.rgb+= SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv + offset).rgb * weight;

//                     //最後にウェイトは正規化する必要があるので、トータルのウェイトも
//                     //ついでに計算しておく
//                     totalW+=weight;

//                 }

//                 //ウェイトを正規化
//                 color/=totalW;
//                 color.a=1;

//                 return color;

//             }
//              ENDHLSL

//         }



//          //1,スクリーン合成
//           Pass
//         {       
//             Name "Screen"

//             Blend One Zero

//             HLSLPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
         
//             #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

//             //頂点シェーダーの入力形式
//             struct appdata
//             {
//                 float4 vertex : POSITION;
//                 float2 uv : TEXCOORD0;
//             };

//             //頂点シェーダーからの出力→ピクセルシェーダーの入力データ
//             struct v2f
//             {
//                 float4 pos : SV_POSITION;
//                 float2 uv : TEXCOORD0;
//             };

//             //データ
//             TEXTURE2D(_MainTex);
//             SAMPLER(sampler_MainTex);

//             //データ
//             TEXTURE2D(_BackTex);
//             SAMPLER(sampler_BackTex);

//             //パラメーター
//             float4 _Blend;//合成率
         
//             //頂点シェーダー
//             v2f vert (appdata v)
//             {
//                 v2f o;
//                 o.pos = TransformObjectToHClip(v.vertex.xyz);
//                 o.uv = v.uv;
//                 return o;
//             }

//             //ピクセルシェーダー
//             float4 frag (v2f i) : SV_Target
//             {
//                float4 main = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv);
//                float4 back =SAMPLE_TEXTURE2D(_BackTex,sampler_BackTex,i.uv);

//                //backをsdr化する
//                float4 sdrMainColor=saturate(main);
//                float4 sdrBackColor=saturate(back);

//                //Screen合成(HDRはSDRとして扱う)
//                main.rgb-1-(1-sdrBackColor.rgb)*(1-main.rgb*_Blend);
//                //HDR分を加算
//                main.rgb+=max(0,back.rgb-1);

//                return main;

//             }

//             ENDHLSL
//         }


//         //2,比較(明)
//           Pass
//         {       
//             Name "ComparisonBright"

//             Blend One Zero

//             HLSLPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
         
//             #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

//             //頂点シェーダーの入力形式
//             struct appdata
//             {
//                 float4 vertex : POSITION;
//                 float2 uv : TEXCOORD0;
//             };

//             //頂点シェーダーからの出力→ピクセルシェーダーの入力データ
//             struct v2f
//             {
//                 float4 pos : SV_POSITION;
//                 float2 uv : TEXCOORD0;
//             };

//             //データ
//             TEXTURE2D(_MainTex);
//             SAMPLER(sampler_MainTex);

//             //描画先のテクスチャ
//             TEXTURE2D(_BackTex);
//             SAMPLER(sampler_BackTex);

            
         
//             //頂点シェーダー
//             v2f vert (appdata v)
//             {
//                 v2f o;
//                 o.pos = TransformObjectToHClip(v.vertex.xyz);
//                 o.uv = v.uv;
//                 return o;
//             }

//             //ピクセルシェーダー
//             float4 frag (v2f i) : SV_Target
//             {
//                float4 main = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv);
//                float4 back =SAMPLE_TEXTURE2D(_BackTex,sampler_BackTex,i.uv);


//              //max関数で明るい方の色を取得
//                return max(main,back);

//             }

//             ENDHLSL
//         }


//          //3,乗算
//           Pass
//         {       
//             Name "SelfMultiply"

//             Blend One Zero

//             HLSLPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
         
//             #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

//             //頂点シェーダーの入力形式
//             struct appdata
//             {
//                 float4 vertex : POSITION;
//                 float2 uv : TEXCOORD0;
//             };

//             //頂点シェーダーからの出力→ピクセルシェーダーの入力データ
//             struct v2f
//             {
//                 float4 pos : SV_POSITION;
//                 float2 uv : TEXCOORD0;
//             };

//             //データ
//             TEXTURE2D(_MainTex);
//             SAMPLER(sampler_MainTex);

           
            
//             //頂点シェーダー
//             v2f vert (appdata v)
//             {
//                 v2f o;
//                 o.pos = TransformObjectToHClip(v.vertex.xyz);
//                 o.uv = v.uv;
//                 return o;
//             }

//             //ピクセルシェーダー
//             float4 frag (v2f i) : SV_Target
//             {
//                float4 main = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv);
             

//              //mainを2乗
//                return main*main;

//             }

//             ENDHLSL
//         }



//     }
// }

