Shader "Unlit/CrystalShader"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        //透過用のタグ
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha 

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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                fixed3x3 worldMatrix = unity_ObjectToWorld;
                o.normal = normalize(UnityObjectToWorldNormal(v.normal));
                o.viewDir = normalize(_WorldSpaceCameraPos - mul(worldMatrix, v.vertex.xyz));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed alpha = 1 - abs(dot(i.normal, i.viewDir));

                // sample the texture
                fixed4 col = fixed4(_Color.xyz, alpha);
                
                return col;
            }
            ENDCG
        }
    }
}
