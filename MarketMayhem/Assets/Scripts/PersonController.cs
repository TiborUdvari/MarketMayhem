using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/********************************************
 * 					Enums					*
 *******************************************/

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

/********************************************
 * 				Attributes					*
 *******************************************/
public List<PersonController> containmentList;
	public List<Sprite> spriteList = new List<Sprite>();
	public float speed;
	private bool hasBeenTappedWhileWatching = false;

	// Has getter / setter
	public float direction;
	public CharacterState state;
	private SpeedCategory speedCategory;
	private float attention;
	private float timeSinceInteracting;
	private float timeAllowedToInteract = 3.0f;

	private float beforeSpeed;

/********************************************
 * 				Getters/Setters 			*
 *******************************************/

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
				Attention = 0.0f;
				hasBeenTappedWhileWatching = false;
				SpeedCategory = speedCategory;

				break;
			case CharacterState.WATCHING:
				speed *= 0.5f;
				headSpriteRenderer.sprite = spriteList[1];

				StartCoroutine(continueWalking());

				Debug.Log("Should change head");
				break;
			case CharacterState.INTERACTING:
				// Change the head
				Attention = 0.5f;
				timeSinceInteracting = 0.0f;
				speed = 0.0f;

				break;
			case CharacterState.PISSED:
				Debug.Log("Pissed 2");
				speed *= 5.0f;
				break;
			}
		}
	}
	
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

	public float Attention
	{
		get {return attention;}
		set 
		{
			attention = value;
			GameObject circle = transform.FindChild("Circle").gameObject;
			circle.transform.localScale = new Vector3(  Mathf.Clamp((1-attention) * 2, 0.0f, 1.0f), Mathf.Clamp((1-attention) * 2, 0.0f, 1.0f), 1 );

			SpriteRenderer spriteRendererCircle = circle.GetComponent<SpriteRenderer>() as SpriteRenderer;

			spriteRendererCircle.color = new Color (1f, 1f, 1f, attention);

			SpriteRenderer headSpriteRendered = transform.FindChild("Head").GetComponent<SpriteRenderer>() as SpriteRenderer;

			float green = 1.0f;
			float blue = 1.0f;

			if (attention >= 0.6)
			{
				green -= attention;
				blue -= attention;
			}
			headSpriteRendered.color = new Color(1f, green * 2.5f, blue * 2.5f, 1f);
		}
	}
		
/********************************************
 * 				Start/Update				*
 *******************************************/

	void Start () 
	{
		Attention = 0.0f;
		State = CharacterState.WALKING;
	}
	
	void Update () 
	{
		rigidbody.MovePosition (new Vector3 ( rigidbody.transform.position.x + speed * direction * Time.deltaTime  , rigidbody.transform.position.y, 0.0f));
		HandleCleanup ();

		if (state == CharacterState.INTERACTING) 
		{
			InteractionUpdate();
		}
	}

	void InteractionUpdate()
	{
		timeSinceInteracting += Time.deltaTime;

		if (timeSinceInteracting > timeAllowedToInteract) 
		{
			interactionEndSuccess();
		}

		Attention -= (float)speedCategory / 75 * Time.deltaTime;
		if (Attention <= 0.01f || Attention >= 0.99f) 
		{
			interactionEndFailure();	
		}

		Debug.Log (timeSinceInteracting);
	}

	void interactionEndSuccess()
	{
		Debug.Log ("Success");
		State = CharacterState.WALKING;
	}

	void interactionEndFailure()
	{
		Debug.Log("Failure");
		State = CharacterState.WALKING;

	}
/********************************************
 * 				Touch handling				*
 *******************************************/

	void OnTouchDown()
	{
		Debug.Log("ontouchdown");
		switch (state) 
		{
		case CharacterState.WALKING:
			State = CharacterState.PISSED;
			break;
		case CharacterState.WATCHING:
			hasBeenTappedWhileWatching = true;
			State = CharacterState.INTERACTING;
			break;
		case CharacterState.INTERACTING:
			Attention += (float)speedCategory / 200;
			break;
		}
	}

/********************************************
 * 				Coroutines					*
 *******************************************/

	IEnumerator continueWalking()
	{
		yield return new WaitForSeconds (1);
		if (!hasBeenTappedWhileWatching) 
		{
			State = CharacterState.WALKING;
		}

	}

/********************************************
 * 				Public methods				*
 *******************************************/

	public void watch()
	{
		if (state == CharacterState.WALKING) 
		{
			State = CharacterState.WATCHING;
		}
	}

/********************************************
 * 				Cleanup						*
 *******************************************/

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
}
