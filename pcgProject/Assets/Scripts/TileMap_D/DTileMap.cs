using UnityEngine;
using System.Collections.Generic;
public class DTileMap {

	protected class DRoom{
		public int left;
		public int top;
		public int width;
		public int height;

		public int right{
			get {return left + width - 1;}
		}

		public int bottom{
			get { return top + height - 1;}
		}
		public bool CollidesWith(DRoom other){
			if (left > other.right) {
				return false;
			}
			if (top > other.bottom) {
				return false;
			}
			if (right < other.left) {
				return false;
			}
			if (bottom < other.top) {
				return false;
			}
			return true;
		}
	}

	int size_x;
	int size_y;

	public const int TILE_FLOOR 		= 0;
	public const int TILE_WALL 			= 1;
	public const int TILE_BACKGROUND 	= 2;
	public const int TILE_BLACK 		= 3;

	public int type = TILE_FLOOR; //type of tile

	int[,] map_data;
	List<DRoom> rooms;

	public DTileMap(int size_x, int size_y){
		this.size_x = size_x;
		this.size_y = size_y;

		map_data = new int[size_x,size_y];

		for (int x=0; x < size_x; x++) {
			for(int y=0; y < size_y; y++){
				map_data[x,y] = TILE_BACKGROUND;
			}
		}

		rooms = new List<DRoom> ();

		for (int i=0; i < 10; i++) {
			int rsx = Random.Range(4,8);
			int rsy = Random.Range(4,8);

			DRoom r = new DRoom();
			r.left = Random.Range(0, size_x - rsx);
			r.top =  Random.Range (0, size_y - rsy);
			r.width = rsx;
			r.height = rsy;

			if(!RoomCollides(r)){
				rooms.Add(r);
			}
		}
		foreach(DRoom r2 in rooms){
			MakeRoom(r2);
		}

	}

	bool RoomCollides(DRoom r){
		foreach (DRoom r2 in rooms) {
			if(r.CollidesWith(r2)){
				return true;
			}
		}
		return false;
	}

	public int GetTileAt(int x, int y){
		return map_data[x,y];
	}

	void MakeRoom(DRoom r){
		for (int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				if(x==0 || x == r.width - 1 || y==0 || y==r.height-1){
					map_data[r.left+x,r.top+y] = TILE_WALL;
				}
				else{
					map_data[r.left+x,r.top+y] = TILE_FLOOR;
				}
			}
		}
	}
}
