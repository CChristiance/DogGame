shader_type canvas_item;
uniform sampler2D screen_tex: hint_screen_texture, filter_linear;

void fragment() {
	vec3 color = texture(screen_tex, SCREEN_UV).rgb;
	float r = color.r;
	float g = color.g;
	float b = color.b;
	
	float sepia_red = r * 0.393 + g * 0.769 + b * 0.189;
	float sepia_green = r * 0.349 + g * 0.686 + b * 0.168;
	float sepia_blue = r * 0.272 + g * 0.534 + b * 0.131;
	
	COLOR.rgb = vec3(sepia_red, sepia_green, sepia_blue);
	COLOR.a = texture(screen_tex, SCREEN_UV).a;
}
