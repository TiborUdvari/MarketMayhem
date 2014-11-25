using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class SpawnLogic : MonoBehaviour {

	private Vector3 spawnPoint;
	public GameObject person;
	private GameObject personClone;

	private int rowNumber = 6;
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

		personController.scoreController = GameObject.Find ("SpawnLogic").GetComponent<ScoreController>();


		personController.SpeedCategory = getRandomSpeedCategory();


		configurePersonSprites (personController, personController.SpeedCategory);

		personController.containmentList = listPersons;
		listPersons.Add (personController);
	}


	private void configurePersonSprites(PersonController personController, SpeedCategory speedCategory)
	{
		string gender = Random.Range(0,10) % 2 == 0 ? "M" : "W";

		int bodyIndex = gender == "M" ? Random.Range (0, 3) : Random.Range (0, 2);
		int headIndex = gender == "M" ? Random.Range (0, 3) : Random.Range (0, 3);

		int colorIndex = Random.Range (0, 5);
		string colorName = "n";
		switch (colorIndex) 
		{
		case 0:
			colorName = "r";
			break;
		case 1: 
			colorName = "g";
			break;
		case 2:
			colorName = "b";
			break;
		case 3:
			colorName = "n";
			break;
		}

		switch (speedCategory) 
		{
		case SpeedCategory.SLOW:
			colorName = "g";
			break;
		case SpeedCategory.NORMAL:
			colorName = Random.Range(0,2) == 1 ? "n" : "b";
			break;
		case SpeedCategory.FAST:
			colorName = "r";
			break;
		}

		int armIndex = Random.Range(0, 2);
		int legIndex = Random.Range (0, 7);

		string headSpriteName = headIndex + "_" + gender + "_headF_" + colorName;
		string headSpriteTurnedName = headIndex + "_" + gender + "_headS_" + colorName;
		string bodySpriteName = bodyIndex + "_body_" + gender + "_" + colorName;
		string legSpriteName = "leg_" + legIndex;
		string armSpriteName = "arm_" + colorName + "_" + armIndex;

		personController.headSprite = Resources.Load<Sprite>(headSpriteName);
		personController.headTurnedSprite = Resources.Load<Sprite> (headSpriteTurnedName);
		personController.bodySprite = Resources.Load<Sprite> (bodySpriteName);
		personController.legSprite = Resources.Load<Sprite> (legSpriteName);
		personController.armSprite = Resources.Load<Sprite> (armSpriteName);

	}
	
	private SpeedCategory getRandomSpeedCategory()
	{
		int rand = Random.Range (0, 3);
		SpeedCategory returnSpeedCategory = SpeedCategory.SLOW;
		switch (rand) 
		{
			case 0:
			returnSpeedCategory = SpeedCategory.SLOW;	
			break;
			case 1:
			returnSpeedCategory = SpeedCategory.NORMAL;
			break;
			case 2:
			returnSpeedCategory = SpeedCategory.FAST;
			break;
		}
		return returnSpeedCategory;
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
		}
	}


}
