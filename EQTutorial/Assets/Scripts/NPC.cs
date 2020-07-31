using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
	public string NPCName = "name";
	GameObject FloatingTextObj;
	GameObject SpawnedObj;
	public float height = 2;
	// Use this for initialization
	void Start () {
		FloatingTextObj = Resources.Load("FloatingText", typeof(GameObject)) as GameObject;
		SpawnedObj = Instantiate(Resources.Load("FloatingText", typeof(GameObject)) as GameObject, new Vector3(transform.position.x, transform.position.y + height, transform.position.z), Quaternion.identity);
		TextMesh txtMesh = SpawnedObj.GetComponent<TextMesh>();
		txtMesh.text = NPCName;
	}
	
	// Update is called once per frame
	void Update () {
		SpawnedObj.transform.rotation = GameObject.Find("Player_soandso").transform.rotation;
		//SpawnedObj.transform.LookAt(GameObject.Find("Player_soandso").transform.position); // makes text inverted
	}
}
