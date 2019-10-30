using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public void Mainmenu(){
		SceneManager.LoadScene(0);
	}

	public void RapidProt(){
		SceneManager.LoadScene(1);
	}

	public void Final(){
		SceneManager.LoadScene(2);
	}

	public void Endgame(){
		Application.Quit();
	}

}
