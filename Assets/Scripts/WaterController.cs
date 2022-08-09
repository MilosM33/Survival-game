using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterController : MonoBehaviour
{
    public int width = 1;
    public int height = 1;
    public float spacing = 1;
    public float scale = 0.89f;
    public float time = 0;
    public float speed = 1;
    MeshFilter meshFilter;
    Mesh mesh;
    Vector3[] vertices;
    public float drag = 1;
    int[] triangles;
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        GenerateMesh();

    }


    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Item" || other.tag =="Float")
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.AddForce(transform.up*20);
            rb.AddForce(rb.velocity * -1 * drag);
            
        }
    }

    private void Update()
    {
        if(vertices != null)
        {
            UpdateWater();
        }
    } 

    void GenerateMesh()
    {
        mesh = new Mesh();
        vertices = new Vector3[(width + 1) * (height + 1)];
        Vector2[] uv = new Vector2[(width + 1) * (height + 1)];
        List<Vector3> normals = new List<Vector3>();
        for (int y = 0; y < height+1; y++)
        {
            //Precalculate height for array optimazition
            for (int x = 0; x < width+1; x++)
            {
                vertices[y * (width+1) + x] = new Vector3(x*spacing,0, y*spacing);
                uv[y * (width + 1) + x] = new Vector2((float)x/(width),(float)y/(height));
                normals.Add(Vector3.up);
            }
        }
        

        triangles = new int[(width+1)*(height+1)*6];
        int current = 0;
        for (int y = 0; y < (width + 1) * (height + 1) - width - 1; y++)
        {
            if ((y + 1) % (width + 1) == 0)
            {
                continue;
            }
            triangles[current + 0] = y;
            triangles[current + 1] = width + 1 + y;
            triangles[current + 2] = width + 1 + 1 + y;

            triangles[current + 3] = width + 1 + 1 + y;
            triangles[current + 4] = y + 1;
            triangles[current + 5] = y;

            current += 6;
        }
        
        mesh.SetVertices(vertices);
        
        mesh.SetUVs(0, uv);
        mesh.SetNormals(normals);
        mesh.SetTriangles(triangles, 0);
        meshFilter.mesh = mesh;

    }
    
    void UpdateWater()
    {
        time += Time.deltaTime*speed;
        for (int y = 0; y < vertices.Length; y++)
        {

            Vector3 temp = vertices[y];
            temp.y = Mathf.PerlinNoise(temp.x *scale+time, temp.z *scale+time);
            vertices[y] = temp;
        }

        mesh.vertices = vertices;
    }
}
