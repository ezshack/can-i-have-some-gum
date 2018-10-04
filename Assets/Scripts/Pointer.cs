using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {

    GameObject playerObject;
    GameObject pointerTarget;
    Camera cam;
	// Use this for initialization
	void Start () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = cam.transform.position + Vector3.forward;
        PointTowards(pointerTarget);

	}

    void PointTowards(GameObject target){
        Vector2 Dir = new Vector2(target.transform.position.x - playerObject.transform.position.x, target.transform.position.y - playerObject.transform.position.y);
        transform.up = Dir;
    }

    void SetTarget(GameObject target){
        pointerTarget = target;
    }

    void SetPlayer(GameObject player){
        playerObject = player;
    }

}
