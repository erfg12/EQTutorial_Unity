using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;

public class EQObjectImporter : EditorWindow
{
  [MenuItem("Window/EQ Object Importer")]
  static void ShowWindow()
  {
    EditorWindow.GetWindow(typeof(EQObjectImporter));
  }

  void OnGUI()
  {
    GUILayout.BeginHorizontal();

    if (GUILayout.Button("Load XML File"))
    {
      string path = EditorUtility.OpenFilePanel("Select an XML File", Application.dataPath, "xml");
      if (path.Length > 0)
      {
        using (XmlTextReader reader = new XmlTextReader(path))
        {
          string objectName;
          Vector3 pos = Vector3.zero;
          Vector3 rot = Vector3.zero;
          Vector3 scale = Vector3.zero;

          while (reader.Read())
          {
            if (reader.IsStartElement("Object"))
            {
              objectName = reader.GetAttribute("name");
              while (reader.Read())
              {
                if (reader.IsStartElement("Position"))
                {
                  reader.Read();
                  pos.y = float.Parse(reader.ReadString());
                  reader.Read();
                  pos.x = -float.Parse(reader.ReadString());
                  reader.Read();
                  pos.z = float.Parse(reader.ReadString());
                }
                else if (reader.IsStartElement("Rotation"))
                {
                  reader.Read();
                  rot.y = -float.Parse(reader.ReadString()) / 512F * 360F;
                  reader.Read();
                  rot.x = float.Parse(reader.ReadString()) / 512F * 360F;
                  reader.Read();
                  rot.z = float.Parse(reader.ReadString()) / 512F * 360F;
                }
                else if (reader.IsStartElement("Scale"))
                {
                  reader.Read();
                  scale.y = float.Parse(reader.ReadString());
                  reader.Read();
                  scale.z = float.Parse(reader.ReadString());
                  reader.Read();
                  scale.x = float.Parse(reader.ReadString());

                  // scale.x appears to always be 1, which is incorrect
                  scale.x = scale.z;
                  //pos *= 0.3f;

                  CreateObject(objectName, ref pos, ref rot, ref scale);
                  break;
                }
              }
            }
          }
        }
      }
    }

    GUILayout.EndHorizontal();
  }

  void CreateObject(string name, ref Vector3 pos, ref Vector3 rot, ref Vector3 scale)
  {
    GameObject prefab = null;
    bool dummy = false;
    if (prefab == null)
      prefab = Resources.Load("Objects/Prefab/" + name.Replace("_ACTORDEF", string.Empty)) as GameObject;
    if (prefab == null)
      prefab = Resources.Load("Objects/Prefab/" + name) as GameObject;
    if (prefab == null)
      prefab = Resources.Load("Objects/Static/" + name.Replace("_ACTORDEF", string.Empty)) as GameObject;
    if (prefab == null)
      prefab = Resources.Load("Objects/Static/" + name) as GameObject;

    if (prefab == null)
    {
      Debug.LogWarning(string.Format("Prefab {0} not found in 'objects/Prefab|Static...', using dummy!", name));
      prefab = Resources.Load("Objects/Dummy") as GameObject;
      dummy = true;
    }

    if (prefab != null)
    {
      GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
      go.transform.position = pos;
      go.transform.rotation = Quaternion.Euler(rot);
      go.transform.localScale = scale;
      go.transform.parent = (dummy ?
        GameObject.Find("Objects_Dummy") ?? new GameObject("Objects_Dummy")
        : GameObject.Find("Objects") ?? new GameObject("Objects")).transform;
      go.name = name.Replace("_ACTORDEF", string.Empty);

      if (string.Compare(Application.loadedLevelName, "North Qeynos", true) == 0)
      {
        if (string.Compare(go.name, "TEMPLELIFE", true) == 0)
        {
          go.transform.position = new Vector3(450, 38, -85);
          go.transform.localScale = new Vector3(3, 3, 3);
        } 
        else if (string.Compare(go.name, "TEMPLELIFEB", true) == 0)
        {
          go.transform.position = new Vector3(450, 10, -85);
          //go.transform.localScale = new Vector3(3, 3, 3);
        }
      }
    }
    else
    {
      Debug.LogError("Dummy prefab not found in 'objects/...', object can not be placed!");
    }
  }
}
