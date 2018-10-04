using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {

	public GameObject Enemy; // The enemy prefab to spawn for each wave.
	public float waveMultiplyer; // The number used to determine wave size.
	public int startingWave; // The first wave should be about this big.

	public GUIStyle skin;

	int wave = 0;

    Spawn [] spawns;

	// Start is used for initialization
	void Start (){
        //Physics2D.IgnoreLayerCollision (9, 10, false);
        spawns = FindObjectsOfType<Spawn>();
	}

	// Update is called once per frame
	void Update () {

		int CurrWave = Mathf.FloorToInt (startingWave * Mathf.Pow( waveMultiplyer, wave));
		
		if (GameObject.FindObjectOfType<Enemy>() == null) {
			
			SpawnWave(CurrWave);
		}
	}

	void OnGUI()
	{
		GUI.Label(new Rect(10, 5, 100, 20), string.Concat( "WAVE: " + wave.ToString()), skin);
	}

	void SpawnWave(int size){

		for ( int i = size; i > 0; i--){
            int spawn = Random.Range(0, spawns.Length);

            Vector2 pos = new Vector2( spawns[spawn].transform.position.x + (Random.value - 0.5f) , spawns[spawn].transform.position.y + (Random.value - 0.5f));

            GameObject newEnemy = Instantiate(Enemy, pos , transform.rotation).gameObject;
            newEnemy.layer = 9;
            Physics2D.IgnoreLayerCollision( 9 , 10 , true );
            newEnemy.SendMessage("DifficultyLevel",wave);
		}
		wave++;

	}
}
