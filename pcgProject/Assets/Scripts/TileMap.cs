using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))] //it requires a (not null) mesh filter component
[RequireComponent(typeof(MeshRenderer))] //it requires a (not null) mesh renderer component
[RequireComponent(typeof(MeshCollider))] //it requires a (not null) mesh collider component
public class TileMap : MonoBehaviour {
	public int numTilesX = 100; //100 tiles in x-axis
	public int numTilesZ = 50; //50 tiles in z-axis
	public float tileSize = 1.0f;

	// Use this for initialization
	void Start () {
		BuildMesh ();
	
	}

	public void BuildMesh(){
		int numTiles = numTilesX * numTilesZ; //number of tiles
		int numTris  = numTiles * 2; //number of triangles

		int numVertX = numTilesX + 1; //we need one more vertex in order to finish the 100 tiles
		int numVertZ = numTilesZ + 1; //we need one more vertex in order to finish the 50 tiles
		int numVerts = numVertX * numVertZ;

		//Generate the mesh data
		Vector3[] vertices	= new Vector3[numVerts];
		Vector3[] normals	= new Vector3[numVerts];
		Vector2[] uv		= new Vector2[numVerts]; //uv coordinates to apply textures

		int[] triangles		= new int[numTris * 3];

		int x, z;
		for(z=0; z < numVertZ; z++){
			for (x=0; x < numVertX; x++) {
				vertices[z * numVertX + x]   = new Vector3(x*tileSize,0,z*tileSize);
				normals[z * numVertX + x]	 = Vector3.up;
				uv[z * numVertX + x]		 = new Vector2((float) x / numVertX, (float) z / numVertZ);
			}
		}

		for (z=0; z < numTilesZ; z++) {
			for (x=0; x < numTilesX; x++) {
				int squareIndex = z * numTilesX + x;
				int triOffset	= squareIndex * 6;

				triangles[triOffset + 0] = z * numVertX + x + 			 0;
				triangles[triOffset + 1] = z * numVertX + x + numVertX + 0;
				triangles[triOffset + 2] = z * numVertX + x + numVertX + 1; 

				triangles[triOffset + 3] = z * numVertX + x + 			 0;
				triangles[triOffset + 4] = z * numVertX + x + numVertX + 1;
				triangles[triOffset + 5] = z * numVertX + x + 			 1;
			}
		}


		//Create a new mesh and populate with data
		Mesh mesh 		= new Mesh();
		mesh.vertices 	= vertices;
		mesh.triangles 	= triangles;
		mesh.normals 	= normals;
		mesh.uv 		= uv;


		//Applying the mesh to the mesh filter/renderer/collider
		MeshFilter 		mf = GetComponent<MeshFilter>();
		MeshRenderer 	mr = GetComponent<MeshRenderer>();
		MeshCollider 	mc = GetComponent<MeshCollider>();

		mf.mesh = mesh;
	}
}
