shader_type canvas_item;

uniform float radius : hint_range(0.0, 1.5) = 0.0;
uniform vec2 center = vec2(0.5, 0.5); // normalized screen position

void fragment() {
    vec2 uv = FRAGCOORD.xy / VIEWPORT_SIZE;
    float dist = distance(uv, center);
    float mask = smoothstep(radius - 0.02, radius + 0.02, dist);
    COLOR = vec4(vec3(0.0), mask);
}
