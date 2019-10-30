using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PebbleManager : MonoBehaviour {

	public Text pebbleText;
	public GameObject[] pebble;
	public bool toggleSwitch = false;

	void Update(){
		if(Input.GetKeyDown(KeyCode.T)){
			toggleSwitch = !toggleSwitch;
			pebbleText.text = (toggleSwitch)?"PEBBLES : ON":"PEBBLES: OFF";
		}
	}

// mark visited squares
	public void PlacePebbles( List<Node> path )
	{
		int i = 0;
		foreach (Node n in path){
			i++;
// place pebbel if is at required distance
			if( i%5 == 0 && toggleSwitch){
				Instantiate( pebble[Random.Range(0,pebble.Length)] , n.worldPosition, Quaternion.identity);
			}
			n.visited = true;
		}

	}
}
