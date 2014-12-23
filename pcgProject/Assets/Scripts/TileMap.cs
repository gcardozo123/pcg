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

	public Texture2D terrainTiles;
	public int tileResolution = 16;
	
	// Use this for initialization
	void Start () {
		BuildMesh ();
	}

	void BuildTexture(){
		int numTilesPerRow = terrainTiles.width / tileResolution;
		int numRows = terrainTiles.height / tileResolution;

		int texWidth = numTilesX * tileResolution;
		int texHeight = numTilesZ * tileResolution;
		Texture2D texture = new Texture2D (texWidth,texHeight);

		for (int z=0; z < numTilesZ; z++) {
			for(int x=0; x < numTilesX; x++){
				int tileOffset = Random.Range (0, 3) * tileResolution; 
				Color[] c = terrainTiles.GetPixels (tileOffset,0, tileResolution, tileResolution);
				texture.SetPixels(x*tileResolution,z*tileResolution, tileResolution, tileResolution, c);
			}
		}
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply ();
		MeshRenderer mr = GetComponent<MeshRenderer> ();
		mr.sharedMaterials[0].mainTexture = texture;
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
				uv[z * numVertX + x]		 = new Vector2((float) x / numTilesX, (float) z / numTilesZ);
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


		//Applying the mesh to the mesh filter/collider
		MeshFilter 		mf = GetComponent<MeshFilter>();
		MeshCollider 	mc = GetComponent<MeshCollider>();

		mf.mesh = mesh;
		mc.sharedMesh = mesh;

		BuildTexture ();
	}
}
