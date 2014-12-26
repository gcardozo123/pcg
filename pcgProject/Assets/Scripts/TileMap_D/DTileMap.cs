using UnityEngine;
using System.Collections.Generic;
public class DTileMap {

	protected class DRoom{
		public int left;
		public int top;
		public int width;
		public int height;
		public bool isConnected = false;

		public int right{
			get {return left + width - 1;}
		}

		public int bottom{
			get { return top + height - 1;}
		}

		public int center_x{
			get {return left + width/2;}
		}

		public int center_y{
			get {return top + height/2;}
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
		DRoom r;
		this.size_x = size_x;
		this.size_y = size_y;

		map_data = new int[size_x,size_y];

		for (int x=0; x < size_x; x++) {
			for(int y=0; y < size_y; y++){
				map_data[x,y] = TILE_BACKGROUND;
			}
		}

		rooms = new List<DRoom> ();
		int maxFails = 10;
		//for (int i=0; i < 10; i++) {
		while (rooms.Count < 10){
			int rsx = Random.Range(6,12);
			int rsy = Random.Range(6,10);

			r = new DRoom();
			r.left = Random.Range(0, size_x - rsx);
			r.top =  Random.Range (0, size_y - rsy);
			r.width = rsx;
			r.height = rsy;

			if(!RoomCollides(r)){
				rooms.Add(r);
			}
			else{
				maxFails--;
				if(maxFails <=0){
					break;
				}
			}
		}
		foreach(DRoom r2 in rooms){
			MakeRoom(r2);
		}

		for(int i = 0; i < rooms.Count; i++){
			if(!rooms[i].isConnected){
				int j = Random.Range (1, rooms.Count);
				MakeCorridor(rooms[i], rooms[(i+j) % rooms.Count]);
			}
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

	void MakeCorridor(DRoom r1, DRoom r2){
		int x = r1.center_x;
		int y = r1.center_y;

		while (x != r2.center_x) {
			map_data[x,y] = TILE_FLOOR;
			//Move x wise
			x += x < r2.center_x ? 1 : -1;
		}

		while (y != r2.center_y) {
			map_data[x,y] = TILE_FLOOR;
			//Move y wise
			y += y < r2.center_y ? 1 : -1;

		}
		r1.isConnected = true;
		r2.isConnected = true;
	}
}
