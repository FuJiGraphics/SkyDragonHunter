Shader "Unlit/TutorialMaskShader"
{
    Properties
    {
        _Color ("Color", Color) = (1, 0, 0, 0.5) // 어두운 색 + 알파
        _Rect ("Rect (xy = min, zw = max)", Vector) = (0, 0, 0, 0) // 뚫을 사각형 영역
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float4 _Rect;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 min = _Rect.xy;
                float2 max = _Rect.zw;

                // 지정된 영역(사각형)만 투명하게 뚫음
                if (i.uv.x >= min.x && i.uv.x <= max.x &&
                    i.uv.y >= min.y && i.uv.y <= max.y)
                {
                    discard;
                }

                return _Color; // 나머지는 어두운 색 출력
            }
            ENDCG
        }
    }
}
