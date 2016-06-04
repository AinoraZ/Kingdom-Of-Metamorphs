using UnityEngine;
using System.Collections;

public class spwncrd : MonoBehaviour {

	public CardManager manager;

	public void spwn () {
		
		manager.AddCard (0);
	}
}
