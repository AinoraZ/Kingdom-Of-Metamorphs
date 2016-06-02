using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NextLevel2 : MonoBehaviour {


	// Use this for initialization
	void Start() {
		StartCoroutine(NextLevel());
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			SceneManager.LoadScene(2);
		}
	}

	IEnumerator NextLevel() {
		yield return new WaitForSeconds(10f);
		SceneManager.LoadScene(2);
	}
}
