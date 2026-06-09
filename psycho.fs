#version 420

in vec4 fragCoord;
in vec2 fragTexCoord;
uniform int time;

uniform sampler2D tex0;

out vec4 finalColor;

float rand(float c, float x, float y, float t)
{
	return sin(cos(c * 3.3 * x * 1.11 * y * 3243.34) + t);
}

float shift(float c, float x, float y, float t)
{
	float r = rand(c, x, y, t);
	return fract(r) / 50;
}

void main()
{
	float x = fragTexCoord.x;
	float y = fragTexCoord.y;

	float sx = floor(fragTexCoord.x * 20);
	float sy = floor(fragTexCoord.y * 20);

	float abber_rnd = float(((time / 200) * 200) % 1000);
	float dxr = shift(0.3, sx, sy, abber_rnd);
	float dyr = shift(0.3, sx, sy, abber_rnd);

	float dxg = shift(1.5, sx, sy, abber_rnd);
	float dyg = shift(1.5, sx, sy, abber_rnd);

	float dxb = shift(2.7, sx, sy, abber_rnd);
	float dyb = shift(2.7, sx, sy, abber_rnd);

	float r = texture(tex0, vec2(x + dxr, 1 - y + dyr)).x;
	float g = texture(tex0, vec2(x + dxg, 1 - y + dyg)).y;
	float b = texture(tex0, vec2(x + dxb, 1 - y + dyb)).z;

	finalColor = vec4(r, g, b, (r + g + b) > 0 ? 1 : 0);
}
