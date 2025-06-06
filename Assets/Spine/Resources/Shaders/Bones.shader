Shader "Hidden/Spine/Bones" {
    SubShader {
        Tags { "RenderType"="Opaque" }
        Pass {
            ColorMaterial AmbientAndDiffuse
            Lighting Off
            Cull Off
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 _Color;

            fixed4 frag(v2f i) : SV_Target {
                return _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
