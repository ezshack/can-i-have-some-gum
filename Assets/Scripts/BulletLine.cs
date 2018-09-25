using UnityEngine;
using System.Collections;

public class BulletLine : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
	}

	void OnEnabled(){	
	}

	// Update is called once per frame
	void Update () {
		Destroy ( gameObject , 0.03f );
	}
}
