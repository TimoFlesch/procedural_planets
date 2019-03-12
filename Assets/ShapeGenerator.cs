using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator {

    ShapeSettings settings;
    INoiseFilter[] noiseFilters;
    public MinMax elevationMinMax;
    
    public void UpdateSettings(ShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
            
        }
        elevationMinMax = new MinMax();
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)    
    {
        /* 
        distorts point on planet by applying user-defined simplex noise
         */
        float elevation = 0;
        // store val of first layer to use it as mask for subsequent layers 
        float firstLayerValue = 0;        
        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {   
            if (settings.noiseLayers[i].enabled)
            {
                // if first layer set as mask in editor, use it as mask for subsequent layers, ow set to 1
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;    
            }
            
        }
        elevation = settings.planetRadius * (1+elevation);
        elevationMinMax.AddValue(elevation); // keep track of minimum and maximum elevation of all vertices
        return pointOnUnitSphere * elevation;
        
    }
}
