Shader "CorgiRebrushed/BrushStroke"
{
    Properties
    {
        _Softness ("Edge Softness", Range(0.01, 0.49)) = 0.15
        _Opacity  ("Opacity",       Range(0.0,  1.0))  = 1.0
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex   Vert
            #pragma fragment Frag
            #include "UnityCG.cginc"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            float _Softness;
            float _Opacity;

            Varyings Vert(Attributes v)
            {
                Varyings o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.uv;
                return o;
            }

            fixed4 Frag(Varyings i) : SV_Target
            {
                float dist  = distance(i.uv, float2(0.5, 0.5));
                float alpha = 1.0 - smoothstep(0.5 - _Softness, 0.5, dist);
                return fixed4(1.0, 1.0, 1.0, alpha * _Opacity);
            }
            ENDCG
        }
    }
}
