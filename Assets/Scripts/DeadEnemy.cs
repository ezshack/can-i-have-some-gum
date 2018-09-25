using UnityEngine;
using System.Collections;

public class DeadEnemy : MonoBehaviour {
	SpriteRenderer self;
	public Color StartColor;
	public Color EndColor;
	Color StartC;
	Color End;
	float time = 0f;
	// Use this for initialization

	void Start () {
		self = gameObject.GetComponent<SpriteRenderer> ();
		self.color = StartColor;
		StartC = StartColor;
		End = EndColor;}

	// Update is called once per frame
	void Update (){
		time = time + Time.deltaTime;
	}
	
	void FixedUpdate () {
		self.color = Color.Lerp (StartC, End, time * 3f);
		if(self.color == End){
			Destroy(gameObject);
		}
	}

	void ResetColors(){
		StartC = StartColor;
		End = EndColor;
	}

}
