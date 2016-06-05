using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveHandler : MonoBehaviour {

	bool makingMove = false;
	List<GameObject> posMove;
	GameObject movingFromTile;
	public bool movingTurn = false;
	public List <Teams> teams;
	public List<GameObject> rePoints;
	public int moveBy = 2;
	public int forTurns;

	void FixedUpdate() {
		if (gameObject.GetComponent<TurnHandler>().turns <= forTurns) {
			moveBy = 2;
		}
		else {
			moveBy = 1;
		}
	}

	public void TileClicked(GameObject tile) {
		if (movingTurn) {
			if (!makingMove && tile.GetComponent<TileInfo>().minion != null) {
				if (tile.GetComponent<TileInfo>().minion.tag == "P1" && !tile.GetComponent<TileInfo>().minion.GetComponent<Minion>().moved) { //!makingMove && tile.GetComponent<TileInfo>().unit
					for(int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
						GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().ColorReset();
					}
					makingMove = true;
					movingFromTile = tile;
					List<GameObject> tempPossibleMoveList = new List<GameObject>();
					Vector2 tilePos = tile.GetComponent<TileInfo>().tilePos;
					for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
						if (Mathf.Abs(tilePos.x - GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos.x) <= moveBy &&
							Mathf.Abs(tilePos.y - GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos.y) <= moveBy) {
							if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
								if ((GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag == "P2")) {
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
						if (match.GetComponent<TileInfo>().minion.tag == "P2") {
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
		if(to.GetComponent<TileInfo>().minion.tag == "P1")
			to.GetComponent<TileInfo>().minion.GetComponent<Minion>().moved = true;
		if (to.GetComponent<TileInfo>().tilePos == new Vector2(1, 20) && to.GetComponent<TileInfo>().minion.tag == "P1") {
			GameObject.Find("WinScreen").GetComponent<WinLoseReciever>().EndGame(true);
		}
		if (to.GetComponent<TileInfo>().tilePos == new Vector2(20, 1) && to.GetComponent<TileInfo>().minion.tag == "P2") {
			GameObject.Find("WinScreen").GetComponent<WinLoseReciever>().EndGame(false);
		}
	}

	public void Attack(GameObject from, GameObject to) {
		to.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().def -= from.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().atk;
		from.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().def -= to.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().atk;
		try {
			from.GetComponent<TileInfo>().minion.GetComponent<Minion>().moved = true;
		}
		catch {
			Debug.Log("Caught");
		}
		if (to.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().def <= 0 &&
			from.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().def > 0) {
			if(from.GetComponent<TileInfo>().minion.tag == "P1")
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CardHandler>().CardAdd();
			to.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().DestroyCall();
			Transfer(from, to);
		}
		if (to.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().def <= 0) {
			if (from.GetComponent<TileInfo>().minion.tag == "P1")
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

	/*public static bool FriendlyCheck(string tagName, int currentTeam)
	{
		MoveHandler component = GameObject.Find("Main Camera").GetComponent<MoveHandler>();
		for (int x = 0; x < Uti.ListLength(component.teams[currentTeam].members); x++)
		{
			if (tagName == component.teams[currentTeam].members[x])
			{
				return true;
			}
		}
		return false;
	}*/

	/*public static bool BaseTaken(Vector2 pos, int team) {
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos == pos) {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
					if (FriendlyCheck(GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag, team)) {
						return true;
					}
				}
			}
		}
		return false;
	}*/
}

[System.Serializable]
public class Teams
{
	public List<string> members;
	public List<GameObject> bases;
}