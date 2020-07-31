using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered water");
        other.GetComponent<PlayerMove>().UnderWater = true;
    }

    private void OnTriggerStay(Collider other)
    {
        other.GetComponent<PlayerMove>().UnderWater = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exited water");
        other.GetComponent<PlayerMove>().UnderWater = false;
    }
}
