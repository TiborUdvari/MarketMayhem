using UnityEngine;
using System.Collections;

public enum CharacterState
{
	WALKING,
	WATCHING,
	INTERACTING,
	PISSED
}

public class PersonController : MonoBehaviour {

	// Attributes 
	public CharacterState state; // TODO setter
	public float speed;
	public float direction;
	
	// Use this for initialization
	void Start () 
	{
		direction = 1.0f;
		speed = 1.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		rigidbody2D.MovePosition (new Vector3 ( rigidbody2D.transform.position.x + speed * direction * Time.deltaTime  , rigidbody2D.transform.position.y, 0.0f));
		HandleCleanup ();
	
	}

	void HandleCleanup()
	{
		float padding = 0.0f;
		Vector3 exitPosition = Camera.main.WorldToScreenPoint ( new Vector3(rigidbody2D.transform.position.x + padding, 0.0f, 0.0f));
		if (exitPosition.x >= Screen.width)
		{
			Debug.Log("DESTROOOOOOOOOOOY BLEAAAH BLOOOD ");
			Destroy(gameObject);
		}
	}
}
