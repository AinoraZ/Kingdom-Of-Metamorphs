/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI2 : MonoBehaviour {

	public GameObject enemy;
	public GameObject baseSpawn;
	public int atk;
	public int def;
	public int currentTeam = 1;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public IEnumerator EnemyInit() {
		Spawn(SpawnLocal());
		yield return new WaitForSeconds(0.1f);
		MovementStart();
		//--Debug--//
		yield return new WaitForSeconds(0.5f);
		GetComponent<TurnHandler>().FriendlySpawn = true;
		GetComponent<TurnHandler>().SwitchTurns();
		//--Debug--//
	}

	//----------SPAWNING-----------//

	//--LocalSpawning--//
	public void Spawn(GameObject tile) {
		if (tile != null) {
			GameObject enemyTemp = Instantiate(enemy, tile.transform.position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
			enemyTemp.GetComponent<MinionInfo>().atk = atk;
			enemyTemp.GetComponent<MinionInfo>().def = def;
			tile.GetComponent<TileInfo>().MinionChange(enemyTemp);
		}
	}

	public GameObject SpawnLocal() {
		Debug.Log("Works");
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

	private void Refresh() {
		List<GameObject> tiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Tile"));
		List<GameObject> resourcePoints = GameObject.Find("Main Camera").GetComponent<MoveHandler>().resourcePoint;
		posLocalSpawn = new List<GameObject>();
		for (int x = 0; x < Uti.ListLength(tiles); x++) {
			if ((Mathf.Abs(baseSpawn.GetComponent<TileInfo>().tilePos.x - tiles[x].GetComponent<TileInfo>().tilePos.x) == 0 &&
				Mathf.Abs(baseSpawn.GetComponent<TileInfo>().tilePos.y - tiles[x].GetComponent<TileInfo>().tilePos.y) == 1) ||
				(Mathf.Abs(baseSpawn.GetComponent<TileInfo>().tilePos.x - tiles[x].GetComponent<TileInfo>().tilePos.x) == 1 &&
				Mathf.Abs(baseSpawn.GetComponent<TileInfo>().tilePos.y - tiles[x].GetComponent<TileInfo>().tilePos.y) == 0)) {
				if (tiles[x].GetComponent<TileInfo>().minion == null) {
					posLocalSpawn.Add(tiles[x]);
				}
			}
			for (int z = 0; z < Uti.ListLength(resourcePoints); x++) {
				if (Uti.BaseTaken(resourcePoints[z].GetComponent<TileInfo>().tilePos, currentTeam)) {
					if ((Mathf.Abs(resourcePoints[z].GetComponent<TileInfo>().tilePos.x - tiles[x].GetComponent<TileInfo>().tilePos.x) <= 1 &&
						Mathf.Abs(resourcePoints[z].GetComponent<TileInfo>().tilePos.y - tiles[x].GetComponent<TileInfo>().tilePos.y) <= 1) &&
						(Mathf.Abs(resourcePoints[z].GetComponent<TileInfo>().tilePos.x - tiles[x].GetComponent<TileInfo>().tilePos.x) > 0 &&
						Mathf.Abs(resourcePoints[z].GetComponent<TileInfo>().tilePos.y - tiles[x].GetComponent<TileInfo>().tilePos.y) > 0)) {
						if (tiles[x].GetComponent<TileInfo>().minion == null) {
							posLocalSpawn.Add(tiles[x]);
						}
					}
				}
			}

		} 
	}
	//--Refresh--//
	
	//----------SPAWNINGend-----------//

	//----------JOB ASSIGN-----------//

	private void MovementStart() {
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
				if (Uti.FriendlyCheck(GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag, currentTeam)) {
					PossibleEnemyMove(GameObject.FindGameObjectsWithTag("Tile")[x]);
				}
			}
		}
	}

	void PossibleEnemyMove(GameObject tile) {
		List<GameObject> posMove = new List<GameObject>();
		Vector2 tilePos = tile.GetComponent<TileInfo>().tilePos;
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if ((Mathf.Abs(tilePos.x - GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos.x) <= 1 &&
				Mathf.Abs(tilePos.y - GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos.y) <= 1)) {
				if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
					if (!Uti.FriendlyCheck(GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag, currentTeam)) {
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
			if (PointNotProtected(new Vector2(10, 1)) != null) {
				tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective = PointNotProtected(new Vector2(10, 1));
			}
			else if (AttackNearBase(tile) != null) {
				tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective = AttackNearBase(tile);
			}
			else {
				if (PointNotProtected(new Vector2(1, 1)) != null) {
					possibleJobs.Add(PointNotProtected(new Vector2(1, 1)));
				}
				if (PointNotProtected(new Vector2(10, 10)) != null) {
					possibleJobs.Add(PointNotProtected(new Vector2(10, 10)));
				}
				if (PointNotProtected(new Vector2(1, 10)) != null) {
					possibleJobs.Add(PointNotProtected(new Vector2(1, 10)));
				}
				tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective = possibleJobs[new System.Random().Next(Uti.ListLength(possibleJobs))];
				possibleJobs = new List<GameObject>();
			}
		}
		MoveToObjectiveCheck(tile, posMove);
	}

	void JobRefresh(GameObject tile) {
		if (tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective != null) {
			if (Uti.FriendlyCheck(tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective.tag, currentTeam)) {
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
				if (!Uti.FriendlyCheck(posMove[x].GetComponent<TileInfo>().minion.tag, currentTeam)) {
					if (posMove[x].GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().def - tile.GetComponent<TileInfo>().minion.GetComponent<MinionInfo>().atk <= 0) {
						return posMove[x];
					}
				}
			}
		}
		return null;
	}

	GameObject AttackNearBase(GameObject tile) {           //Called when there is an attacker near the base
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if (GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion != null) {
				if (!Uti.FriendlyCheck(GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag, currentTeam)) {
					if (Vector2.Distance(tile.GetComponent<TileInfo>().tilePos, GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().tilePos) < 3f) {
						if (Vector2.Distance(tile.GetComponent<TileInfo>().tilePos, new Vector2(10, 1)) < 3f) {
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
				else if (!Uti.FriendlyCheck(GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion.tag, currentTeam)) {
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
				if (Uti.FriendlyCheck(tile.GetComponent<TileInfo>().minion.GetComponent<Enemy>().objective.tag, currentTeam)) {
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
			if (!Uti.FriendlyCheck(posMove[goToTile].GetComponent<TileInfo>().minion.tag, currentTeam)) {
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
		for (int x = 0; x < GameObject.FindGameObjectsWithTag("Tile").Length; x++) {
			if (minion == GameObject.FindGameObjectsWithTag("Tile")[x].GetComponent<TileInfo>().minion) {
				return GameObject.FindGameObjectsWithTag("Tile")[x];
			}
		}
		return null;
	}
	//----------MOVING end-----------//
}*/
