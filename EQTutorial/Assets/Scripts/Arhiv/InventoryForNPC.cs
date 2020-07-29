using UnityEngine;
using System.Collections;

/// <summary>
/// override of inventory lets you put different textures to different bodyparts (excluding face and helmet, what is a different story)
/// this is normally used by players and humanoid NPCs
/// you can use materials from cloth to plate by selecting VisibleInventoryMaterial
/// 
/// todo:
/// - ability to chose material color
/// - velious armor
/// - robes (probably inherited)
/// - helmet and face (probably inherited)
/// </summary>
public class InventoryForNPC : InventoryInGeneral
{
  #region these are the values to be set, and are visible to the other objects
  public VisibleInventoryMaterial Suit;
  #endregion

  #region these are the current and visible values
  private VisibleInventoryMaterial CurrentSuit = VisibleInventoryMaterial.Cloth;
  #endregion

  #region initialization
  // Use this for initialization
  void Start()
  {

  }
  #endregion

  #region core
  // Update is called once per frame
  void Update()
  {
    //compare material with a possible new
    if (CurrentSuit != Suit)
    {
      //wear the new suit
      Update("CH0?0#", Suit, 1, 1);
      Update("CH0?0#", Suit, 2, 2);
      Update("FA0?0#", Suit, 1, 1);
      Update("FT0?0#", Suit, 1, 2);
      Update("HN0?0#", Suit, 1, 2);
      Update("LG0?0#", Suit, 1, 3);
      Update("UA0?0#", Suit, 1, 1);

      //we surely set the new value, regardless of success in setup
      CurrentSuit = Suit;
    }
  }

  /// <summary>
  /// set (part) (from) (to) (SetThis) material
  /// </summary>
  /// <param name="Part">the armor set short form (like chest = ch, etc.), must come from the filename</param>
  /// <param name="SetThis">set this armor for sure</param>
  /// <param name="From">if (part) comes with multiple items (like leggings has 3 pieces) this is the first of (part)</param>
  /// <param name="To">see also (from), this is the last (part)</param>
  void Update(string Part, VisibleInventoryMaterial SetThis, int From, int To)
  {
    //we are to change armor
    Material Replaceable;
    for (int i = From; i <= To; i++)
    {
      //get and replace material
      Replaceable = (Material)Resources.Load(
        Prefix + (Part.Replace("?", "0").Replace("#", i.ToString()))
      , typeof(Material));
      if (Replaceable != null)
      {
        //material exists, changing texture
        //remark: checking existance is important, because not all materials exists: so elmhe0101 yes, elmhe0102 no, but elmhe0103 yes...
        Texture NewTexture = (Texture)(Resources.Load(
          Prefix + (Part.Replace("?", ((int)SetThis).ToString()).Replace("#", i.ToString()))
        , typeof(Texture)));
        Replaceable.mainTexture = NewTexture;
        Replaceable.color = Color.white;
      }
    }
  }
  #endregion
}
