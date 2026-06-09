#include "raylib.h"

#include <chrono>
#include <cmath>
#include <format>
#include <iostream>
#include <string>

#define TEX_SCALE 0.5
#define MODE_TEXT_SIZE 40
#define TIMER_TEXT_SIZE 64

// #define HOT_RELOAD

auto nice_time(int secs) {
	int hours = secs / 3600;
	int minutes = (secs - hours * 3600) / 60;
	int seconds = secs % 60;

	return std::format("{:02}:{:02}:{:02}", hours, minutes, seconds);
}

auto load_rendering_assets(bool is_focus, Shader *shader, Texture2D *tex)
{
	if (is_focus) {
		*shader = LoadShader(nullptr, "color_noise.fs");
		*tex = LoadTexture("focus.png");
	} else {
		*shader = LoadShader(nullptr, "psycho.fs");
		*tex = LoadTexture("relax.png");
	}
}

auto main(int argc, char **argv) -> int
{
	if (argc < 3) {
		std::cerr << "Invalid arguments, expected FOCUS_TIME and RELAX_TIME in seconds\n";

		return EXIT_FAILURE;
	}

	auto focus_time = std::stoi(argv[1]);
	auto relax_time = std::stoi(argv[2]);

	if (focus_time <= 0) {
		std::cerr << "Nonsensical focus time\n";

		return EXIT_FAILURE;
	}
	if (focus_time <= 0) {
		std::cerr << "Nonsensical relax time\n";

		return EXIT_FAILURE;
	}

	float time_remaining = focus_time;
	bool is_focus = true;

	const int W = 800;
	const int H = 800;

	InitWindow(W, H, "...");
	SetTargetFPS(60);

	RenderTexture2D target = LoadRenderTexture(W, H);
	Texture2D tex;
	Shader shader;

	load_rendering_assets(is_focus, &shader, &tex);

	unsigned int t_elapsed_msec = 0;
	auto t1 = std::chrono::system_clock::now();
	while (!WindowShouldClose()) {
		BeginBlendMode(BLEND_ALPHA);
		BeginTextureMode(target);
			ClearBackground(BLACK);
			DrawTextureEx(tex, Vector2(W / 2 - tex.width / 2 * TEX_SCALE, H / 2 - tex.height / 2 * TEX_SCALE), 0, TEX_SCALE, WHITE);

		EndTextureMode();

#ifdef HOT_RELOAD
		UnloadShader(shader);
		UnloadTexture(tex);
		load_rendering_assets(is_focus, &shader, &tex);
#endif /* HOT_RELOAD */

		BeginDrawing();
		BeginBlendMode(BLEND_ALPHA);
			ClearBackground(BLACK);

			if (IsShaderValid(shader))
			{
				int shaderTime = GetShaderLocation(shader, "time");
				int rnd = int(t_elapsed_msec);
				SetShaderValue(shader, shaderTime, &rnd, SHADER_UNIFORM_INT);

				BeginShaderMode(shader);
				DrawTexture(target.texture, 0, 0, BLANK);
				EndShaderMode();
			}

			const char *mode = is_focus ? "FOCUS" : "RELAX";
			auto text_w = MeasureText(mode, MODE_TEXT_SIZE);
			DrawText(mode, W / 2 - text_w / 2, 32, MODE_TEXT_SIZE, WHITE);

			auto time_string = nice_time(std::ceil(time_remaining));
			text_w = MeasureText(time_string.c_str(), TIMER_TEXT_SIZE);
			auto timer_color = time_remaining <= 5 ? RED : WHITE;
			DrawText(time_string.c_str(), W / 2 - text_w / 2, H - 100, TIMER_TEXT_SIZE, timer_color);
		EndDrawing();
		EndBlendMode();

		auto t2 = std::chrono::system_clock::now();
		unsigned int dt_msec = std::chrono::duration_cast<std::chrono::milliseconds>(t2 - t1).count();
		time_remaining -= float(dt_msec) / 1000.f;
		t_elapsed_msec += dt_msec;
		t1 = t2;
		if (time_remaining <= 0) {
			is_focus = !is_focus;

			time_remaining = is_focus ? focus_time : relax_time;

			UnloadShader(shader);
			UnloadTexture(tex);
			load_rendering_assets(is_focus, &shader, &tex);
		}
	}

	CloseWindow();

	return 0;
}
