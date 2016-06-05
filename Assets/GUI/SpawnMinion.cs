using UnityEngine;
using System.Collections;

public class SpawnMinion : MonoBehaviour {

	public GameObject gemcount;
	public GameObject camera;
	public GameObject spawnThis;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnThis () {
		camera.GetComponent<Spawner> ().friendlyMinion = spawnThis;
	}
}
