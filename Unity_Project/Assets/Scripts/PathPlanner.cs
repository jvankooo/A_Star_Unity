using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlanner : MonoBehaviour {

	Grid gridscript;
	PebbleManager pebbleScript;



	void Awake(){
		gridscript = GetComponent<Grid>();
		pebbleScript = GetComponent<PebbleManager>();
	}

	public void StartFindPath(Vector2 startPos, Vector2 targetPos){
// update grid with new features and start pathplanning
		gridscript.GridUpdate();
		StartCoroutine(FindPath(startPos, targetPos));
	}
// impliment A*
	IEnumerator FindPath(Vector2 startPos, Vector2 endPos){

		Vector2[] wayPoints = new Vector2[0];
		bool pathSuccess = false;

		Node startNode = gridscript.NodeFromPoint(startPos);
		Node targetNode = gridscript.NodeFromPoint(endPos);

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);
// loop A*
		while (openSet.Count > 0){
			Node currentNode = openSet[0];
			for (int i = 1; i<openSet.Count; i++){
				if((openSet[i].fCost < currentNode.fCost && !openSet[i].visited ) || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost<currentNode.hCost   && !openSet[i].visited)) {
					currentNode = openSet[i];
				}
			}
// update nodes
			openSet.Remove(currentNode);
			closedSet.Add(currentNode);
// if finish
			if (currentNode == targetNode) {
				pathSuccess = true;
				break;
			}
// neighbouring nodes
			foreach (Node neighbour in gridscript.GetNeighbours(currentNode)){
				if(!neighbour.walkable || closedSet.Contains(neighbour)){
					continue;
				}
// calculate and check neighbour costs
				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				if ( ( newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) && !neighbour.visited){
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = currentNode;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
		yield return null;
		if(pathSuccess){
			wayPoints = RetracePath(startNode, targetNode);
			GameObject.Find("player").GetComponent<PlayerController>().PathUpdate(wayPoints, pathSuccess);
			Debug.Log("Path Updated");
		}
	}

	Vector2[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		List<Vector2> wayPoints = new List<Vector2>();
		Node currentNode = endNode;
// update path and vector waypoints
		while (currentNode != startNode){
			path.Add(currentNode);
			pebbleScript.PlacePebbles(path);
			wayPoints.Add(currentNode.worldPosition);
			currentNode = currentNode.parent;
		}
// reverse path from start to end
		path.Reverse();
		wayPoints.Reverse();
		gridscript.path = path;
		return wayPoints.ToArray();
	}

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10*(dstX-dstY);
		return 14*dstX + 10*(dstY-dstX); 
	}

}
