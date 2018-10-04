using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {


	GameObject player;
	public string functionCalledByItem;
	public float lerpTime;
	public float lerpDistance;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ().gameObject;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(player.transform.position,gameObject.transform.position) < lerpDistance ) {

			transform.Translate ( -(transform.position - player.transform.position).normalized * lerpTime ); //(-(transform.position - player.transform.position));

            if (Vector3.Distance(player.transform.position,gameObject.transform.position) < 0.5f ){

				PickUp(player);
			}
		}

        Destroy(gameObject, 30f);
    }

	void PickUp (GameObject other){

		other.SendMessage (functionCalledByItem , SendMessageOptions.DontRequireReceiver);

		Destroy (gameObject);

	}

}
