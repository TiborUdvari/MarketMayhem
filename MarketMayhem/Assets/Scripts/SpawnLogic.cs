using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnLogic : MonoBehaviour {

	private Vector3 spawnPoint;
	public GameObject cubeC;
	//public List<GameObject> personList;
	private GameObject personClone;

	public int rowNumber = 5;
	private float xPos;
	private float yPos;
	private float bottomPadding = 40;
	private float topPadding = 40;
	public List<float> ySpawnPosList = new List<float>();


	void Start () {

		PopulateSpawnPosition ();
	}
	

	void FixedUpdate () {
	
	}

	void PopulateSpawnPosition() {
		float step = (Screen.height - bottomPadding - topPadding) / rowNumber;
		for (int i = 0; i < rowNumber; i++) {
			float yPos = bottomPadding + i * step + step/2;
			ySpawnPosList.Add(yPos);
			spawnPoint = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width * 0.5f, yPos, 10));
			personClone = Instantiate (cubeC, spawnPoint, Quaternion.identity) as GameObject;
		}
		Debug.Log (ySpawnPosList.Count);
	}
}
