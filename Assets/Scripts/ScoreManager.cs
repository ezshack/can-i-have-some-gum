using UnityEngine;
using System.Collections;
using System.IO;

public class ScoreManager : MonoBehaviour {
	
	int kills = 0;

	int playerRecord;

	public GUIStyle Skin;

	void Awake() {

		DontDestroyOnLoad(transform.gameObject);

	}



	// Use this for initialization
	void Start () {

		playerRecord = LoadScore ();

		Skin.font.material.mainTexture.filterMode = FilterMode.Point;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

		if( Application.loadedLevelName == "Player Test Scene" ){

			GUI.Label(new Rect(10, 80, 150, 20), /*string.Concat( "Kills: " + */kills.ToString());	

		}else{

			GUI.Label(new Rect( (Screen.width/2) - 75 , (Screen.height/2) - 10 , 150, 20), string.Concat( "Record: " + playerRecord.ToString()), Skin);

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
