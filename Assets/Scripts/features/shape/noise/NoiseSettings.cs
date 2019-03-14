using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // show up in editor
public class NoiseSettings
{
  public enum FilterType {Smooth,Ridge};
  public FilterType filterType;

  // [ConditionalHide("filterType",0)]
  public SmoothNoiseSettings smoothNoiseSettings; // attribute ensures that this is only displayed when smooth noise is selected from dropdown
  // [ConditionalHide("filterType",1)]
  public RidgeNoiseSettings ridgeNoiseSettings;
    
  [System.Serializable]
  public class SmoothNoiseSettings {
    
    // spatial frequency of noise of 1st layer
    public float baseRoughness = 1;
    // change in noise amplitude with each layer
    // with each additional layer, amplitude either decreases (<1) or increases (>1)
    public float persistence = .5f;

    // amplitude of noise 
    public float strength  = 1;
    // spatial frequency of noise
    public float roughness = 1;
    [Range(1,8)]
    public int numLayers = 1;
    
    public Vector3 centre;

    public float minValue;
  }

  [System.Serializable]
  public class RidgeNoiseSettings : SmoothNoiseSettings
  {
    public float weightMultiplier = .8f;
  }
  

}
