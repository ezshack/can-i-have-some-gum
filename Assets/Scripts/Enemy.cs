using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public GameObject[] weapons;
	public GameObject[] items;

	public GameObject navSensor;
	//public GameObject navSensorRight;
	//public GameObject navSensorLeft;



	public int health;
	PlayerController player;
	Rigidbody2D self;
	public float speed = 1f;

	public float hitRate = 0.8f;
	public float lastHit;
	public int damage;

	ScoreManager scoreManager;

	// Use this for initialization
	void Start () {

		player = FindObjectOfType<PlayerController> ();

		self = transform.GetComponent<Rigidbody2D> ();

		scoreManager = FindObjectOfType<ScoreManager> ();

		gameObject.layer = 1;

	}

	public GameObject deadEnemy;

	// Update is called once per frame
	void FixedUpdate () {

		Vector2 position = new Vector2 ( transform.position.x , transform.position.y );

		Vector2 target = player.gameObject.transform.position;

		Vector2 Dir = -(position - target).normalized;

		transform.up = Dir;

		self.AddForce (-(position - target).normalized * speed);

	}

	public void Damage (int damage){

		health = health - damage;

		//print (Health);
		if (health <= 0) {

			Die();

		}

		return;
	}
	void Die (){

		GameObject newDeadEnemy = deadEnemy;

		GameObject body = Instantiate(newDeadEnemy, transform.position, transform.rotation) as GameObject;

		body.SendMessage ("ResetColors");

		scoreManager.SendMessage ("KillCount");

		player.SendMessage ("KillCount");

		if (Random.Range (1, 4) == 1) {

			int rand = Random.Range(0,2);

			if( rand > 0 ){

				Instantiate (items [Random.Range (0, items.Length)], transform.position , Quaternion.Euler(Vector3.zero));

			} else{

				Instantiate (weapons [Random.Range (0, weapons.Length)] , transform.position, transform.rotation);

			}
		}

		Destroy (gameObject);
	}

	void OnCollisionStay2D (Collision2D other){
		if (Time.time - lastHit > 1 / hitRate)
		{
			lastHit = Time.time;

			other.collider.SendMessage ("TakeDamage", damage , SendMessageOptions.DontRequireReceiver);
		}
	}
	
}
