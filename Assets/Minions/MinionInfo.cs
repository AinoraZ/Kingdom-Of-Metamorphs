using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MinionInfo : MonoBehaviour {

	public int atk = 4;
	public int def = 4;
	public int cost = 2;
	public GameObject Text;

	public Sprite[] shape;

	GameObject stats;
	void Start() {
		stats = Instantiate(Text, transform.position, transform.rotation) as GameObject;
		stats.transform.SetParent(GameObject.Find("Canvas").transform, false);
		stats.transform.position = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (stats.transform.position != transform.position) {
			stats.transform.position = transform.position;
		}
		if (stats.GetComponent<Text>().text != atk + "/" + def) {
			stats.GetComponent<Text>().text = atk + "/" + def;
		}
		if (def <= 0) {
			for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion == gameObject) {
					GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion = null;
					DestroyCall();
				}
			}
		}
	}

	public void DestroyCall() {
		Destroy(stats);
		Destroy(gameObject);
	}

	void OnMouseDown() {
		if (Input.GetKey("mouse 0")) {
			for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion == gameObject) {
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveHandler>().TileClicked(GameObject.FindGameObjectsWithTag("Tile")[x]);
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CardHandler>().MinionClicked(GameObject.FindGameObjectsWithTag("Tile")[x]);
				}
			}
		}
	}

	public void Switch() {
		int temp = atk;
		atk = def;
		def = temp;
		if (GetComponent<SpriteRenderer>().sprite == shape[0]) {
			GetComponent<SpriteRenderer>().sprite = shape[1];
		}
		else {
			GetComponent<SpriteRenderer>().sprite = shape[0];
		}
	}
}
