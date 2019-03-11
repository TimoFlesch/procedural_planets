using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // show up in editor
public class NoiseSettings
{
  public float strength  = 1;
  public float roughness = 1;
  [Range(1,8)]
  public int numLayers = 1;
  public float baseRoughness = 1;
  public float persistence = .5f;
  public Vector3 centre;

  public float minValue;

}
