using UnityEngine;
using System.Collections;

public class Cards : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		if (Input.GetKey("mouse 0")) {
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CardHandler>().CardClicked(gameObject);
		}
	}
}
