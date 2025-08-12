// CRT.fx
// HLSL pixel shader (ps_3_0) that simulates a CRT look using ONLY the Resolution uniform.
// Usage: set sampler TextureSampler to your rendered scene and set Resolution to viewport size in pixels.

sampler2D TextureSampler : register(s0);

float2 Resolution; // in pixels (width, height)

// helper: saturate for ps_3_0
float saturate_f(float x) { return clamp(x, 0.0, 1.0); }

float4 main(float2 uv : TEXCOORD0) : COLOR0
{
    // uv expected in 0..1
    // convert to pixel space
    float2 px = uv * Resolution;

    // aspect correction for radial effects
    float aspect = Resolution.x / Resolution.y;

    // --- Curvature (simple barrel-distortion-like) ---
    // move uv into -0.5..0.5 space
    float2 center = uv - 0.5;
    // correct X by aspect so curvature looks right on wide screens
    float2 c = float2(center.x * aspect, center.y);
    float r2 = dot(c, c);
    // strength scales with resolution (small screens less obvious)
    float curveStrength = 0.01; // feel free to tweak
    float2 uv_curved = uv + center * (r2 * curveStrength);

    // clamp to avoid sampling outside
    uv_curved = saturate(uv_curved);

    // --- Chromatic aberration ---
    // small offset proportional to curvature and distance from center
    float caAmount = 0.0025; // small in UV-space (based on 1080p-ish)
    // scale CA by r2 so it increases toward edges
    float2 caOffset = center * caAmount * (1.0 + r2 * 2.0);

    // sample each channel with slight offsets
    float4 colR = tex2D(TextureSampler, uv_curved + caOffset);
    float4 colG = tex2D(TextureSampler, uv_curved);
    float4 colB = tex2D(TextureSampler, uv_curved - caOffset);

    float3 col = float3(colR.r, colG.g, colB.b);

    // --- Scanlines ---
    // Create thin horizontal scanlines using a sin function on pixel Y
    // Frequency scales with resolution so it remains subtle on all displays
    float scanFreq = 1.5 * (Resolution.y / 480.0); // adjust base frequency
    // Compute a smooth scanline multiplier (0.0..1.0)
    float scan = 0.5 + 0.5 * sin(px.y * scanFreq * 3.14159);
    // Make scanlines subtle and use a power to shape darkness
    float scanIntensity = lerp(0.85, 1.05, scan);
    col *= scanIntensity;

    // --- Phosphor/Gain and Contrast ---
    // slight contrast boost and soft clipping
    float contrast = 1.08;
    float gain = 1.02;
    col = (col - 0.5) * contrast + 0.5;
    col *= gain;

    // --- Vignette / darken corners ---
    // stronger toward corners using radial falloff
    //float2 v = center;
    //v.x *= aspect; // correct for aspect
    //float dist = length(v);
    // vignette: smoothstep from inner radius to outer radius
    //float vignette = saturate_f(1.0 - smoothstep(0.45, 0.9, dist));
    // give the vignette a soft power curve
    //vignette = pow(vignette, 1.25);
    //col *= vignette;

    // --- CRT Phosphor Bloom (tiny blur-like softening) ---
    // emulate slightly soft edges by adding a tiny mix of neighbor samples
    float2 pxUV = 1.0 / Resolution;
    float3 neighbor = tex2D(TextureSampler, uv_curved + float2(pxUV.x, 0)).rgb
                    + tex2D(TextureSampler, uv_curved - float2(pxUV.x, 0)).rgb
                    + tex2D(TextureSampler, uv_curved + float2(0, pxUV.y)).rgb
                    + tex2D(TextureSampler, uv_curved - float2(0, pxUV.y)).rgb;
    neighbor *= 0.25;
    // blend very subtly to avoid heavy blur
    col = lerp(col, neighbor, 0.06);

    // --- Final color grading: emulate subtle CRT warmness ---
    float3 grade = float3(1.02, 1.01, 0.98);
    col *= grade;

    // final clamp and gamma-ish tweak
    col = saturate(col);
    // slight s-curve: simple approximation with pow to keep it cheap
    col = pow(col, float3(0.95, 0.95, 0.95));

    return float4(col, 1.0);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 main();
    }
}
