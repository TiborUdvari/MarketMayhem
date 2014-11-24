using UnityEngine;
using System.Collections;

public enum Scenestate
{
	LOAD,
	START,
	GAME,
	END
}


public class SceneManager : MonoBehaviour {


	public Scenestate actualState;
	public bool isStarting = false; 

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	void Start () {
		actualState = Scenestate.LOAD;
	}

	void Update () {
		ChangeState();

	}

	private void ChangeState() 
	{
		switch (actualState) 
		{
		case Scenestate.LOAD:
			Application.LoadLevel(1);
			actualState = Scenestate.START;
			isStarting = true;
			break;
		case Scenestate.START:
			StStart();
			break;
		case Scenestate.GAME:
			StIngameLoop();
			break;
		case Scenestate.END:
			StEndscreen();
			break;
		}

	}
		
	void OnTouchDown() {
		Debug.Log ("start game");
		actualState = Scenestate.GAME;
		
	}

	public void StStart() {


	}

	public void StIngameLoop() {






		//if game over
		//actualState = Scenestate.END;
	}

	public void StEndscreen() {

		//display High-score
	//	if (Input.GetTouch) {
			Debug.Log ("start new game");
			actualState = Scenestate.GAME;
			isStarting = false;
	//	}
	}


}
