using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	public int typeID;
	public CardManager manager;

	public int uniqueID;

	public void Use () {
		switch (typeID) {
		case 0:
			Debug.Log ("card used");
			break;
		}
		manager.RemoveCard (uniqueID);
	}

}
