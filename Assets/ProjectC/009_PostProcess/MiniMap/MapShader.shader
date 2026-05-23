Shader "Hidden/MapShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        //Passは上から0になる
        //Pass指定しなかったら０番目ができる

        // 0, Copy
        Pass
        {
            Name "Copy"

            Blend One Zero

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
            }

            ENDHLSL
        }


        //1,Contrast
        Pass
        {
            Name "Contrast"

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

            //コントラスト用のパラメーター
            float _ContrastPower;

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
                float4 color = 0;

                color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                color = 1 / (1+exp( -_ContrastPower * (color - 0.5)));

                color.a = 1;
                return color;

            }
             ENDHLSL
        }

        //2,HSV
        Pass
        {
            /// <summary>
            /// 参考サイト
            /// https://techblog.kayac.com/unity_advent_calendar_2018_15
            /// 
            /// </summary>

            Name "HSV"

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

            //HSV用のパラメーター
            float _Hue;
            float _Saturation;
            float _Value;


            // RGB → HSV
            float3 RGBtoHSV(float3 rgb)
            {
                float3 hsv;
                float maxValue = max(rgb.r, max(rgb.g, rgb.b));
                float minValue = min(rgb.r, min(rgb.g, rgb.b));
                float delta = maxValue - minValue;

                //V 明度
                hsv.z = maxValue;

				//S 彩度
                if (maxValue != 0)
				{
					hsv.y = delta / maxValue;
				}
				else
				{
					hsv.y = 0;
				}

                //H 色相
                if(hsv.y > 0)
                {
                    if(rgb.r == maxValue)
                    {
                        hsv.x = (rgb.g - rgb.b) / delta;
					}
					else if(rgb.g == maxValue)
					{
						hsv.x = 2 + (rgb.b - rgb.r) / delta;
					}
					else
					{
						hsv.x = 4 + (rgb.r - rgb.g) / delta;
                    }

                    hsv.x /= 6;
                    if(hsv.x < 0)
                    {
                        hsv.x += 1;
                    }
                }

                return hsv;
            }

            // HSV → RGB
			float3 HSVtoRGB(float3 hsv)
			{
				float3 rgb;

                // 彩度が0の場合はRGB値はVの値
                if (hsv.y == 0)
                {
                    rgb.r = rgb.g = rgb.b = hsv.z;
                    return rgb;
                }


				float h = hsv.x * 6;
				float s = hsv.y;
				float v = hsv.z;

				float c = v * s;
				float x = c * (1 - abs(fmod(h, 2) - 1));
				float m = v - c;

				if (h < 1)
				{
					rgb = float3(c, x, 0);
				}
				else if (h < 2)
				{
					rgb = float3(x, c, 0);
				}
				else if (h < 3)
				{
					rgb = float3(0, c, x);
				}
				else if (h < 4)
				{
					rgb = float3(0, x, c);
				}
				else if (h < 5)
				{
					rgb = float3(x, 0, c);
				}
				else
				{
					rgb = float3(c, 0, x);
				}

				return rgb + m;
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
                float4 color = 0;

                color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                float3 hsv = RGBtoHSV(color.rgb);

                hsv.x += _Hue;
                hsv.y *= _Saturation;
                hsv.z += _Value;
                
                color.rgb = HSVtoRGB(hsv);

                return color;

            }
             ENDHLSL
        }

        //3,GrayToon
        Pass
        {
            Name "GrayToon"

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

            //コントラスト用のパラメーター
            float4 _WhiteColor;
            float4 _GrayColor;
            float4 _DarkColor;

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
                float4 color = 0;

                color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                float grayscale = dot(color.rgb, float3(0.299, 0.587, 0.114));

                // ちょっと境界をぼかしたい

                if(grayscale < 0.17)
				{
					color.rgb = _DarkColor.rgb;
				}
				else if(grayscale < 0.3)
                {
					color.rgb = _GrayColor.rgb;
                }
				else
				{
					color.rgb = _WhiteColor.rgb;
				}

                return color;

            }
             ENDHLSL
        }


    }
}

