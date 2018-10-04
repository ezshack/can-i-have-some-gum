using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public int NumberOfBullets;
	public int DamagePerBullet;
	public float Spread;
	public float Range;
	public float Knockback;
	public float FireRate;
	Weapon This;
	GameObject player;
	//Collider2D trigger;
	//Collider2D playerCollider;
	// Use this for initialization
	void Start () {
		//trigger = gameObject.GetComponent<Collider2D> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		This = gameObject.GetComponent<Weapon> ();
		//playerCollider = player.GetComponent<Collider2D> ();
		//gameObject.layer = 3;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(player.transform.position,gameObject.transform.position) < 4f && Input.GetKey(KeyCode.E) )
			PickUp(player);

		Destroy (gameObject, 20f);
	}

	void PickUp(GameObject other){
		print ("Pick Up");
		//if ( other.gameObject == player ) {

			other.SendMessage( "SetWeapon" , This);

			//SendMessage ("SetWeapon", This);
			Destroy(gameObject);
		//} else {
			return;
		//}
	}

}
