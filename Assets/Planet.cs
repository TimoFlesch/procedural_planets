using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    // global parameters
    [Range(2,256)]
    public int resolution = 10; // the spatial resolution of the mesh grid (i.e. density of vertices)
    public bool autoUpdate = true; // update the object whenever a value is being changed
    public enum FaceRenderMask {All, Top, Bottom, Left, Right, Front, Back};
    public FaceRenderMask faceRenderMask;

    // settings 
    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;


    // gui editor: foldout for settings
    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colourSettingsFoldout;

    // transforms sphere into "planet" by applying simplex noise:
    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColourGenerator colourGenerator = new ColourGenerator();

    // the meshes that comprise a planet are defined here
    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;
     

	void Initialize()
    {
        // on init: construct a new surface generator
        // shapeGenerator = new ShapeGenerator(shapeSettings);
        // colourGenerator = new ColourGenerator(colourSettings);

        // new: on init, just update settings 
        shapeGenerator.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);

        // initialise is called multiple times, e.g. whenever settings have been updated, thus restrict construction of meshes to first call
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        // the planet is actually a cube (with six faces). To generate a sphere, the faces are inflated (i.e. bend outwards)  
        
        // transform the meshes into a sphere!
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                // meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask -1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    void GenerateMesh()
    {
        // foreach (TerrainFace face in terrainFaces)
        // {
        //     face.ConstructMesh();
        // }

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh(); // show only active mesh 
            }
        }

        colourGenerator.updateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColours()
    {
        foreach (MeshFilter m in meshFilters)
        {
            // m.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.planetColour;
            colourGenerator.UpdateColours();
        }
    }
}
