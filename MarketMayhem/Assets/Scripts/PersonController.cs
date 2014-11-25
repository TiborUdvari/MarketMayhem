using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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



	public List<PersonController> containmentList;
	// Attributes 
	public CharacterState state; // TODO setter{
	public CharacterState State
	{
		get {return state;}
		set {
			state = value;
			SpriteRenderer headSpriteRenderer = transform.FindChild("Head").GetComponent<SpriteRenderer>() as SpriteRenderer;

			switch (state)
			{
			case CharacterState.WALKING:
				headSpriteRenderer.sprite = spriteList[0];
				break;
			case CharacterState.WATCHING:
				speed *= 0.5f;
				//TODO add the head
				headSpriteRenderer.sprite = spriteList[1];

				StartCoroutine(continueWalking());

				Debug.Log("Should change head");
				break;
			case CharacterState.INTERACTING:
				// Change the head
				break;

			}

		}
	}

	public List<Sprite> spriteList = new List<Sprite>();

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

	private float attention;
	public float Attention
	{
		get {return attention;}
		set 
		{
			attention = value;
			SpriteRenderer spriteRenderer = transform.FindChild("Circle").GetComponent<SpriteRenderer>() as SpriteRenderer;
			spriteRenderer.color = new Color (1f, 1f, 1f, attention);
		}
	}

	private bool hasBeenTappedWhileWatching = false;

	// Use this for initialization
	void Start () 
	{
		Attention = 0.0f;
		State = CharacterState.WALKING;
		//SpriteRenderer.color = new Color (1f, 1f, 1f, 0.2f);
		//speed = 1.0f + ((float)Random.Range (0, 10)) / 10;
	}
	
	// Update is called once per frame
	void Update () 
	{
		rigidbody.MovePosition (new Vector3 ( rigidbody.transform.position.x + speed * direction * Time.deltaTime  , rigidbody.transform.position.y, 0.0f));
		HandleCleanup ();
	
	}

	void HandleCleanup()
	{
		float padding = (Camera.main.WorldToScreenPoint( new Vector3(GetComponentInChildren<SpriteRenderer>().bounds.size.x, 0.0f, 0.0f))).x;

		Vector3 exitPosition;
		if (direction > 0) 
		{
			exitPosition = Camera.main.WorldToScreenPoint ( new Vector3(rigidbody.transform.position.x, 0.0f, 0.0f));

			if (exitPosition.x >= Screen.width + padding) 
			{
				containmentList.Remove(this);
				Destroy (gameObject);
			}	
		} 
		else 
		{
			exitPosition = Camera.main.WorldToScreenPoint ( new Vector3(rigidbody.transform.position.x, 0.0f, 0.0f));

			if (exitPosition.x <= -padding )
			{
				containmentList.Remove(this);
				Destroy(gameObject);

			}
		}


	}

	void OnTouchDown()
	{
		Debug.Log("ontouchdown");

		switch (state) 
		{
		case CharacterState.WALKING:
			state = CharacterState.PISSED;
			break;
		case CharacterState.WATCHING:
			state = CharacterState.INTERACTING;
			break;
		case CharacterState.INTERACTING:
			//TODO
			break;
		}

		/*if (CharacterState.WALKING)
		{
			state = CharacterState.PISSED;
		}*/


	}

	IEnumerator continueWalking()
	{
		yield return new WaitForSeconds (1);
		if (!hasBeenTappedWhileWatching) 
		{
			State = CharacterState.WALKING;
		}

	}

	public void watch()
	{
		if (state == CharacterState.WALKING) 
		{
			State = CharacterState.WATCHING;
		}
	}
}
