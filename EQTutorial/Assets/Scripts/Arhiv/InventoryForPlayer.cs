using UnityEngine;
using System.Collections;
using System.Text;

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
public class InventoryForPlayer : InventoryInGeneral
{
  #region these are the values to be set, and are visible to the other objects
  public VisibleInventoryMaterial Chest;
  public VisibleInventoryMaterial Bracers;
  public VisibleInventoryMaterial Feet;
  public VisibleInventoryMaterial Gloves;
  public VisibleInventoryMaterial Leggings;
  public VisibleInventoryMaterial Arms;
  public VisibleInventoryMaterial Collar;
  #endregion

  #region these are the current and visible values
  private VisibleInventoryMaterial CurrentChest = VisibleInventoryMaterial.Cloth;
  private VisibleInventoryMaterial CurrentBracers = VisibleInventoryMaterial.Cloth;
  private VisibleInventoryMaterial CurrentFeet = VisibleInventoryMaterial.Cloth;
  private VisibleInventoryMaterial CurrentGloves = VisibleInventoryMaterial.Cloth;
  private VisibleInventoryMaterial CurrentLeggings = VisibleInventoryMaterial.Cloth;
  private VisibleInventoryMaterial CurrentArms = VisibleInventoryMaterial.Cloth;
  private VisibleInventoryMaterial CurrentCollar = VisibleInventoryMaterial.Cloth;
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
    //we check all the parts of visible armor and switch to new value if needed
    Update("CH0?0#", ref CurrentChest, ref Chest, 1, 1);
    Update("CH0?0#", ref CurrentCollar, ref Collar, 2, 2);
    Update("FA0?0#", ref CurrentBracers, ref Bracers, 1, 1);
    Update("FT0?0#", ref CurrentFeet, ref Feet, 1, 2);
    Update("HN0?0#", ref CurrentGloves, ref Gloves, 1, 2);
    Update("LG0?0#", ref CurrentLeggings, ref Leggings, 1, 3);
    Update("UA0?0#", ref CurrentArms, ref Arms, 1, 1);
  }

  /// <summary>
  /// we check a piece of visible armor (part), compare the current value (current) to the new value (comparedto), if different we switch armor
  /// we keep comparing (from) (to) until all items in this (part) are done
  /// </summary>
  /// <param name="Part">the armor set short form (like chest = ch, etc.), must come from the filename</param>
  /// <param name="Current">the current value, currently visible</param>
  /// <param name="ComparedTo">the new value, the one we set by other objects</param>
  /// <param name="From">if (part) comes with multiple items (like leggings has 3 pieces) this is the first of (part)</param>
  /// <param name="To">see also (from), this is the last (part)</param>
  void Update(string Part, ref VisibleInventoryMaterial Current, ref VisibleInventoryMaterial ComparedTo, int From, int To)
  {
    if (Current != ComparedTo)
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
            Prefix + (Part.Replace("?", ((int)ComparedTo).ToString()).Replace("#", i.ToString()))
          , typeof(Texture)));
          Replaceable.mainTexture = NewTexture;
          Replaceable.color = Color.white;
        }
      }

      //even if we find no material, we set the new value, so we do not look again for nothing..
      Current = ComparedTo;
    }
  }
  #endregion
}
