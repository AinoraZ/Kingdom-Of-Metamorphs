using UnityEngine;
using System.Collections;

public class Cards : MonoBehaviour {
	public void OButton() {
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CardHandler>().CardClicked(gameObject);
		GameObject.Find("CardButton").GetComponent<ToggleMenu>().Toggle();
	}
}
