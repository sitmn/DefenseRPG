Shader "Unlit/StageGridShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _SubTex ("Sub Texture", 2D) = "white"{}
        _MaskTex ("Mask Texture", 2D) = "white"{}
        _NormalTex ("Normal Texture", 2D) = "white"{}
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float3 normal : NORMAL;
                float3 tangent : TANGENT;
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 tangent : TEXCOORD2;
                float3 binormal : TEXCOORD3;
            };

            sampler2D _MainTex;
            sampler2D _SubTex;
            sampler2D _MaskTex;
            sampler2D _NormalTex;
            float4 _MainTex_ST;

            /// <summary>
            /// ステージのGridを作成するメソッド
            /// </summary>
            /// <param name="st">frac関数で分割したuv座標</param>
            /// <param name="size">１つのGridにおける線の境界座標</param>
            /// <returns>Grid部分の座標に0,それ以外の座標に1を返す</returns>
            float4 box(float2 st,float size){
                //境界座標の計算
                size = 1 - (size / 2);
                //stまたは1.0-stのどちらかが0であれば、0となる
                st = step(st, size) * step(1.0 - st, size);
                float magnification = st.x * st.y;
                magnification = (magnification == 0)? 0.8 : magnification;
                return magnification;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                fixed4x4 objectToWorld = unity_ObjectToWorld;
                //主法線、従法線、接戦をワールド座標に変換
                o.normal = normalize(mul(v.normal, objectToWorld));
                o.tangent = normalize(mul(v.tangent, objectToWorld));
                o.binormal = normalize(mul(cross(v.normal, v.tangent), objectToWorld));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //法線マップをワールドスペースに変換
                fixed3 normalMap = tex2D(_NormalTex, i.uv);
                normalMap = (normalMap - 0.5f) * 2.0f;
                normalMap = i.tangent * normalMap.x
                            + i.binormal * normalMap.y
                            + i.normal * normalMap.z;
                //法線マップの拡散反射光
                float3 lig = max(0.0f, dot(normalMap, - normalize(float3(0,100,0)))) * 2 * _LightColor0;
                //鏡面反射光
                lig += 0.8;
                //lig = lig * lig;

                fixed4 color1 = tex2D(_MainTex, i.uv);
                fixed4 color2 = tex2D(_SubTex, i.uv);
                fixed4 p = tex2D(_MaskTex, i.uv);
                //法線マップの光を反映
                color1.rgb *= lig;
                fixed4 sv = color1 * p + color2 * (1 - p);
                //縦横を分割
                float2 st = frac(i.uv * float2(8,5));
                //Grid部を作成
                sv = sv * box(st, 0.035);
                return sv;
            }
            ENDCG
        }
    }
}
