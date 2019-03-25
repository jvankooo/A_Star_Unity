using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PlayerController : MonoBehaviour {

	Rigidbody2D player;
	bool updateTarget = true;
	public GameObject[] target;
	public int speed = 2;
	int targetIndex = 0, i = 1;
	int[] story;
	Transform currentTarget;
	string storyData;

	void Start(){
// read the data from the file and store it
		StreamReader reader = new StreamReader("Storyline.txt");
		storyData = reader.ReadToEnd();
		string[] data = storyData.Split(',');
		story = new int[data.Length];
		for(var a = 0; a < data.Length; a++){
			story[a] = int.Parse(data[a])-1;
		}
		transform.position = target[story[0]].transform.position;
		target[story[0]].layer = 0;
	}

	void Update(){
// update target with spacebar
		if ( Input.GetKeyDown("space") && updateTarget && i < story.Length){
			currentTarget = target[story[i]].transform;
// change layer to avoid crossing over other nodes
			target[story[i]].layer = 0;
// revert back previos node layer
			if(i>=2){
				target[story[i-2]].layer = 8;
			}
// find and call the path planner
			GameObject.Find("Planner").GetComponent<PathPlanner>().StartFindPath(transform.position, currentTarget.position);
			updateTarget = false;
			i++;
		}
	}
// update new path
	public void PathUpdate(Vector2[] newPath, bool success){
		if(success){
			StartCoroutine(FollowPath(newPath));
		}
		
	}
// follow path
	IEnumerator FollowPath(Vector2[] path){
		Vector2 currentWayPoint = path[0];
		while(true){
			if(new Vector2(transform.position.x, transform.position.y) == currentWayPoint){
				targetIndex++;
				if ( targetIndex >= path.Length ){
					updateTarget = true;
					targetIndex = 0;
					Debug.Log("Destination Reahced");
					yield break;
				}
				currentWayPoint = path[targetIndex];
			}

			transform.position = Vector2.MoveTowards(transform.position, currentWayPoint, speed*Time.deltaTime);
			yield return null;
		}
	}
}
