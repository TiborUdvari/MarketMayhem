using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class SpawnLogic : MonoBehaviour {

	private Vector3 spawnPoint;
	public GameObject person;
	//public List<GameObject> personList;
	private GameObject personClone;

	private int rowNumber = 4;
	private float xPos;
	private float yPos;
	private float bottomPadding = 80;
	private float topPadding = 20;
	public List<float> ySpawnPosList = new List<float>();
	private Timer timer;


	// --- Hacks --- 
	int timerHack;
	
	void Start () 
	{
		PopulateSpawnPosition ();
		//timer = new System.Threading.Timer(obj => { SpawnPerson(); }, null, 1000, System.Threading.Timeout.Infinite);
		//StartCoroutine (SpawnPerson());
		StartCoroutine (spawn());
	}


	IEnumerator spawn()
	{
		while (true) {
			yield return new WaitForSeconds (1);
			SpawnPerson();
		}
	}

	public void SpawnPerson()
	{
		int yPosIndex = Random.Range (0, ySpawnPosList.Count);
		float yPos = ySpawnPosList[yPosIndex];

		float padding = 60.0f; // TODO get half of sprite size
		bool side = yPosIndex % 2 == 0;
		float xPos = side ? -padding : Screen.width + padding;

		Vector3 initPosition =  Camera.main.ScreenToWorldPoint( new Vector3 ( xPos, yPos, 9.0f));
		personClone = Instantiate (person, initPosition, Quaternion.identity) as GameObject;
		PersonController personController = personClone.GetComponent<PersonController>();
		personController.Direction = side ? 1.0f : -1.0f;
		personController.SpeedCategory = SpeedCategory.FAST;
	}

	void FixedUpdate () 
	{

	}
	
	// --- Person generation ---

	void PopulateSpawnPosition() 
	{
		float step = (Screen.height - bottomPadding - topPadding) / rowNumber;
		for (int i = 0; i < rowNumber; i++) {
			float yPos = bottomPadding + i * step + step/2;
			ySpawnPosList.Add(yPos);
			spawnPoint = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width * 0.5f, yPos, 10));
			Debug.Log("Position y " + yPos);

		}
	}
}
