using UnityEngine;
using System.Collections;

public class FortressSpawner : MonoBehaviour {

	public int atk;
	public int def;
	public GameObject enemy;

	// Use this for initialization
	void Start () {
		GameObject enemyTemp = Instantiate(enemy, transform.position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
		enemyTemp.GetComponent<MinionInfo>().atk = atk;
		enemyTemp.GetComponent<MinionInfo>().def = def;
		GetComponent<TileInfo>().MinionChange(enemyTemp);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
