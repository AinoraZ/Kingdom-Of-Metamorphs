using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardManager : MonoBehaviour {

	public GameObject[] CardSpawns;
	public List<GameObject> CardObjects;
	List<GameObject> Hand  = new List<GameObject>();
	int idsForEveryone = 0;


	public void AddCard (int typeID) {
		if (Uti.ListLength(Hand) >= 7) return;
		GameObject temp = Instantiate (CardObjects[typeID]) as GameObject;
		temp.GetComponent<Card>().uniqueID = idsForEveryone;
		temp.GetComponent<Card>().manager = this;
		Hand.Add (temp);
		idsForEveryone++;

		Allign ();
	}


	public void RemoveCard (int unique) {
		for (int i = 0; i < Uti.ListLength(Hand); i++) {
			if (Hand[i].GetComponent<Card>().uniqueID == unique) {
				Destroy (Hand [i]);
				Hand.RemoveAt(i);
			}
		}

		Allign ();
	}


	void Allign () {
		for(int i = 0; i < Uti.ListLength(Hand); i++){
			Hand[i].transform.SetParent (CardSpawns[i].transform);
			Hand [i].transform.localPosition = new Vector3 (0, 0, 0);
		}
	}
}
