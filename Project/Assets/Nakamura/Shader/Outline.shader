Shader "Unlit/Outline"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex += float4(v.normal * 0.01f, 0);   //アウトラインの太さを変える
                o.vertex = UnityObjectToClipPos(v.vertex); 
                return o;
            }
            
            //アウトラインの色
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(0.2, 0.2, 0.2, 1);                
                return col;
            }
            ENDCG
        }

        Pass
        {
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {   
                float r = 1.0f;
                float g = 1.0f;
                float b = 1.0f;

                half nl = max(0, dot(i.normal, _WorldSpaceLightPos0.xyz));
                //一番暗い影
                if( nl <= 0.25f ) 
                {
                    r = 0.44f;
                    g = 0.67f;
                    b = 0.36f;
                }
                //間
                else if( nl <= 0.26f && nl > 0.25f) 
                {
                    r = 0.0f;
                    g = 0.0f;
                    b = 0.0f;
                }
                ////真ん中色
                //else if( nl <= 0.9f) 
                //{
                //    r = 0.68f;
                //    g = 0.96f;
                //    b = 0.58f;
                //}
                ////間
                //else if( nl <= 0.905f && nl > 0.9f) 
                //{
                //    r = 0.0f;
                //    g = 0.0f;
                //    b = 0.0f;
                //}
                //明るいところ
                else
                {
                    r = 0.68f;
                    g = 0.96f;
                    b = 0.58f; 
                }

                fixed4 col = fixed4(r, g, b, 1);
                return col;
            }
            ENDCG
        }
    }
}