Shader "Custom/UVShift"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _UVOffset ("UV Offset", Vector) = (0, 0, 0, 0)
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture",2D)="white"{}
        _ScrollSpead("ScrollSpead",float)=0.1
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _MaskTex;
            sampler2D _NoiseTex;
            float     _ScrollSpead;

            float4 _UVOffset;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //fixed2 noiseTexcord=fixed2(i.uv.x + cos(_Time.y*0.5),i.uv.y);
                fixed2 noiseTexcord=fixed2(i.uv.x + _ScrollSpead * _Time.y,i.uv.y);
                fixed4 noiseColor = tex2D(_NoiseTex,noiseTexcord)*0.05f;

                i.uv.x+=noiseColor.r;

              
                return tex2D(_MaskTex,i.uv);
               
            }
            ENDCG
        }
    }
}
