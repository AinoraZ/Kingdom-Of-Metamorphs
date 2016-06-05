using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {

	public int typeID;
	public CardManager manager;

	List<GameObject> possibleMove;

	public int uniqueID;

	public void Use () {

		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag == "P1") {
					GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().PossibleMove();
					possibleMove.Add(GameObject.FindGameObjectsWithTag("Tile")[x]);
				}
			}
		}
		manager.inUse = true;
		manager.clicked = uniqueID;
	}

	public void Clear(){
		for (int i=0; i < Uti.ListLength(possibleMove); i++) {
			possibleMove [i].GetComponent<TileInfo> ().ColorReset ();
		}
		possibleMove = new List<GameObject> ();
	}
}
