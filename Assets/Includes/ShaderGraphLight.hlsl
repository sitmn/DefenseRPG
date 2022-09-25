#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

void CalculateMainLight_float(out float3 Direction, out float3 Color) {
	#ifdef SHADERGRAPH_PREVIEW
		Direction = float3(0.5, 0.5, 0);
		Color = 1;
	#else
		Light mainLight = GetMainLight();
		Direction = mainLight.direction;
		Color = mainLight.color;
	#endif
}


#endif