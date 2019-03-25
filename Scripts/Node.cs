using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

	public bool walkable, visited;
	public Vector2 worldPosition;
	public int gridX;
	public int gridY;
	public int gCost;
	public int hCost;
	public Node parent;

// bool visited to avoid overlapping
// bool stoned to creat more natural pebble placement
	public Node(bool _walkable, Vector2 _worlPos, int _gridX, int _gridY, bool _visited){
		walkable = _walkable;
		worldPosition = _worlPos;
		gridX = _gridX;
		gridY = _gridY;
		visited = _visited;
	}

	public int fCost{
		get {
			return gCost + hCost;
		}
	}
}
