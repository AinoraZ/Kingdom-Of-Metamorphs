using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileInfo : MonoBehaviour {

	public Vector2 tilePos;
	public bool movedOnce = false;
	public GameObject minion;
	// Use this for initialization
	void Start () {
		ColorReset();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ColorReset() {
		SpriteRenderer rend = GetComponent<SpriteRenderer>();
		rend.color = new Color(255, 255, 255);
		TileIn(rend);
		if (tilePos == new Vector2(1, 20) || tilePos == new Vector2(20, 1)) {
			rend.color = new Color(204, 0, 204);
		}
	}

	public void PossibleMove() {
		SpriteRenderer rend = GetComponent<SpriteRenderer>();
		rend.color = new Color(0, 255, 0);
	}

	public void PossibleSpawn() {
		SpriteRenderer rend = GetComponent<SpriteRenderer>();
		rend.color = new Color(0, 255, 255);
	}


	public void MinionChange(GameObject newMinion) {
		minion = newMinion;
	}

	void OnMouseDown() {
		if (Input.GetKey("mouse 0")) {
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveHandler>().TileClicked(gameObject);
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Spawner>().TileClicked(gameObject);
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CardHandler>().MinionClicked(gameObject);
			if(minion != null)
				if(minion.tag == "P1")
					GameObject.Find ("CardButton").GetComponent<CardManager> ().actualyUse (gameObject);
		}
	}

	void TileIn(SpriteRenderer rend) {
		MoveHandler handler = GameObject.Find("Main Camera").GetComponent<MoveHandler>();
		for (int x = 0; x < Uti.ListLength(handler.rePoints); x++) {
			if (tilePos == handler.rePoints[x].GetComponent<TileInfo>().tilePos) {
				rend.color = new Color(255, 255, 0);
			}
		}
	}
}