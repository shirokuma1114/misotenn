
float2 Random2(float2 uv)
{
    float2 st = float2(dot(uv, float2(127.1, 311.7)), dot(uv, float2(269.5, 183.3)));
    return -1.0 + 2.0 * frac(sin(st) * 43758.5453123);
}

void WaterSurface_float
(float2 uv, out float3 output)
{
    // ƒp[ƒŠƒ“
    float2 p = floor(uv);
    float2 f = frac(uv);
    float2 u = f * f * (3.0 - 2.0 * f);

    float v00 = Random2(p + float2(0, 0));
    float v10 = Random2(p + float2(1, 0));
    float v01 = Random2(p + float2(0, 1));
    float v11 = Random2(p + float2(1, 1));

    float2 height = lerp(
                lerp(dot(v00, f - float2(0, 0)), dot(v10, f - float2(1, 0)), u.x),
                lerp(dot(v01, f - float2(0, 1)), dot(v11, f - float2(1, 1)), u.x),
                u.y
                ) + 0.5f;
    
    // –@ü
    output = cross(
                float3(0, ddy(height.y), 1),
                float3(1, ddx(height.x), 0)
                );

}