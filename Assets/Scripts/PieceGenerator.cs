using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PieceGenerator : MonoBehaviour
{
    //serialized
    [Header("Dimensions")]
    [SerializeField] private float width = 3f;
    [SerializeField] private float height = 1f;
    [SerializeField] private float depth = 1f;

    [Header("Textures")]
    [SerializeField] private Texture2D[] RollOne;
    [SerializeField] private Texture2D[] RollTwo;
    [SerializeField] private Texture2D[] RollThree;
    [SerializeField] private Texture2D[] RollFour;
    [SerializeField] private Texture2D[] RollFive;
    [SerializeField] private Texture2D[] RollSix;
    [SerializeField] private Texture2D[] RollSeven;

    public void CreatePiece(Quaternion rotation)
    {
        //give piece a collider
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(width, height, depth);

        //give a rigidbody
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 10f;
        rb.drag = 0f;
        rb.useGravity = true;

        //mesh things
        Vector3[] vertices = new Vector3[24];
        int[] triangles = new int[36];
        Vector2[] uvs = new Vector2[24];

        //get half lengths
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;
        float halfDepth = depth / 2f;

        //define vertices
        Vector3[,] faceVertices =
        {
            // long front face
            {
                new Vector3(halfWidth, -halfHeight, -halfDepth),  // bottom right
                new Vector3(-halfWidth, -halfHeight, -halfDepth), // bottom left
                new Vector3(-halfWidth, halfHeight, -halfDepth),   // top left
                new Vector3(halfWidth, halfHeight, -halfDepth)   // top right
            },
            // long back face
            {
                new Vector3(-halfWidth, -halfHeight, halfDepth),  // bottom right
                new Vector3(halfWidth, -halfHeight, halfDepth),   // bottom left
                new Vector3(halfWidth, halfHeight, halfDepth),     // top left
                new Vector3(-halfWidth, halfHeight, halfDepth),   // top right
            },
            // long top face
            {
                new Vector3(halfWidth, halfHeight, -halfDepth),   // bottom left
                new Vector3(-halfWidth, halfHeight, -halfDepth),  // bottom right
                new Vector3(-halfWidth, halfHeight, halfDepth),   // top right
                new Vector3(halfWidth, halfHeight, halfDepth)     // top left
            },
            // long bottom face
            {
                new Vector3(halfWidth, -halfHeight, halfDepth),   // bottom right
                new Vector3(-halfWidth, -halfHeight, halfDepth),  // bottom left
                new Vector3(-halfWidth, -halfHeight, -halfDepth),  // top left
                new Vector3(halfWidth, -halfHeight, -halfDepth)  // top right
            },
            // square left face
            {
                new Vector3(-halfWidth, -halfHeight, -halfDepth),  // bottom front
                new Vector3(-halfWidth, -halfHeight, halfDepth),   // bottom back
                new Vector3(-halfWidth, halfHeight, halfDepth),    // top back
                new Vector3(-halfWidth, halfHeight, -halfDepth)    // top front
            },

            // square right face
            {
                new Vector3(halfWidth, -halfHeight, halfDepth),    // bottom back
                new Vector3(halfWidth, -halfHeight, -halfDepth),   // bottom front
                new Vector3(halfWidth, halfHeight, -halfDepth),    // top front
                new Vector3(halfWidth, halfHeight, halfDepth)      // top back
            }

        };

        //define triangles
        int[][] faceTriangles =
        {
            new int[] { 0, 1, 2, 0, 2, 3 }, // front
            new int[] { 0, 1, 2, 0, 2, 3 }, // back
            new int[] { 0, 1, 2, 0, 2, 3 }, // top
            new int[] { 0, 1, 2, 0, 2, 3 }, // bottom
            new int[] { 0, 1, 2, 0, 2, 3 }, // left
            new int[] { 0, 1, 2, 0, 2, 3 }  // right
        };

        //define uvs
        Vector2[] faceUVs =
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

        //pick ranmdom roll for textures
        int whichRoll = Random.Range(0,7);
        
        //for each face
        for (int i = 0; i < 6; i++)
        {
            //initialise face
            GameObject faceObject = new GameObject("Face_" + i);
            faceObject.transform.parent = transform;
            faceObject.transform.localPosition = Vector3.zero;

            //create a mesh for it
            Mesh mesh = new Mesh();
            
            //create array for face vertices
            Vector3[] faceVerts = new Vector3[4];

            //initialise for each
            for (int j = 0; j < 4; j++)
            {
                faceVerts[j] = rotation * faceVertices[i, j];
            }
            //create array for face triangles + set
            int[] faceTris = faceTriangles[i];

            //create array for uvs
            Vector2[] faceUvs = new Vector2[faceVerts.Length];
            //apply
            for (int j = 0; j < faceUvs.Length; j++)
            {
                faceUvs[j] = faceUVs[j % 4]; 
            }

            //set mesh
            mesh.vertices = faceVerts;
            mesh.triangles = faceTris;
            mesh.uv = faceUvs;
            mesh.RecalculateNormals();

            //initialise mesh filter
            MeshFilter meshFilter = faceObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            //initialise mesh renderer
            MeshRenderer meshRenderer = faceObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Standard"));

            if(i == 4 || i == 5)
            {
                //for the square faces
                //give tag so we can check when its clicked
                faceObject.tag = "Clickable";
                
                //give its own unique collider
                MeshCollider meshCollider = faceObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = mesh;
                meshCollider.convex = true; 
            }

            //set texture depending on roll
            switch (whichRoll)
            {
                case 0:
                    meshRenderer.material.mainTexture = RollOne[i];
                    break;
                case 1:
                    meshRenderer.material.mainTexture = RollTwo[i];
                    break;
                case 2:
                    meshRenderer.material.mainTexture = RollThree[i];
                    break;
                case 3:
                    meshRenderer.material.mainTexture = RollFour[i];
                    break;
                case 4:
                    meshRenderer.material.mainTexture = RollFive[i];
                    break;
                case 5:
                    meshRenderer.material.mainTexture = RollSix[i];
                    break;
                case 6:
                    meshRenderer.material.mainTexture = RollSeven[i];
                    break;
                default:
                    Debug.LogError("Missing texture on face: " + faceObject.name);
                    break;
            }
        }
    }
}