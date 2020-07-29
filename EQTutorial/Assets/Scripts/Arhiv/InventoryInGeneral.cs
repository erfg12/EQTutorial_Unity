using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryInGeneral : MonoBehaviour
{
  #region this code will give no good result for editor
  ///// <summary>
  ///// item for main hand (sword)
  ///// </summary>
  //public GameObject MainHand
  //{
  //  get { return mainHand; }
  //  set
  //  {
  //    if (mainHand != value)
  //    {
  //      //change mainhand item
  //      if (mainHand != null)
  //      {
  //        //free up main hand item
  //        //todo
  //      }
  //      mainHand = value;
  //      mainHand.transform.parent = transform.FindChild("R_POINT");
  //    }
  //  }
  //}
  //GameObject mainHand = null;
  #endregion

  /// <summary>
  /// item for main hand
  /// </summary>
  public string MainHand;
  string CurrentMainHand = null;

  /// <summary>
  /// item for offhand (shield)
  /// </summary>
  public string OffHand;
  string CurrentOffHand = null;

  /// <summary>
  /// this shows which material set we need, this is the short form of race and sex, like elm = elf male, elf = elf female, hum = human male
  /// </summary>
  public string Prefix;

  /// <summary>
  /// armor materials
  /// </summary>
  public enum VisibleInventoryMaterial { Cloth = 0, Leather = 1, Mail = 2, Plate = 3, }

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    Update(ref MainHand, ref CurrentMainHand, "R");
    Update(ref OffHand, ref CurrentOffHand, "L");
  }

  /// <summary>
  /// switch item if it is needed
  /// </summary>
  /// <param name="SetThis">the item to be set (like MainHand)</param>
  /// <param name="Current">the item that is currently equiped (like CurrentMainHand)</param>
  /// <param name="Hand">the code for hand "R", or "L"</param>
  void Update(ref string SetThis, ref string Current, string Hand)
  {
    if (SetThis != Current)
    {
      //change mainhand item
      if (!string.IsNullOrEmpty(Current))
      {
        //free up main hand item
        //todo
      }
      Current = SetThis;

      if (string.IsNullOrEmpty(Current)) return;

      GameObject NewHandHeldItem = Instantiate(Resources.Load("Items/" + Current)) as GameObject;

      Transform NewParent;
      Transform Root = transform;
      string LookFor = Hand + "_POINT";
      FindTransform(ref Root, ref LookFor, out NewParent);

      if (NewParent == null)
      {
        //other chance to find the finger
        LookFor = "FI_" + Hand;
        Root = transform;
        FindTransform(ref Root, ref LookFor, out NewParent);
      }

      NewHandHeldItem.transform.parent = NewParent;

      NewHandHeldItem.transform.localPosition = Vector3.zero;
      NewHandHeldItem.transform.rotation = new Quaternion(0, 270f / 512f / 360f, 90 / 512f / 360f, 1);
    }
  }

  /// <summary>
  /// find transform by name recursively
  /// </summary>
  /// <param name="Current">the current level</param>
  /// <param name="Name">name of the child</param>
  /// <param name="Result">the transform found</param>
  void FindTransform(ref Transform Current, ref string Name, out Transform Result)
  {
    Result = null;

    for (int i = 0; i < Current.childCount; i++)
    {
      //check through the children
      Transform Checked = Current.GetChild(i);
      if (string.Compare(Name, Checked.name, true) == 0)
      {
        //we got the child
        Result = Current.GetChild(i);
        return;
      }

      FindTransform(ref Checked, ref Name, out Result);
      if (Result != null) return;
    }
  }

  public List<string> debug2;
  public string de2;
  public int de3 = 0;
  public Transform[] debug;
}
