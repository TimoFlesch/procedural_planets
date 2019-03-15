using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    all settings related to the shape of the planet.
    global: planetRadius
    layer-specific: noiseSettings
    
 */
[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject {

    public float planetRadius = 1;
    public NoiseLayer[] noiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool enabled  = true;
        public bool useFirstLayerAsMask;
        public NoiseSettings noiseSettings;
    }
}

