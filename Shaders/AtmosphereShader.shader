Shader "Custom/Atmosphere"
{
    Properties
    {
        _AtmosphereColor ("Atmosphere Color", Color) = (0.3, 0.6, 1.0, 0.3)
        _Thickness ("Thickness", Range(0.01, 0.5)) = 0.1
        _Power ("Rim Power", Range(0.5, 8.0)) = 3.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Front
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; float3 normal : NORMAL; };
            struct v2f { float4 pos : SV_POSITION; float rimFactor : TEXCOORD0; };

            float4 _AtmosphereColor;
            float _Power;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                float3 viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
                o.rimFactor = 1.0 - saturate(dot(worldNormal, viewDir));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float rim = pow(i.rimFactor, _Power);
                return fixed4(_AtmosphereColor.rgb, rim * _AtmosphereColor.a);
            }
            ENDCG
        }
    }
}