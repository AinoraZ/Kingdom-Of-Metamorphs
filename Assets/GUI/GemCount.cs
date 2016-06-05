using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GemCount : MonoBehaviour {

	public int gems = 0;
	public Text text;
	public GameObject thing;

	void Update () {
		text.text = gems + " gems";
	}

	public void DoPoints () {
		int addz = 2;
		for(int i=0; i < Uti.ListLength(thing.GetComponent<MoveHandler> ().rePoints); i++){
			if(thing.GetComponent<MoveHandler> ().rePoints[i].GetComponent<TileInfo>().minion != null){
				if(thing.GetComponent<MoveHandler> ().rePoints[i].GetComponent<TileInfo>().minion.tag == "P1" && thing.GetComponent<MoveHandler> ().rePoints[i].name != "Friendly"){
					addz += 2;
				}
			}
		}
		gems += addz;
	}
}
