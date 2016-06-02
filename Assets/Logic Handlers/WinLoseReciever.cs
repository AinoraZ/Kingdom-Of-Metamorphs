using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLoseReciever : MonoBehaviour {

	public Sprite won;
	public Sprite lose;
	public GameObject text;
	public GameObject thing;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}

	public void EndGame(bool win) {
		if (win) {
			thing.GetComponent<Image>().sprite = won;
		}
		else {
			thing.GetComponent<Image>().sprite = lose;
		}
		text.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TurnHandler>().turns + " turns";
		thing.SetActive(true);
		text.SetActive(true);
		StartCoroutine(restart());
	}
	IEnumerator restart() {
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene(2);
	}

}
