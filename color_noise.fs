#version 420

in vec2 fragTexCoord;
uniform int time;

uniform sampler2D tex0;

out vec4 finalColor;

float rand(float c, float x, float y, float t)
{
	return fract(sin(dot(vec2(x, y), vec2(12.9898, 78.233)) * (t + c)) * 43758.5453);
}

float shift(float c, float x, float y, float t)
{
	float r = rand(c, x, y, t);
	return fract(r) / 200;
}

float clamp(float v)
{
	if (v < 0.0) return 0.0;
	else if (v > 1.0) return 1.0;
	return v;
}

void main()
{
	float x = fragTexCoord.x;
	float y = fragTexCoord.y;

	vec4 txl = texture(tex0, vec2(x, y));

	float noise_rnd = float((time / 100) * 100);
	float dxr = shift(txl.x, x, y, noise_rnd);
	float dyr = shift(txl.x, x, y, noise_rnd);

	float dxg = shift(txl.y, x, y, noise_rnd);
	float dyg = shift(txl.y, x, y, noise_rnd);

	float dxb = shift(txl.z, x, y, noise_rnd);
	float dyb = shift(txl.z, x, y, noise_rnd);

	float r = texture(tex0, vec2(clamp(x + dxr), clamp(1 - y + dyr))).x;
	float g = texture(tex0, vec2(clamp(x + dxg), clamp(1 - y + dyg))).y;
	float b = texture(tex0, vec2(clamp(x + dxb), clamp(1 - y + dyb))).z;

	finalColor = vec4(r, g, b, (r + g + b) > 0 ? 1 : 0);
}
