using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
  /*
  applies custom settings to noise generated via Simplex algorithm
  */
    NoiseSettings settings;
    Noise noise = new Noise();

    public NoiseFilter(NoiseSettings settings)
    {
      this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
      float noiseValue = 0;
      float frequency = settings.baseRoughness;
      float amplitude = 1;
      for (int i = 0; i < settings.numLayers; i++)
      {
          float v = noise.Evaluate(point * frequency + settings.centre);
          noiseValue += (v+1)*.5f*amplitude;
          frequency *= settings.roughness; // when roughness > 1, freq increases with each layer. ow it decreases
          amplitude *= settings.persistence; // when persistence < 1: amplitude will decrease with each layer
      }
      noiseValue = Mathf.Max(0,noiseValue-settings.minValue);
      return noiseValue * settings.strength;

    }
}
