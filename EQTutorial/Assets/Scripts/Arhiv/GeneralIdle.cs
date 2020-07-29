using UnityEngine;
using System.Collections;

public class GeneralIdle : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (!GetComponent<Animation>().isPlaying)
    {
      //not animating
      //let's do another idle
      if (Random.Range(0, 100) < 80)
      {
        //mainly the normal idle anim
        GetComponent<Animation>().Play("Idle");
      }
      else
      {
        //rarely the "other one" (todo: sound for this (todo: there are actors with multiple idle sounds))
        if (GetComponent<Animation>().GetClip("Idle_01") != null) GetComponent<Animation>().Play("Idle_01");
        else GetComponent<Animation>().Play("Idle"); //if any
      }
    }
  }
}
