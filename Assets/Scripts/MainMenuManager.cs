using UnityEngine;
using System.Collections;


public class MainMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

	}

	void PlayGame (){
		Application.LoadLevel ("Player Test Scene");
	}

	void QuitGame (){
		Application.Quit ();

	}

}
