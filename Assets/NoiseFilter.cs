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
      float noiseValue = (noise.Evaluate(point * settings.roughness + settings.centre)+1)*.5f;
      return noiseValue * settings.strength;

    }
}
