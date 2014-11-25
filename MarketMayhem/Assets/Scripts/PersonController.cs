using UnityEngine;
using System.Collections;

public enum CharacterState
{
	WALKING,
	WATCHING,
	INTERACTING,
	PISSED
}

public enum SpeedCategory
{
	SLOW = 5,
	NORMAL = 10,
	FAST = 20
}

public class PersonController : MonoBehaviour {

	// Attributes 
	public CharacterState state; // TODO setter
	public float speed;
	public float direction;

	public float Direction
	{
		get { return direction; }
		set
		{
			direction = value;
			// Do usefull stuff 
			transform.localScale =  new Vector3(value * transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
	}

	private SpeedCategory speedCategory;
	public SpeedCategory SpeedCategory
	{
		get { return speedCategory; }
		set {
			speedCategory = value;
			speed = 1.0f + (float)speedCategory / 10.0f;

			switch (speedCategory)
			{
			case SpeedCategory.SLOW:

				break;
			case SpeedCategory.NORMAL:

				break;
			case SpeedCategory.FAST:

				break;
			}

		}
	}

	// Use this for initialization
	void Start () 
	{
		//speed = 1.0f + ((float)Random.Range (0, 10)) / 10;
	}
	
	// Update is called once per frame
	void Update () 
	{
		rigidbody2D.MovePosition (new Vector3 ( rigidbody2D.transform.position.x + speed * direction * Time.deltaTime  , rigidbody2D.transform.position.y, 0.0f));
		HandleCleanup ();
	
	}

	void HandleCleanup()
	{
		float padding = 21.0f;
		Vector3 exitPosition;

		if (direction > 0) 
		{
			exitPosition = Camera.main.WorldToScreenPoint ( new Vector3(rigidbody2D.transform.position.x, 0.0f, 0.0f));

			if (exitPosition.x >= Screen.width + padding) 
			{
				Destroy (gameObject);
			}	
		} 
		else 
		{
			exitPosition = Camera.main.WorldToScreenPoint ( new Vector3(rigidbody2D.transform.position.x, 0.0f, 0.0f));

			if (exitPosition.x <= -padding )
			{
				Destroy(gameObject);

			}
		}


	}
}
