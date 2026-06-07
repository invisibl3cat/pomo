#version 420

in vec4 fragCoord;
in vec2 fragTexCoord;
uniform float time;

uniform sampler2D tex0;

out vec4 finalColor;

float rand(float c, float x, float y, float t)
{
	return sin(cos(c * 3.3 * x * 1.11 * y * 3243.34) + t * 7353.353);
}

float shift(float c, float x, float y, float t)
{
	float r = rand(c, x, y, floor(t * 3.5));
	return fract(r) / 50;
}

void main()
{
	float x = fragTexCoord.x;
	float y = fragTexCoord.y;

	float sx = floor(fragTexCoord.x * 20);
	float sy = floor(fragTexCoord.y * 20);

	float dxr = shift(0.3, sx, sy, time);
	float dyr = shift(0.3, sx, sy, time);

	float dxg = shift(1.5, sx, sy, time);
	float dyg = shift(1.5, sx, sy, time);

	float dxb = shift(2.7, sx, sy, time);
	float dyb = shift(2.7, sx, sy, time);

	float r = texture(tex0, vec2(x + dxr, 1 - y + dyr)).x;
	float g = texture(tex0, vec2(x + dxg, 1 - y + dyg)).y;
	float b = texture(tex0, vec2(x + dxb, 1 - y + dyb)).z;

	finalColor = vec4(r, g, b, (r + g + b) > 0 ? 1 : 0);
}
