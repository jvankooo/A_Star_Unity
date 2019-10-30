using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	Node[,] grid;
	public List<Node> path;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeDiameter;
	int gridSizeX, gridSizeY;
	void Start()
	{
// find grid size
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		Creategrid();

	}

	void Creategrid() {
		grid = new Node[gridSizeX,gridSizeY];
// Set reference
		Vector3 worldBottomLeft = transform.position - Vector3.right*gridWorldSize.x/2 - Vector3.up*gridWorldSize.y/2;
// Check for obstacles
		for (int x = 0; x<gridSizeX; x++){
			for (int y = 0; y<gridSizeY; y++){
// position of current node
				Vector3 worldPoint = worldBottomLeft + Vector3.right*(x*nodeDiameter + nodeDiameter/2) + Vector3.up*(y*nodeDiameter+nodeDiameter/2);
// check if node is walkable
				bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeDiameter/2, unwalkableMask));
				bool visited = false;
// add node to grid
				grid[x,y] = new Node(walkable,worldPoint, x, y, visited);
			}
		}
	}
	
// dynamic update targets to avoid overlapping
	public void GridUpdate(){
		for (int x = 0; x<gridSizeX; x++){
			for (int y = 0; y<gridSizeY; y++){
				grid[x,y].walkable = !(Physics2D.OverlapCircle(grid[x,y].worldPosition, nodeDiameter/2, unwalkableMask));
			}
		}
	}

public List<Node> GetNeighbours(Node node){
	List<Node> neighbours = new List<Node>();
	for (int x = -1; x<=1; x++){
		for (int y = -1; y<=1; y++){
			if (x == 0 && y == 0)
				continue;
// coordinates(grid) of the subject
			int checkX = node.gridX + x;
			int checkY = node.gridY + y;
// add valid neighbours
			if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
				neighbours.Add(grid[checkX,checkY]);
			}
		}
	}
	return neighbours;
}
public Node NodeFromPoint(Vector3 point){
	float percentX = Mathf.Clamp01((point.x + gridWorldSize.x/2)/gridWorldSize.x);
	float percentY = Mathf.Clamp01((point.y + gridWorldSize.y/2)/gridWorldSize.y);
	int x = Mathf.RoundToInt(percentX*(gridSizeX-1)); // -1 to avoid goind out of bounds
	int y = Mathf.RoundToInt(percentY*(gridSizeY-1));
	return grid[x,y];

}

// draw the grid
	void OnDrawGizmos(){
		int i = 0;
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
		if(grid != null){
			foreach (Node n in grid){
				i++;
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				if(path != null){
					if (path.Contains(n)){
						Gizmos.color = Color.black;
					}
				}
				Gizmos.DrawCube(n.worldPosition, Vector3.one*(nodeDiameter-0.1f));
			}
		}
	}
}
