using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour {
	void Update () {
		if (!moved && GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TurnHandler>().Move)
			GetComponent<SpriteRenderer>().color = Color.Lerp(Color.blue, Color.cyan, Mathf.PingPong(Time.time, 1));
		else
			GetComponent<SpriteRenderer>().color = Color.blue;
	}

	public bool moved = false;
}
