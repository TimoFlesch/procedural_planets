using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidgeNoiseFilter : INoiseFilter
{
  /*
  applies custom settings to noise generated via Simplex algorithm
  */
    NoiseSettings settings;
    Noise noise = new Noise();

    public RidgeNoiseFilter(NoiseSettings settings)
    {
      this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
      float noiseValue = 0;
      float frequency  = settings.baseRoughness;
      float amplitude  = 1;
      float weight     = 1;
      for (int i = 0; i < settings.numLayers; i++)
      {
          float v = 1- Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
          v *= v; // ridges more pronounced
          v *= weight; // first layers have ..
          weight = v;  // ... less detail than last layers
          noiseValue += v*amplitude;
          
          frequency *= settings.roughness; // when roughness > 1, freq increases with each layer. ow it decreases
          amplitude *= settings.persistence; // when persistence < 1: amplitude will decrease with each layer
      }
      noiseValue = Mathf.Max(0,noiseValue-settings.minValue);
      return noiseValue * settings.strength;

    }
}
