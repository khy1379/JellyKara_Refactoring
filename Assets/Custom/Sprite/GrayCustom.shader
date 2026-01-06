Shader "Custom/FixedGrayscale"
{
    Properties
    {
        // SpriteRenderer/Image 컴포넌트가 텍스처와 색상을 전달할 수 있도록 필수적으로 유지
        _MainTex ("Sprite Texture", 2D) = "white" {} 
        _Color ("Tint Color", Color) = (1,1,1,1) // 흰색(1,1,1,1)으로 설정 유지
    }
    SubShader
    {
        // (이전 코드와 동일한 Tags, Blend, ZWrite 설정)
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        CGPROGRAM
        #pragma surface surf NoLighting alpha

        sampler2D _MainTex;
        fixed4 _Color; // C#에서 제어하지 않더라도 유지

        struct Input
        {
            float2 uv_MainTex;
        };

        inline fixed4 LightingNoLighting (SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            return fixed4(s.Albedo, s.Alpha);
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            // 1. 텍스처와 _Color(흰색)를 곱해서 색상을 가져옵니다. 
            // **이 곱셈은 SpriteRenderer/Image가 텍스처를 셰이더에 제대로 전달하는 데 중요한 역할을 합니다.**
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex); 
            
            // 2. 흑백 값 계산 (Luminance 공식)
            //float gray = dot(c.rgb, float3(0.299, 0.587, 0.114));
            float gray = dot(c.rgb, float3(0.1495, 0.2935, 0.057));
            
            // 3. 최종 색상은 흑백 값으로 고정 (lerp 불필요)
            fixed3 finalColor = fixed3(gray, gray, gray);

            // 4. 최종 출력
            o.Albedo = finalColor;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Sprites/Default"
}