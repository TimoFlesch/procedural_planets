using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    this class renders a sphere.
    The sphere is actually a cube, consisting of six faces.
    the faces are morphed from flat to concave, turning the cube into a sphere.
    All credit for this code goes to Seb Lague, this file is a shameless copy x)
    Methods:
    - PlanetFace: contructor, passes variables 
    - ConstructMesh: generates a sphere and applies shape deformations
 */
public class PlanetFace {

    ShapeGenerator shapeGenerator;
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;

    /*
        constructor
     */
    public PlanetFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh           =           mesh;
        this.resolution     =     resolution;
        this.localUp        =        localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB =                Vector3.Cross(localUp, axisA);
    }
    
    /*
        generates mesh
     */
    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                // turn sphere into noisy planet:
                vertices[i] = shapeGenerator.UpdateShape(pointOnUnitSphere);
                
                // some scaling:
                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices  =  vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
