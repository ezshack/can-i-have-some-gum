using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	Rigidbody2D Player;                   // This rigidbody...
	public int health = 50;               // Player health, pretty self explanitory...
	public GameObject gun;                // This is simply going to be a reference point for the raycast to use as an origin...
	Camera main;                          // Main camera...
	public GameObject bulletLinePrefab;   // The prefab to spawn as the bullet line effect...
	public SpriteRenderer pauseButton;    // The sprite that looks like a pause button...

	public float speed;                   // Player travel speed..

	public int currNumberOfBullets = 1;   // Number of bullets I want the gun to spawn for each shot...
	public int currDamagePerBullet = 3;   // Amount of damage I want each bullet to do when it hits something that can take damage...
	public float currSpread = 0.1f;       // The amount of random spread I want to apply to each bullet...
	public float currRange = 20f;         // The distance I want the bullet to travel...
	public float currKnockback = 1f;      //The amount of force I want to apply per bullet when it hits an enemy or rigidbody...
	public float fireRate = 0.8f;         //The number of bullets I want the gun to shoot per second...
	public float lastFired;               // The value of Time.time at the last firing moment...

	int kills = 0;                        // Current score for the current game...

	//int healthMod = 0;                  // Will use this with the health mod later...
	int damageMod = 0;                    // Damage modifier or number of damage powerups the player has picked up...
	float rangeMod = 0.0f;                // Will use this with the range modifier later...
	float knockbackMod = 0.0f;            // Knockback modifier, or the number of knockback powerups the player has picked up...
	float fireRateMod = 0.0f;             // Firerate modifier, or the number of knockback powerups the player has picked up...

	public GUIStyle skin;                 // A skin to use for the gui contained in this script...

	ScoreManager scoreManager;            // The Scoremanager object that the player sends score to at the end of the current game...

    GameObject bounds;

    int layerMask;

    public Texture pointer;
    // Use this for initialization...
    void Start () {

		// Find the score manager. The score manager doesn't start in the same scene as the player so we can't set this value in the editor. We have to find the value when the player spawns.
		scoreManager = FindObjectOfType<ScoreManager> ();

		// Find the main camera. It's pretty self explanitory.
		main = Camera.main;

		// Find the Rigidbody2D component attached to this object( the Player )...
		Player = gameObject.GetComponent<Rigidbody2D> ();

		pauseButton.enabled = false;

        bounds = FindObjectOfType<Bounds>().gameObject;

        bounds.layer = 10;

        layerMask = 1 << bounds.layer;
	}

	// Late Update happens after regular update, making it useful for moving objects that follow other moving objects...
	void LateUpdate (){


		//Checks if the camera is at least 7.5 units away
		if (Vector2.Distance (transform.position, main.transform.position) > 7.5f) {

			// linearly interpolates the camera's position to within 7.5 units of the player.
			main.transform.position = Vector3.Lerp(main.transform.position, new Vector3 (transform.position.x , transform.position.y , -5.0f), (Vector2.Distance (transform.position, main.transform.position) - 7.5f) / 8f );
		}
		/*if (Vector2.Distance(gameObject.transform.position,Vector2.zero) > mapRange){
			gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, Vector2.zero, ( Vector2.Distance ( transform.position , Vector2.zero ) - mapRange ) / 8f );
		}*/

	}

	void Update (){
	}

	// Update is called once per frame
	void FixedUpdate () {

		// Move the player according to the input axis
        Player.velocity =( new Vector2( Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).normalized * speed);

		// Calls the faceMouse function defined later. Does exactly what you would expect.
		faceMouse ();

		// Checks if either the mouse button or space key are BEING pressed.
		if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) {

			// Checks to make sure that it's been at least ( 1 / firerate ) seconds since the player last fired their weapon. Also incorperates the fireRateMod.
			if (Time.time - lastFired > 1 / (fireRate + (fireRateMod / 3))){

				// Resets the lastFired variable. Basically says, "Nope, time to wait before shooting again."
				lastFired = Time.time;

				// Calls the makeBullets function.
				makeBullets( currNumberOfBullets , currDamagePerBullet , gun.transform.position , gun.transform.up , currRange , currSpread , currKnockback);
			}
		}
	}

	// The faceMouse function called a little earlier.
	void faceMouse(){

		// Gets the mouse position on the screen.
		Vector3 mousePos = Input.mousePosition;

		// Converts the mouse position to a 3D world point, which will allow us to reference it as a relevant position, and not just a point on the screen.
		mousePos = Camera.main.ScreenToWorldPoint (mousePos);

		// Creates a new direction for the player to point in the direction of the mouse's world point.
		Vector2 Dir = new Vector2 (mousePos.x - transform.position.x, mousePos.y - transform.position.y);

		// Changes the orientation of the player to point at the mouse's world point. 
		// I should probably linearly interpolate this, I've noticed some jittery movement with the player's orientation.
		// The alternative is to call the function earlier or later to see if it's a timing issue.
		transform.up = Dir;
	}

	// The [makeBullets()] function was called earlier. It calls for an amount of bullets, an amount of damage that each bullet will deal, an origin point, 
	// another point to shoot towards, a float for a range, a float for the max spread, and a float for knockback force.
	public void makeBullets ( int Quantity , int Damage, Vector2 Origin, Vector2 Direction, float Range, float Spread = 0f, float KnockBack = 0.0f){

		// This entire for loop repeats for each bullet being created.
		for( int i = Quantity ; i > 0 ; i-- ){

			// Gets the point where the ray will end. Uses normalized [Direction], adds a random value to it multiplied by the [Spread], 
			// which means if spread is zero, the bullets will fly perfectly straight.
			Vector2 rayEnd = new Vector2( (Direction.normalized.x + (Random.value - 0.5f) * Spread) * (Range + rangeMod), (Direction.normalized.y + (Random.value - 0.5f) * Spread) * (Range + rangeMod));

			// Creates and defines the bullet variable as a ray shooting from [Origin] towards [rayEnd] but only as far as [Range].
			RaycastHit2D bullet;
            bullet = Physics2D.Raycast ( Origin , rayEnd, Range + rangeMod, ~layerMask );

			// Creates a [lineRend] variable and defines it as a newly created line renderer.
			LineRenderer lineRend = ((GameObject)Instantiate(bulletLinePrefab)).GetComponent<LineRenderer>();

			// Sets the first position of the line renderer to [Origin].
			lineRend.SetPosition( 0 , gun.transform.position);

			// Checks if the raycast hit something AND the thing it hit is within range.
			if ( bullet.collider != null && bullet.distance <= Range + rangeMod){

				// Checks if the the thing the ray hit has a rigidbody.
				if (bullet.collider.attachedRigidbody != null){

					// Adds force to the rigidbody equal to [KnockBack].
					bullet.collider.attachedRigidbody.AddForce(new Vector2 ( bullet.collider.transform.position.x - bullet.point.x , bullet.collider.transform.position.y - bullet.point.y ) * (KnockBack + knockbackMod), ForceMode2D.Impulse);

					// If the thing it hit has the "Enemy" tag...
					if (bullet.collider.tag == "Enemy"){

						// Tell the enemy it hit to take damage equal to [Damage].
						bullet.collider.SendMessage("Damage", (Damage + damageMod));
					}
				}

				// Set the second position of the line renderer (the end of the line renderer) to the point where the ray hit a collider.
				lineRend.SetPosition( 1 , bullet.point );
			
			} else if (bullet.collider == null){ // If there wasn't a collider...

				// Set the second position on the line renderer (the end of the line renderer) to the end of the ray, [rayEnd].
				lineRend.SetPosition( 1 , rayEnd + new Vector2 ( transform.position.x , transform.position.y ) );
			}		
		}
	}

	// This function is called when the player picks up a new weapon.
	public void SetWeapon (Weapon newWeapon){

		// Each of these lines simply sets the local variable in THIS script to the relevant value contained by the weapon.

		currNumberOfBullets = newWeapon.NumberOfBullets;

		currDamagePerBullet = newWeapon.DamagePerBullet;

		currRange = newWeapon.Range;

		currSpread = newWeapon.Spread;

		currKnockback = newWeapon.Knockback;

		fireRate = newWeapon.FireRate;
	}

	// This function is responsible for taking damage. It takes an integer as the amount of damage.
	void TakeDamage(int amount){

		// Subtract [amount] from your total [health]
		health = health - amount;

		// If [health] is less than or equal to 0 then call the [EndGame()] function.
		if (health <= 0){

			EndGame();
		}
	}

	// A simple function to call when a pause button is pressed.
	public void TogglePause() {

		// Toggles the time scale between 0 and 1, effectively pausing and unpausing the game.
		Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;

		pauseButton.enabled = !pauseButton.enabled;
	}

	// This function is called either when the player has 0 or less life, or when the "Quit" button is pressed.
	public void EndGame(){

		// Sends a message telling the score manager to save the current score.
		scoreManager.SendMessage("SaveScore");

        // Loads the main menu scene.
        SceneManager.LoadScene(0);

		// Resets the time scale to 1, just in case the player quit while paused.
		Time.timeScale = 1.0f;
	}

	// This function is called when the player picks up a Heart object.
	void PickUpMedpack (){

		// Add 5 to [health].
		health = health + 5;

		// This little bit just limits the life total to 50 and no higher.
		if (health > 50)
			health = 50;
	}

	// This function is called when the player picks up a FireRateMod object.
	void PickUpFireRateModifier (){

		// Add 1 to [fireRateMod].
		fireRateMod = fireRateMod + 1;
	}

	// This function is called when the player picks up a DamageMod object.
	void PickUpDamageModifier (){

		// Add 1 to [damageMod].
		damageMod = damageMod + 1;
	}

	// This function is called when the player picks up a KnockbackMod object.
	void PickUpKnockbackModifier (){

		// Add 1 to [knockbackMod].
		knockbackMod = knockbackMod + 1f;
	}

	// This function is called when the player picks up a RangeMod object.
	void PickUpRangeModifier (){

		// Add 1 to [rangeMod]
		rangeMod = rangeMod + 1f;
	}


	void OnGUI(){

		// Displays [health].
		GUI.Label(new Rect(10, 40, 150, 20), string.Concat( "Health: " + health.ToString()), skin);

		// Displays the total damage being done by each bullet.
		GUI.Label(new Rect(10, 60, 150, 20), string.Concat( "Damage/Bullet: " + (currDamagePerBullet + damageMod).ToString()), skin);

		// Displays the total knockback force being applied with each bullet hit.
		GUI.Label(new Rect(10, 80, 150, 20), string.Concat( "Knockback: " + (currKnockback + knockbackMod).ToString()), skin);

		// Displays the total number of bullets being shot per second.
		GUI.Label(new Rect(10, 100, 150, 20), string.Concat( "Bullets/Second: " + (fireRate + (fireRateMod / 3)).ToString()), skin);

		// Displays the total range.
		GUI.Label(new Rect(10, 120, 150, 20), string.Concat( "Range: " + (currRange + rangeMod).ToString()), skin);

		// Displays the total number of enemies the player has killed this game.
		GUI.Label(new Rect(10, 140, 150, 20), string.Concat( "Kills: " + kills.ToString()), skin);
	}

	// This function is called when an enemy dies.
	void KillCount (){

		// Add 1 to [kills]
		kills = kills + 1;
	}
}
