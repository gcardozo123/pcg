﻿public class TDMap {
	DTileMap[] _tiles;
	int _width;
	int _height;

	public TDMap(int width, int height){
		_width 	= 	width;
		_height =	height;

		_tiles = new DTileMap[_width * _height];
	}

	public DTileMap GetTile(int x, int y){
		if (x < 0 || x >= _width || y >= _height) {
			return null;
		}

		return _tiles [y * _width + x];
	}
}
