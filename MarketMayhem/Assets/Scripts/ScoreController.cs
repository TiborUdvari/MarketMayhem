using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {
	
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
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
