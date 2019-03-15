using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColourSettings : ScriptableObject {

    // adjustable colour gradient (currently only via GUI)
    // look here to learn how to do this in script https://docs.unity3d.com/ScriptReference/Gradient.html
    public Gradient gradient;

    // the shader
    public Material planetMaterial;
} 
