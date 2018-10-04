using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public GameObject[] weapons;
	public GameObject[] items;

	public GameObject navSensor;

    public GameObject pointerPrefab;

	public int health;
	PlayerController player;
	Rigidbody2D self;
	public float speed = 1f;

	public float hitRate = 0.8f;
	public float lastHit;
	public int damage;

	ScoreManager scoreManager;

    Rect pointerLoc;

    GameObject pointer;

    SpriteRenderer pointerSprite;

    // Use this for initialization
    void Start () {

		player = FindObjectOfType<PlayerController> ();

		self = transform.GetComponent<Rigidbody2D> ();

		scoreManager = FindObjectOfType<ScoreManager> ();

        //gameObject.layer = 9;

        /*GameObject bounds = FindObjectOfType<Bounds>().gameObject;

        Debug.Log( string.Concat( "Bounds: " , bounds.layer.ToString() ));
        Debug.Log( gameObject.layer.ToString() );

        Physics2D.IgnoreLayerCollision( bounds.layer , gameObject.layer , true );*/
        GameObject newPointer = Instantiate(pointerPrefab, gameObject.transform.position, gameObject.transform.rotation);
        newPointer.SendMessage("SetTarget", gameObject);
        newPointer.SendMessage("SetPlayer", player.gameObject);
        pointer = newPointer;
        pointerSprite = pointer.GetComponent<SpriteRenderer>();

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

		if (health <= 0) {

			Die();

		}

		return;
	}
	void Die (){

        DestroyImmediate(pointer);

        GameObject newDeadEnemy = deadEnemy;

		GameObject body = Instantiate(newDeadEnemy, transform.position, transform.rotation) as GameObject;

		body.SendMessage ("ResetColors");

		scoreManager.SendMessage ("KillCount");

		player.SendMessage ("KillCount");

		if (Random.Range (1, 3) == 1) {

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

    void DifficultyLevel(int wave)
    {
        health = health + (1 * wave);
        damage = damage + (Mathf.FloorToInt(wave / 4));
        speed = speed + (wave / 7);
    }
    private void OnBecameVisible()
    {
        pointerSprite.color = Color.clear;
    }
    private void OnBecameInvisible()
    {
        if(pointerSprite != null)
        pointerSprite.color = Color.white;
    }

}
