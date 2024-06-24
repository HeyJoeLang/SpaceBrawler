using UnityEngine;

public class InvertNormals : MonoBehaviour
{
    void Start()
    {
        InvertMeshNormals();
    }

    void InvertMeshNormals()
    {
        // Get the mesh filter component
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;

            // Invert normals
            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -normals[i];
            }
            mesh.normals = normals;

            // Invert triangles winding order
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                int[] triangles = mesh.GetTriangles(i);
                for (int j = 0; j < triangles.Length; j += 3)
                {
                    int temp = triangles[j];
                    triangles[j] = triangles[j + 1];
                    triangles[j + 1] = temp;
                }
                mesh.SetTriangles(triangles, i);
            }
        }
        else
        {
            Debug.LogError("MeshFilter not found!");
        }
    }
}