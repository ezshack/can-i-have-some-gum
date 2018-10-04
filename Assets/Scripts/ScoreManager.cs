using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour {
	
	int kills = 0;

	int playerRecord;

	public GUIStyle Skin;

    Scene currScene;

    public Rect position;

	void Awake() {

		DontDestroyOnLoad(transform.gameObject);

	}



	// Use this for initialization
	void Start () {

		playerRecord = LoadScore ();

		Skin.font.material.mainTexture.filterMode = FilterMode.Point;

        currScene = SceneManager.GetActiveScene();

	}
	
	// Update is called once per frame
	void Update () {
    }

	void OnGUI(){

        if( currScene.buildIndex == 0 ){

            GUI.Label(new Rect((Screen.width / 2) - position.x, (Screen.height / 2) - position.y, position.width, position.height), string.Concat("Record: " + playerRecord.ToString()), Skin);

		}else{
            GUI.Label(new Rect(10, 80, 150, 20), string.Concat( "Kills: " + kills.ToString()));

		}

	}

	void KillCount (){
		kills = kills + 1;
	}

	void SaveScore (){
		if(kills > PlayerPrefs.GetInt("recordKillCount"))
		PlayerPrefs.SetInt("recordKillCount", kills );
		Destroy (gameObject);

	}
	int LoadScore (){

		int playerRecord = PlayerPrefs.GetInt ("recordKillCount");
		return playerRecord;

	}

}
