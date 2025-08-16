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

float4 main(float2 uv : TEXCOORD0) : COLOR0
{
    float waveX = sin(uv.y * WaveFreq + Time * WaveSpeed) * DistortX + frac(Time * ScrollX);
    float waveY = cos(uv.x * WaveFreq + Time * WaveSpeed) * DistortY + frac(Time * ScrollY);
    float2 distortedUV = frac(uv + float2(waveX, waveY));
    return tex2D(TextureSampler, distortedUV);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 main();
    }
}
