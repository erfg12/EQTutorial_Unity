using UnityEngine;
using System.Collections;

public class LOC2UI : MonoBehaviour
{
  public Rect OutputHere = new Rect(0, 0, 500, 50);

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnGUI()
  {
    GUI.Label(OutputHere, string.Format("EQ LOC: X({0}) Y({1}) Z({2})\r\nUnity3d: X({3}) Y({2}) Z({1})"
      , -this.transform.position.x, this.transform.position.z, this.transform.position.y, this.transform.position.x));
  }
}
