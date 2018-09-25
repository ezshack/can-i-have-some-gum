using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {
	public GameObject Enemy;
	public float waveMultiplyer;
	public int startingWave;

	public GUIStyle skin;

	//public List<GameObject> viableSpawns = null;

	//public Spawn[] spawns;



	int wave = 0;
	// Use this for initialization
	void Start () {

		//spawns = FindObjectsOfType<Spawn> ();



	}
	
	// Update is called once per frame
	void Update () {

		//int currI = 0;
		
		/*for ( int i = 0; i < spawns.Length - 1 ; i++ ){
			
			if(spawns[i].CheckViability() == true){

				
				viableSpawns.Add(spawns[i].gameObject);//SetValue(spawns[i].gameObject, currI);
				
				currI++;
				
			}
		}	*/

		int CurrWave = Mathf.FloorToInt (startingWave * Mathf.Pow( waveMultiplyer, wave + 1));
		
		if (GameObject.FindObjectOfType<Enemy>() == null) {
			
			SpawnWave(CurrWave);
			
		}

	}

	void OnGUI()
	{
		GUI.Label(new Rect(10, 20, 100, 20), string.Concat( "WAVE: " + wave.ToString()), skin);
	}

	void SpawnWave(int size){

		for ( int i = size; i > 0; i--){
			//GameObject currSpawn = viableSpawns.
			//Instantiate(Enemy, , gameObject.transform.rotation);
			Instantiate(Enemy, new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * 7.5f * (wave + 1), transform.rotation);
		}
		wave++;

	}
}
