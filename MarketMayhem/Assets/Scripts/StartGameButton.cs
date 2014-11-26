using UnityEngine;
using System.Collections;

public class StartGameButton : MonoBehaviour {

	void OnTouchDown() {
		if (Application.loadedLevel == 1) {
			Application.LoadLevel(2);
		} else if (Application.loadedLevel == 3) {
			Application.LoadLevel(2);
		}
	}


}
