using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TurnHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(Starting());
	}

	public GameObject turn;
	public GameObject tip;
	bool firstTime = false;
	public int turns = 0;

	void Update() {
		if (firstTime) {
			if (FriendlySpawn) {
				tip.GetComponent<Text>().text = "Click on one of the cyan blocks to place your first minion!";
			}
			else if (GameObject.FindGameObjectWithTag("Friendly").GetComponent<Minion>().moved && Move) {
				tip.GetComponent<Text>().text = "Now press the END TURN button and watch your enemy make his move!\n\nGood luck!";
			}
			else if (Move) {
				tip.GetComponent<Text>().text = "Click on your minion and move him to one of the green blocks!";
			}
		}
	}

	IEnumerator Starting() {
		yield return new WaitForSeconds(1);
		GetComponent<Spawner>().FriendlySpawn();
		FriendlySpawn = true;
		DisplayStringTurn("Spawn");
		firstTime = true;
	}

	public bool FriendlySpawn = false;
	public bool FriendlyAddSpawn = false;
	public bool Move = false;

	public void SwitchTurns() {
		if (FriendlySpawn) {
			GetComponent<Spawner>().FriendlySpawn();
			DisplayStringTurn("Spawn");
		}
		else if (FriendlyAddSpawn) {
			GetComponent<Spawner>().FriendlyAddSpawn();
		}
		else if (Move) {
			GetComponent<MoveHandler>().movingTurn = true;
			DisplayStringTurn("Move/Attack");
		}
	}

	void DisplayStringTurn(string display) {
		turn.GetComponent<Text>().text = display;
	}

	public List<string> tips;

	IEnumerator RandomTip() {
		tip.GetComponent<Text>().text = tips[new System.Random().Next(Uti.ListLength(tips))];
		yield return new WaitForSeconds(10f);
		StartCoroutine(RandomTip());
	}

	public void EndTurn() {
		FriendlySpawn = false;
		FriendlyAddSpawn = false;
		Move = false;
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().ColorReset();
		}
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Friendly").Length; x++) {
			GameObject.FindGameObjectsWithTag("Friendly")[x].GetComponent<Minion>().moved = false;
			GameObject.FindGameObjectsWithTag("Friendly")[x].GetComponent<MinionInfo>().Switch();
		}
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Enemy").Length; x++) {
			GameObject.FindGameObjectsWithTag("Enemy")[x].GetComponent<MinionInfo>().Switch();
		}
		GetComponent<Spawner>().TurnEnd();
		GetComponent<MoveHandler>().TurnEnd();
		GetComponent<CardHandler>().TurnEnd();
		DisplayStringTurn("Enemy");
		if (firstTime) {
			StartCoroutine(RandomTip());
			firstTime = false;
		}
		turns++;
		StartCoroutine(GetComponent<EnemyAI>().EnemyInit());
	}
}
