Shader "Unlit/StageShader"
{
    Properties{
        _BaseColor("BaseColor", color)=(1,1,1,1)


    }

    Subshader{
        Pass{
            Tags{
                "RenderType"="Transparent"
                "Queue"="Transparent"
            }
            Blend SrcAlpha OneMinusSrcAlpha 
            LOD 100

            HLSLPROGRAM

            #pragma vertex vert;
            #pragma fragment frag;
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                
            };

            struct v2f{
                float4 vertex: SV_POSITION;
                float3 viewDir: TEXCOORD0;
                float3 lightDir: TEXCOORD1;
                float3 worldNormal: TEXCOORD2; 
            };

            fixed4 _BaseColor;

            v2f vert(appdata i){
                v2f o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                
                //ワールド座標変換
                fixed4x4 mtr = unity_ObjectToWorld;

                //ワールド頂点
                float3 worldDir = mul(mtr, i.vertex).xyz;

                //法線ベクトル
                o.worldNormal = normalize(UnityObjectToWorldNormal(i.normal).xyz);

                //object ⇨ light vec
                o.lightDir = normalize(_WorldSpaceLightPos0.xyz - worldDir);

                //camera ⇨ object vec
                o.viewDir = normalize(_WorldSpaceCameraPos.xyz - worldDir);

                return o;
            }

            fixed4 frag(v2f v):SV_Target{
                //水晶用アルファ値
                fixed alpha = saturate(1 - abs(dot(v.viewDir, v.worldNormal)) + 0.2);

                //DiffuseLight
                fixed diffuse = saturate(dot(v.lightDir, v.worldNormal) + 0.2);

                fixed4 color = fixed4(diffuse * _BaseColor.xyz, alpha);

                return color;
            }

            ENDHLSL
        }
    }
}
