Shader "Custom/UI/TwoColorGradShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color1", Color) = (1,1,0,1)
        _Color2 ("Color2", Color) = (1,0,1,1)
        _Alpha ("Alpha", Range(0, 1)) = 1

        _Stencil ("Stencil Reference", Float) = 0
        _StencilComp ("Stencil Comparison", Float) = 8
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15 // 15はRGBA全てを有効にする
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "IgnoreProjector"="False" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off     
        ZTest Always 

        LOD 100

        // ステンシルバッファ設定
        Stencil {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask] // ColorMaskの設定

        /// 縦に2色のグラデーション
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Color1;
            fixed4 _Color2;

            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 gradCol = lerp(_Color2, _Color1, i.uv.y);
                
                fixed4 col = tex2D(_MainTex, i.uv);
                gradCol.a = col.a;
                if (gradCol.a > _Alpha)
                {
                    gradCol.a = _Alpha;
                }

                return gradCol;
            }
            ENDCG
        }
    }
}
