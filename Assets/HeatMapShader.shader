Shader "Custom/HeatmapShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _Intensity ("Intensity", Range (0, 1)) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed _Intensity;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Calculate color based on position (x, y)
            fixed3 color = fixed3(IN.uv_MainTex, 0);

            // Apply intensity
            color *= _Intensity;

            // Apply the color to the pixel
            o.Albedo = color;
        }
        ENDCG
    }

    FallBack "Diffuse"
}

