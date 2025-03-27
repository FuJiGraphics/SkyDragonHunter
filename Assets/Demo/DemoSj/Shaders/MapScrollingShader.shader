Shader "Custom/SpriteScrolling"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Offset ("Offset", Vector) = (0,0,0,0) // ��ũ�� ������
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite On // ���� ���ۿ� ����Ͽ� ������ ��������� ����

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Offset;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex) + _Offset.xy;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target // ������ȭ ���� ó������ (�׸���)
            {
                fixed4 col = tex2D(_MainTex, i.texcoord);
                col *= i.color;

                // Alpha�� 0�̸� �����ϰ� ����
                if (col.a < 0.005) discard; // ������

                return col;
            }
            ENDCG
        }
    }
}
