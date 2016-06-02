using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveHandler : MonoBehaviour {

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	bool makingMove = false;
	List<GameObject> posMove;
	GameObject movingFromTile;
	public bool movingTurn = false;

	public void TileClicked(GameObject tile) {
		if (movingTurn) {
			if (!makingMove && tile.GetComponent<TileInfo>().minion != null) {
				if (tile.GetComponent<TileInfo>().minion.tag == "Friendly" && !tile.GetComponent<TileInfo>().minion.GetComponent<Minion>().moved) { //!makingMove && tile.GetComponent<TileInfo>().unit
					for(int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
						GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().ColorReset();
					}
					makingMove = true;
					movingFromTile = tile;
					List<GameObject> tempPossibleMoveList = new List<GameObject>();
					Vector2 tilePos = tile.GetComponent<TileInfo>().tilePos;
					for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
						if ((Mathf.Abs(tilePos.x - GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos.x) == 1 &&
							Mathf.Abs(tilePos.y - GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos.y) == 1) ||
							(Mathf.Abs(tilePos.x - GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos.x) == 0 &&
							Mathf.Abs(tilePos.y - GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos.y) == 1) ||
							(Mathf.Abs(tilePos.x - GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos.x) == 1 &&
							Mathf.Abs(tilePos.y - GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos.y) == 0)) {
							if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
								if (!(GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag == "Friendly")) {
									GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().PossibleMove();
									tempPossibleMoveList.Add(GameObject.FindGameObjectsWithTag("Tile")[x]);
								}
							}
							else {
								GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().PossibleMove();
								tempPossibleMoveList.Add(GameObject.FindGameObjectsWithTag("Tile")[x]);
							}
						}
					}
					posMove = new List<GameObject>(tempPossibleMoveList);
				}
			}
			else {
				GameObject match = Uti.PossibleMoveCheck(tile, posMove);
				if (match != null) {
					if (match.GetComponent<TileInfo>().minion != null) {
						if (match.GetComponent<TileInfo>().minion.tag == "Enemy") {
							Attack(movingFromTile, match);
						}
					}
					else
						Transfer(movingFromTile, match);
				}
				Uti.Reset(posMove, movingFromTile);
				posMove = new List<GameObject>();
				makingMove = false;
			}
		}
	}

	public void Transfer(GameObject from, GameObject to) {
		from.GetComponent<TileInfo>().minion.transform.position = to.transform.position;
		to.GetComponent<TileInfo>().minion = from.GetComponent<TileInfo>().minion;
		from.GetComponent<TileInfo>().minion = null;
		if(to.GetComponent<TileInfo>().minion.tag == "Friendly")
			to.GetComponent<TileInfo>().minion.GetComponent<Minion>().moved = true;
		if (to.GetComponent<TileInfo>().tilePos == new Vector2(10, 1) && to.GetComponent<TileInfo>().minion.tag == "Friendly") {
			GameObject.Find("WinScreen").GetComponent<WinLoseReciever>().EndGame(true);
		}
		if (to.GetComponent<TileInfo>().tilePos == new Vector2(1, 10) && to.GetComponent<TileInfo>().minion.tag == "Enemy") {
			GameObject.Find("WinScreen").GetComponent<WinLoseReciever>().EndGame(false);
		}
	}

	public void Attack(GameObject from, GameObject to) {
		to.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().def -= from.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().atk;
		from.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().def -= to.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().atk;
		if (to.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().def <= 0 &&
			from.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().def > 0) {
			if(from.GetComponent<TileInfo>().minion.tag == "Friendly")
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CardHandler>().CardAdd();
			to.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().DestroyCall();
			Transfer(from, to);
		}
		if (to.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().def <= 0) {
			if (from.GetComponent<TileInfo>().minion.tag == "Friendly")
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CardHandler>().CardAdd();
		}
	}

	public void TurnEnd() {
		makingMove = false;
		movingTurn = false;
	}
}

public class Uti {
	public static int ListLength<T>(List<T> data) {
		int n = 0;
		while (true) {
			try {
				data[n] = data[n];
				n++;
			}
			catch {
				return n;
			}
		}
	}

	public static void Reset(List<GameObject> posMove, GameObject movingFromTile = null) {
		for (int x = 0; x < Uti.ListLength(posMove); x++) {
			posMove[x].GetComponent<TileInfo>().ColorReset();
		}
		if (movingFromTile != null)
			movingFromTile.GetComponent<TileInfo>().ColorReset();
	}

	public static GameObject PossibleMoveCheck(GameObject tile, List<GameObject> spawn) {
		for (int x = 0; x < Uti.ListLength(spawn); x++) {
			if (tile == spawn[x]) {
				return spawn[x];
			}
		}
		return null;
	}
}
