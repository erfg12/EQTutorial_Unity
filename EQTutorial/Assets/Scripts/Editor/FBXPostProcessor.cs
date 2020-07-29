#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Globalization;

public class FBXPostProcessor : AssetPostprocessor
{
  float scale = 0.3f;
  bool IsEqModel
  {
    get
    {
      if (assetPath.ToUpper().EndsWith(".FBX"))
      {
        scale = 1f;
        return true;
      }
      else
      {
        scale = 0.01f;
        return false;
      }
    }
  }

  void OnPreprocessModel()
  {
    if (IsEqModel)
    {
      ModelImporter importer = assetImporter as ModelImporter;
      if (assetPath.Contains("/Resources/Objects/") || assetPath.ToUpper().Contains("EQUIPMENT"))
      {
        importer.animationCompression = UnityEditor.ModelImporterAnimationCompression.Off;
        importer.animationType = UnityEditor.ModelImporterAnimationType.None;
      }
      importer.globalScale = scale;
      importer.swapUVChannels = false;
      importer.materialSearch = ModelImporterMaterialSearch.Everywhere;
    }
  }

  void OnPostprocessModel(GameObject go)
  {
    if (IsEqModel)
    {
      if (go.GetComponent<Animation>() != null && go.GetComponent<Animation>()["Idle"])
        go.GetComponent<Animation>().clip = go.GetComponent<Animation>()["Idle"].clip;
    }
  }
}
#endif