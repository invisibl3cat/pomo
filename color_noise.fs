#version 420

in vec2 fragTexCoord;
uniform float time;

uniform sampler2D tex0;

out vec4 finalColor;

float rand(float c, float x, float y, float t)
{
	return fract(sin(dot(vec2(x, y), vec2(12.9898, 78.233)) * (t + c)) * 43758.5453);
}

float shift(float c, float x, float y, float t)
{
	float r = rand(c, x, y, floor(t * 8.5) / 8.5);
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

	float dxr = shift(txl.x, x, y, time);
	float dyr = shift(txl.x, x, y, time);

	float dxg = shift(txl.y, x, y, time);
	float dyg = shift(txl.y, x, y, time);

	float dxb = shift(txl.z, x, y, time);
	float dyb = shift(txl.z, x, y, time);

	float r = texture(tex0, vec2(clamp(x + dxr), clamp(1 - y + dyr))).x;
	float g = texture(tex0, vec2(clamp(x + dxg), clamp(1 - y + dyg))).y;
	float b = texture(tex0, vec2(clamp(x + dxb), clamp(1 - y + dyb))).z;

	finalColor = vec4(r, g, b, (r + g + b) > 0 ? 1 : 0);
}
