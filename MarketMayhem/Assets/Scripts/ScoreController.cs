using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreController : MonoBehaviour {

	public List<GameObject> tomatoesList = new List<GameObject>();

	private int mistakes;
	public int Mistakes
	{
		get { return mistakes;}
		set {
			mistakes = value;

			if (mistakes >= 0)
			{
				AudioClip audioClip = Resources.Load("tomato") as AudioClip;
				AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);
			}

			for (int i = 0; i < tomatoesList.Count; i++)
			{
				GameObject tomato = tomatoesList[i];

				if (i < mistakes)
				{
					tomato.SetActive(true);
				}
				else
				{
					tomato.SetActive(false);
				}
			}
			if (mistakes >= 3)
			{
				StartCoroutine(endGameIn2Seconds());
			}
		}

	}

	IEnumerator endGameIn2Seconds()
	{
		yield return new WaitForSeconds (1);

		//TODO end game

		Debug.Log("End game");
		Application.LoadLevel(3);
	}
	
	public TextMesh scoreText;
	private int score;
	public int Score
	{
		get { return score;}
		set {
			score = value;
			scoreText.text = score.ToString();
		}
	}

	// Use this for initialization
	void Start () 
	{
		scoreText = GameObject.Find ("Scoretext").GetComponent<TextMesh>();
		Score = 0;
		Mistakes = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
