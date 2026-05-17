Shader "Hidden/DiffusionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        //Passは上から0になる
        //Pass指定しなかったら０番目ができる
        //0,Blur
        Pass
        {
            Name "Blur"

            Blend One Zero

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
         
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            //頂点シェーダーの入力形式
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            //頂点シェーダーからの出力→ピクセルシェーダーの入力データ
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            //データ
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            //ブラー用のパラメーター
            float _Dispersion;
            int _SmaplingTexelAmount;
            float _TexelInterval;
            float4 _Direction;

            float GetGaussianWeight(float distance)
            {
                return exp(( -distance * distance )/(2 * _Dispersion * _Dispersion))/_Dispersion; 
            }

            //頂点シェーダー
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            //ピクセルシェーダー
            float4 frag (v2f i) : SV_Target
            {
                float4 color=0;
                float totalW=0;

                for(int k=0;k<_SmaplingTexelAmount;k++)
                {
                    //中心（0）からの距離を求める
                    int x=(k-_SmaplingTexelAmount/2);
                    //その位置のウェイトをガウス式で計算
                    float weight = GetGaussianWeight(x);
                    //距離をUVへ
                    float2 offset=_Direction.xy*x;
                    //テクスチャから色を取得
                    color.rgb+= SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv + offset).rgb * weight;

                    //最後にウェイトは正規化する必要があるので、トータルのウェイトも
                    //ついでに計算しておく
                    totalW+=weight;

                }

                //ウェイトを正規化
                color/=totalW;
                color.a=1;

                return color;

            }
             ENDHLSL

        }



         //1,スクリーン合成
          Pass
        {       
            Name "Screen"

            Blend One Zero

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
         
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            //頂点シェーダーの入力形式
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            //頂点シェーダーからの出力→ピクセルシェーダーの入力データ
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            //データ
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            //データ
            TEXTURE2D(_BackTex);
            SAMPLER(sampler_BackTex);

            //パラメーター
            float4 _Blend;//合成率
         
            //頂点シェーダー
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            //ピクセルシェーダー
            float4 frag (v2f i) : SV_Target
            {
               float4 main = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv);
               float4 back =SAMPLE_TEXTURE2D(_BackTex,sampler_BackTex,i.uv);

               //backをsdr化する
               float4 sdrMainColor=saturate(main);
               float4 sdrBackColor=saturate(back);

               //Screen合成(HDRはSDRとして扱う)
               main.rgb-1-(1-sdrBackColor.rgb)*(1-main.rgb*_Blend);
               //HDR分を加算
               main.rgb+=max(0,back.rgb-1);

               return main;

            }

            ENDHLSL
        }


        //2,比較(明)
          Pass
        {       
            Name "ComparisonBright"

            Blend One Zero

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
         
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            //頂点シェーダーの入力形式
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            //頂点シェーダーからの出力→ピクセルシェーダーの入力データ
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            //データ
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            //描画先のテクスチャ
            TEXTURE2D(_BackTex);
            SAMPLER(sampler_BackTex);

            
         
            //頂点シェーダー
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            //ピクセルシェーダー
            float4 frag (v2f i) : SV_Target
            {
               float4 main = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv);
               float4 back =SAMPLE_TEXTURE2D(_BackTex,sampler_BackTex,i.uv);


             //max関数で明るい方の色を取得
               return max(main,back);

            }

            ENDHLSL
        }


         //3,乗算
          Pass
        {       
            Name "SelfMultiply"

            Blend One Zero

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
         
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            //頂点シェーダーの入力形式
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            //頂点シェーダーからの出力→ピクセルシェーダーの入力データ
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            //データ
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

           
            
            //頂点シェーダー
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            //ピクセルシェーダー
            float4 frag (v2f i) : SV_Target
            {
               float4 main = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv);
             

             //mainを2乗
               return main*main;

            }

            ENDHLSL
        }



    }
}

