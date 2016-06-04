using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	List<GameObject> friendlySpawn = new List<GameObject>();
	List<GameObject> enemySpawn = new List<GameObject>();
	List<GameObject> spawner1List = new List<GameObject>();
	List<GameObject> spawner2List = new List<GameObject>();
	public GameObject friendlyMinion;

	void Refresh() {
		List<GameObject> tiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Tile"));
		for (int x = 0; x < Uti.ListLength(tiles); x++) {
			if ((Mathf.Abs(1 - tiles[x].GetComponent<TileInfo>().tilePos.x) == 0 &&
				Mathf.Abs(10 - tiles[x].GetComponent<TileInfo>().tilePos.y) == 1) ||
				(Mathf.Abs(1 - tiles[x].GetComponent<TileInfo>().tilePos.x) == 1 &&
				Mathf.Abs(10 - tiles[x].GetComponent<TileInfo>().tilePos.y) == 0)) {
			friendlySpawn.Add(tiles[x]);
			}
		}
		for (int x = 0; x < Uti.ListLength(tiles); x++) {
			if ((Mathf.Abs(1 - tiles[x].GetComponent<TileInfo>().tilePos.x) == 0 &&
				Mathf.Abs(1 - tiles[x].GetComponent<TileInfo>().tilePos.y) == 1) ||
				(Mathf.Abs(1 - tiles[x].GetComponent<TileInfo>().tilePos.x) == 1 &&
				Mathf.Abs(1 - tiles[x].GetComponent<TileInfo>().tilePos.y) == 0)) {
				spawner1List.Add(tiles[x]);
			}
		}
		for (int x = 0; x < Uti.ListLength(tiles); x++) {
			if ((Mathf.Abs(10 - tiles[x].GetComponent<TileInfo>().tilePos.x) == 0 &&
				Mathf.Abs(10 - tiles[x].GetComponent<TileInfo>().tilePos.y) == 1) ||
				(Mathf.Abs(10 - tiles[x].GetComponent<TileInfo>().tilePos.x) == 1 &&
				Mathf.Abs(10 - tiles[x].GetComponent<TileInfo>().tilePos.y) == 0)) {
				spawner2List.Add(tiles[x]);
			}
		}
	}

	void Awake() {
		Refresh();
	}

	bool friendlySpawning = false;

	public void FriendlySpawn() {
		Refresh();
		for (int x = 0; x < Uti.ListLength(friendlySpawn); x++) {
			if (friendlySpawn[x].GetComponent<TileInfo>().minion == null) {
				friendlySpawn[x].GetComponent<TileInfo>().PossibleSpawn();
				friendlySpawning = true;
			}
		}
		if (!friendlySpawning) {
			GetComponent<TurnHandler>().FriendlySpawn = false;
			GetComponent<TurnHandler>().FriendlyAddSpawn = true;
			GetComponent<TurnHandler>().SwitchTurns();
		}
	}

	bool friendlySpawner1 = false;
	bool friendlySpawner2 = false;

	public void FriendlyAddSpawn(bool secondCheck = false) {
		Refresh();
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if (!secondCheck) {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos == new Vector2(1, 1)) {
					if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
						if (Uti.FriendlyCheck(GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag, 0)) {
							for (int z = 0; z < Uti.ListLength(spawner1List); z++) {
								if (spawner1List[z].GetComponent<TileInfo>().minion == null) {
									spawner1List[z].GetComponent<TileInfo>().PossibleSpawn();
									friendlySpawner1 = true;
								}
							}
						}
						if (!friendlySpawner1){
							FriendlyAddSpawn(true);
						}
					}
					else {
						FriendlyAddSpawn(true);
					}
				}
			}
			else {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos == new Vector2(10, 10)) {
					if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
						if (Uti.FriendlyCheck(GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag, 0)) {
							for (int z = 0; z < Uti.ListLength(spawner2List); z++) {
								if (spawner2List[z].GetComponent<TileInfo>().minion == null) {
									spawner2List[z].GetComponent<TileInfo>().PossibleSpawn();
									friendlySpawner2 = true;
								}
							}
						}
						if (!friendlySpawner2) {
							GetComponent<TurnHandler>().FriendlyAddSpawn = false;
							GetComponent<TurnHandler>().Move = true;
							GetComponent<TurnHandler>().SwitchTurns();
						}
					}
					else {
						GetComponent<TurnHandler>().FriendlyAddSpawn = false;
						GetComponent<TurnHandler>().Move = true;
						GetComponent<TurnHandler>().SwitchTurns();
					}
				}
			}
		}
	}

	public void TileClicked(GameObject tile) {
		if (friendlySpawning) {
			GameObject match = Uti.PossibleMoveCheck(tile, friendlySpawn);
			if (match != null) {
				if (tile.GetComponent<TileInfo>().minion == null) {
					GameObject temp = Instantiate(friendlyMinion, match.transform.position, transform.rotation) as GameObject;
					match.GetComponent<TileInfo>().MinionChange(temp);
					Uti.Reset(friendlySpawn);
					friendlySpawning = false;
					GetComponent<TurnHandler>().FriendlySpawn = false;
					GetComponent<TurnHandler>().FriendlyAddSpawn = true;
					GetComponent<TurnHandler>().SwitchTurns();
				}
			}
		}
		else if (friendlySpawner1) {
			GameObject match = Uti.PossibleMoveCheck(tile, spawner1List);
			if (match != null) {
				GameObject temp = Instantiate(friendlyMinion, match.transform.position, transform.rotation) as GameObject;
				match.GetComponent<TileInfo>().MinionChange(temp);
				Uti.Reset(spawner1List);
				friendlySpawner1 = false;
				FriendlyAddSpawn(true);
			}
		}
		else if (friendlySpawner2) {
			GameObject match = Uti.PossibleMoveCheck(tile, spawner2List);
			if (match != null) {
				GameObject temp = Instantiate(friendlyMinion, match.transform.position, transform.rotation) as GameObject;
				match.GetComponent<TileInfo>().MinionChange(temp);
				Uti.Reset(spawner2List);
				friendlySpawner2 = false;
				GetComponent<TurnHandler>().FriendlyAddSpawn = false;
				GetComponent<TurnHandler>().Move = true;
				GetComponent<TurnHandler>().SwitchTurns();
			}
		}
	}

	public void TurnEnd() {
		friendlySpawning = false;
		friendlySpawner1 = false;
		friendlySpawner2 = false;
	}
}