using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public GameObject instructionsPanel;


    // Use this for initialization
	void Start () {
        instructionsPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

	}

	void PlayGame (){
        SceneManager.LoadScene(1);
	}

	void QuitGame (){
		Application.Quit ();

	}

    void Instruct (){
        instructionsPanel.SetActive(!instructionsPanel.activeSelf);
    }


}
