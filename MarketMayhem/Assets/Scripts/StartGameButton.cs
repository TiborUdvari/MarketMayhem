using UnityEngine;
using System.Collections;

public class StartGameButton : MonoBehaviour {

	public Color defaultColor;
	public Color selectedColor;
	private Material mat;

	void Start() {
		mat = renderer.material;
	}

	void OnTouchDown() {
		mat.color = selectedColor;
		if (Application.loadedLevel == 1) {
			Application.LoadLevel(2);
		} else if (Application.loadedLevel == 3) {
			Application.LoadLevel(2);
		}
	}

	void OnTouchUp() {
		mat.color = defaultColor;
	}

	void OnTouchStay() {
		mat.color = selectedColor;
	}

	void OnTouchExit() {
		mat.color = defaultColor;
	}

}
