// CustomShader.fx

sampler2D TextureSampler : register(s0);

// Distortion parameters
float DistortX; // e.g., 0.05
float DistortY; // e.g., 0.05
float Time; // time in seconds

// Wave parameters
float WaveFreq; // e.g., 20
float WaveSpeed; // e.g., 3

// Scroll parameters
float ScrollX; // pixels per second or normalized speed
float ScrollY;

// Color & Transparency
float4 TintColor; // RGBA (1,1,1,1 = no change)

// Outline
float4 OutlineColor;
float OutlineThickness;

// Noise intensity
float NoiseAmount; // e.g., 0.05

// Simple pseudo-random function
float rand(float2 co)
{
    return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
}

float4 main(float2 uv : TEXCOORD0) : COLOR0
{
    // Distortion + scroll
    float waveX = sin(uv.y * WaveFreq + Time * WaveSpeed) * DistortX + frac(Time * ScrollX);
    float waveY = cos(uv.x * WaveFreq + Time * WaveSpeed) * DistortY + frac(Time * ScrollY);
    float2 distortedUV = frac(uv + float2(waveX, waveY));

    // Sample base texture
    float4 baseColor = tex2D(TextureSampler, distortedUV) * TintColor;

    // Apply noise
    if (NoiseAmount > 0)
    {
        float n = rand(distortedUV + Time);
        baseColor.rgb += (n - 0.5) * NoiseAmount; // center noise around 0
    }
    
    // If pixel has alpha, return it directly
    if (baseColor.a > 0.1)
    {
        if (OutlineThickness > 0)
        {
            return float4(0, 0, 0, 0);
        }
        else
        {
            return baseColor;
        }
    }

    // Otherwise, check surrounding pixels for outline
    float4 neighbor =
        tex2D(TextureSampler, distortedUV + float2(OutlineThickness, 0)) +
        tex2D(TextureSampler, distortedUV + float2(-OutlineThickness, 0)) +
        tex2D(TextureSampler, distortedUV + float2(0, OutlineThickness * 3)) +
        tex2D(TextureSampler, distortedUV + float2(0, -OutlineThickness * 3));

    // If any neighbor has alpha, draw outline
    if (neighbor.a > 0.1)
        return OutlineColor;

    return baseColor;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 main();
    }
}
