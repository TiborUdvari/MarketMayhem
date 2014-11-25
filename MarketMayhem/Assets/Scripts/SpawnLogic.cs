using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class SpawnLogic : MonoBehaviour {

	private Vector3 spawnPoint;
	public GameObject person;
	private GameObject personClone;

	private int rowNumber = 4;
	private float xPos;
	private float yPos;
	private float bottomPadding = 80;
	private float topPadding = 20;
	public List<float> ySpawnPosList = new List<float>();
	private Timer timer;


	private List<PersonController> listPersons = new List<PersonController>();

	void Start () 
	{
		PopulateSpawnPosition ();
		StartCoroutine (spawn());
		StartCoroutine (watchingCoroutine ());
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

		personController.containmentList = listPersons;
		listPersons.Add (personController);
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


		}
	}


	// --- Watching logic --- 


	IEnumerator watchingCoroutine()
	{
		while (true) {
			yield return new WaitForSeconds (1.5f);
			handleWatching();
		}
	}

	void handleWatching()
	{
		int watchingCount = 0;
		for (int i=0; i<listPersons.Count; i++) 
		{
			PersonController pC = listPersons[i];
			if (pC.state == CharacterState.WATCHING) watchingCount++; 
		}

		if (watchingCount < 4) 
		{
			int randomPersonIndex = Random.Range(0, listPersons.Count);
			PersonController pC = listPersons[randomPersonIndex];
			pC.watch();
			Debug.Log("Watching");
		}
	}


}
