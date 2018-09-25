using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	Transform player;

	bool viable = true;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ().transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector2.Distance (gameObject.transform.position, player.position) > 10) {
			viable = true;
		} else {
			viable = false;
		}
	}

	public bool CheckViability (){
		if (Vector2.Distance (gameObject.transform.position, player.position) > 10) {
			viable = true;
		} else {
			viable = false;
		}
		return viable;
	}

}
