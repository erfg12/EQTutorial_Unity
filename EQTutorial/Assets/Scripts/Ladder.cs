using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {
	private void OnTriggerEnter(Collider other)
    {
        Debug.Log("climbing ladder");
        other.GetComponent<PlayerMove>().ClimbingLadder = true;
    }

    private void OnTriggerStay(Collider other)
    {
        other.GetComponent<PlayerMove>().ClimbingLadder = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exited ladder");
        other.GetComponent<PlayerMove>().ClimbingLadder = false;
    }
}
