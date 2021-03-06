﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {

	public GameObject[] enemy;
	public int atk;
	public int def;
	public int moveBy;
	public int gemCount = 0;
	public int gemAmount = 14;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator EnemyInit() {
		moveBy = GameObject.Find("Main Camera").GetComponent<MoveHandler>().moveBy;
		GemAmountCounter();
		gemCount += gemAmount;
		Spawn(SpawnLocal());
		yield return new WaitForSeconds(0.1f);
		MovementStart();
		//--Debug--//
		yield return new WaitForSeconds(0.3f);
		GetComponent<TurnHandler>().FriendlySpawn = true;
		GetComponent<TurnHandler>().SwitchTurns();
		//--Debug--//
	}

	public void GemAmountCounter() {
		int tempGems = 14;
		for (int x = 0; x < Uti.ListLength(GameObject.Find("Main Camera").GetComponent<MoveHandler>().rePoints); x++) {
			if (GameObject.Find("Main Camera").GetComponent<MoveHandler>().rePoints[x].name != "Friendly") {
				if (GameObject.Find("Main Camera").GetComponent<MoveHandler>().rePoints[x].GetComponent<TileInfo>().minion != null) {
					if (GameObject.Find("Main Camera").GetComponent<MoveHandler>().rePoints[x].GetComponent<TileInfo>().minion.tag != "P2") {
						tempGems -= 2;
					}
				}
				else {
					tempGems -= 2;
				}
			}
		}
		gemAmount = tempGems;
	}

	public GameObject MinionChooser() {
		for (int x = enemy.Length - 1; x >= 0; x--) {
			if (gemCount >= enemy[x].GetComponent<MinionInfo>().cost) {
				return enemy[x];
			}
		}
		return enemy[2];
	}

	//----------SPAWNING-----------//

	//--LocalSpawning--//
	public void Spawn(GameObject tile) {
		if (tile != null) {
			GameObject enemyTemp = Instantiate(MinionChooser(), tile.transform.position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
			gemCount -= MinionChooser().GetComponent<MinionInfo>().cost;
			atk = enemyTemp.GetComponent<MinionInfo>().atk;
			def = enemyTemp.GetComponent<MinionInfo>().def;
			tile.GetComponent<TileInfo>().MinionChange(enemyTemp);
		}
	}

	public GameObject SpawnLocal() {
		Refresh();
		return SpawnIn(posLocalSpawn);
	}

	private GameObject SpawnIn(List<GameObject> spawnPlace) {
		if (Uti.ListLength(spawnPlace) > 0)
			return spawnPlace[new System.Random().Next(Uti.ListLength(spawnPlace))];
		else
			return null;
	}
	//--LocalSpawning--//

	//--Refresh--//
	List<GameObject> posLocalSpawn = new List<GameObject>();
	List<GameObject> spawner1List = new List<GameObject>();
	List<GameObject> spawner2List = new List<GameObject>();
	private void Refresh() {
		List<GameObject> tiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Tile"));
		posLocalSpawn = new List<GameObject>();
		spawner1List = new List<GameObject>();
		spawner2List = new List<GameObject>();
		for (int x = 0; x < Uti.ListLength(tiles); x++) {
			if ((Mathf.Abs(1 - tiles[x].GetComponent<TileInfo>().tilePos.x) == 0 &&
				Mathf.Abs(20 - tiles[x].GetComponent<TileInfo>().tilePos.y) == 1) ||
				(Mathf.Abs(1 - tiles[x].GetComponent<TileInfo>().tilePos.x) == 1 &&
				Mathf.Abs(20 - tiles[x].GetComponent<TileInfo>().tilePos.y) == 0)) {
				if (tiles[x].GetComponent<TileInfo>().minion == null) {
					posLocalSpawn.Add(tiles[x]);
				}
			}
		}
		MoveHandler handler = GameObject.Find("Main Camera").GetComponent<MoveHandler>();
		for (int x = 0; x < Uti.ListLength(tiles); x++) {
			for (int z = 0; z < Uti.ListLength(handler.rePoints); z++) {
				Vector2 rePointPos = handler.rePoints[z].GetComponent<TileInfo>().tilePos;
				if ((Mathf.Abs(rePointPos.x - tiles[x].GetComponent<TileInfo>().tilePos.x) == 0 &&
					Mathf.Abs(rePointPos.y - tiles[x].GetComponent<TileInfo>().tilePos.y) == 1) ||
					(Mathf.Abs(rePointPos.x - tiles[x].GetComponent<TileInfo>().tilePos.x) == 1 &&
					Mathf.Abs(rePointPos.y - tiles[x].GetComponent<TileInfo>().tilePos.y) == 0)) {
					if (handler.rePoints[z].GetComponent<TileInfo>().minion != null) {
						if (handler.rePoints[z].GetComponent<TileInfo>().minion.tag == "P2") {
							posLocalSpawn.Add(tiles[x]);
						}
					}
				}
			}
		}

		Debug.Log(Uti.ListLength(posLocalSpawn));
	}

		/*for (int x = 0; x < Uti.ListLength(tiles); x++) {
			if ((Mathf.Abs(1 - tiles[x].GetComponent<TileInfo>().tilePos.x) == 0 &&
				Mathf.Abs(1 - tiles[x].GetComponent<TileInfo>().tilePos.y) == 1) ||
				(Mathf.Abs(1 - tiles[x].GetComponent<TileInfo>().tilePos.x) == 1 &&
				Mathf.Abs(1 - tiles[x].GetComponent<TileInfo>().tilePos.y) == 0)) {
				if (tiles[x].GetComponent<TileInfo>().minion == null) {
					spawner1List.Add(tiles[x]);
				}
			}
		}
		for (int x = 0; x < Uti.ListLength(tiles); x++) {
			if ((Mathf.Abs(20 - tiles[x].GetComponent<TileInfo>().tilePos.x) == 0 &&
				Mathf.Abs(20 - tiles[x].GetComponent<TileInfo>().tilePos.y) == 1) ||
				(Mathf.Abs(20 - tiles[x].GetComponent<TileInfo>().tilePos.x) == 1 &&
				Mathf.Abs(20 - tiles[x].GetComponent<TileInfo>().tilePos.y) == 0)) {
				if (tiles[x].GetComponent<TileInfo>().minion == null) {
					spawner2List.Add(tiles[x]);
				}
			}
		}*/
	//--Refresh--//

	//--Capturable Point Spawning--//
	/*public void EnemyAddSpawn(bool secondCheck = false) {
		Refresh();
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if (!secondCheck) {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos == new Vector2(1, 1)) {
					if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
						if ((GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag == "P2")) {
							Spawn(SpawnIn(spawner1List));
							EnemyAddSpawn(true);
						}
						else {
							EnemyAddSpawn(true);
						}
					}
					else {
						EnemyAddSpawn(true);
					}
				}
			}
			else {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos == new Vector2(20, 20)) {
					if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
						if ((GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag == "P2")) {
							Spawn(SpawnIn(spawner2List));
						}
					}
				}
			}
		}
	}*/
	//--Capturable Point Spawning--//

	//----------SPAWNINGend-----------//

	//----------JOB ASSIGN-----------//

	private void MovementStart() {
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag == "P2") {
					PossibleEnemyMove(GameObject.FindGameObjectsWithTag("Tile")[x]);
				}
			}
		}
	}

	void PossibleEnemyMove(GameObject tile) {
		List<GameObject> posMove = new List<GameObject>();
		Vector2 tilePos = tile.GetComponent<TileInfo>().tilePos;
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if ((Mathf.Abs(tilePos.x - GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos.x) <= moveBy &&
				Mathf.Abs(tilePos.y - GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos.y) <= moveBy)) {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
					if (!(GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag == "P2")) {
						posMove.Add(GameObject.FindGameObjectsWithTag("Tile")[x]);
					}
				}
				else {
					posMove.Add(GameObject.FindGameObjectsWithTag("Tile")[x]);
				}
			}
		}
		JobAssign(tile, posMove);
	}

	void JobAssign(GameObject tile, List<GameObject> posMove) {
		JobRefresh(tile);
		List<GameObject> possibleJobs = new List<GameObject>();
		if (AttackNear(tile, posMove) != null) {
			tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective = AttackNear(tile, posMove);
		}
		else if (tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective == null) {
			if (PointNotProtected(new Vector2(1, 20)) != null) {
				tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective = PointNotProtected(new Vector2(1, 20));
			}
			else if (AttackNearBase(tile) != null) {
				tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective = AttackNearBase(tile);
			}
			else {
				/*if (PointNotProtected(new Vector2(1, 1)) != null) {
					possibleJobs.Add(PointNotProtected(new Vector2(1, 1)));
				}
				if (PointNotProtected(new Vector2(20, 20)) != null) {
					possibleJobs.Add(PointNotProtected(new Vector2(20, 20)));
				}
				if (PointNotProtected(new Vector2(20, 1)) != null) {
					possibleJobs.Add(PointNotProtected(new Vector2(20, 1)));
				}*/
				MoveHandler jobHandler = GameObject.Find("Main Camera").GetComponent<MoveHandler>();

				for (int x = 0; x < Uti.ListLength(jobHandler.rePoints); x++) {
					if (PointNotProtected(jobHandler.rePoints[x].GetComponent<TileInfo>().tilePos) != null) {
						possibleJobs.Add(PointNotProtected(jobHandler.rePoints[x].GetComponent<TileInfo>().tilePos));
					}
				}

				tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective = possibleJobs[new System.Random().Next(Uti.ListLength(possibleJobs))];
				possibleJobs = new List<GameObject>();
			}
		}
		MoveToObjectiveCheck(tile, posMove);
	}

	void JobRefresh(GameObject tile) {
		if (tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective != null){
			if (tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective.tag != "P1") {
				if (tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective.GetComponent<TileInfo>().tilePos != tile.GetComponent<TileInfo>().tilePos) {
					if (PointNotProtected(tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective.GetComponent<TileInfo>().tilePos) == null) {
						tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective = null;
					}
				}
			}
		}
	}

	GameObject AttackNear(GameObject tile, List<GameObject> posMove) {
		for (int x = 0; x < Uti.ListLength(posMove); x++) {
				if (posMove[x].GetComponent<TileInfo>().minion != null) {
					if (posMove[x].GetComponent<TileInfo>().minion.tag == "P1") {
						if (posMove[x].GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().def - tile.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().atk <= 0) {
							return posMove[x];
						}
					}
				}
		}
		return null;
	}

	GameObject AttackNearBase(GameObject tile) {			//Called when there is an attacker near the base
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag == "P1") {
					if (Vector2.Distance(tile.GetComponent<TileInfo>().tilePos, GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos) < 3f) {
						if (Vector2.Distance(tile.GetComponent<TileInfo>().tilePos, new Vector2(1, 20)) < 3f) {
							return GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion;
						}
					}
				}
			}
		}
		return null;
	}

	bool InRange(GameObject checking, List<GameObject> posMove) {
		for (int x = 0; x < Uti.ListLength(posMove); x++) {
			if (checking == posMove[x]) {
				return true;
			}
		}
		return false;
	}

	GameObject PointNotProtected(Vector2 point) {
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos == point) {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion == null) {
					return GameObject.FindGameObjectsWithTag("Tile")[x];
				}
				else if(GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag == "P1") {
					return GameObject.FindGameObjectsWithTag("Tile")[x];
				}
			}
		}
		return null;
	}
	//----------JOB ASSIGN end-----------//

	//----------MOVING-----------//

	void MoveToObjectiveCheck(GameObject tile, List<GameObject> posMove) {
		Refresh();
		if (Uti.ListLength(posMove) > 0) {
			if (tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective != null) {
				if (tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective.tag == "P1") {
					GameObject tileMinion = TileWithMinion(tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective);
					StartCoroutine(MoveToObjective(tile, posMove, tileMinion));
				}
				else {
					if (tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective.GetComponent<TileInfo>().tilePos != tile.GetComponent<TileInfo>().tilePos)
						StartCoroutine(MoveToObjective(tile, posMove, tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective));
				}
			}
		}
	}

	IEnumerator MoveToObjective(GameObject tile, List<GameObject> posMove, GameObject objectiveTile) {
		yield return new WaitForSeconds(0.2f);
		int goToTile = ShortestDist(posMove, objectiveTile);
		if (posMove[goToTile].GetComponent<TileInfo>().minion != null) {
			if (posMove[goToTile].GetComponent<TileInfo>().minion.tag == "P1") {
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveHandler>().Attack(tile, posMove[goToTile]);
			}
		}
		else {
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveHandler>().Transfer(tile, posMove[goToTile]);
		}
	}

	int ShortestDist(List<GameObject> posMove, GameObject objectiveTile) {
		float minDist = 1000f;
		int minNumber = 0;
		for (int x = 0; x < Uti.ListLength(posMove); x++) {
			if (minDist > Vector2.Distance(posMove[x].GetComponent<TileInfo>().tilePos, objectiveTile.GetComponent<TileInfo>().tilePos)) {
				minDist = Vector2.Distance(posMove[x].GetComponent<TileInfo>().tilePos, objectiveTile.GetComponent<TileInfo>().tilePos);
				minNumber = x;
			}
		}
		return minNumber;
	}

	GameObject TileWithMinion(GameObject minion) {
		for(int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if (minion == GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion) {
				return GameObject.FindGameObjectsWithTag("Tile")[x];
			}
		}
		return null;
	}

	//----------MOVING end-----------//
}
