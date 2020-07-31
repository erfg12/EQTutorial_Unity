using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeadTeszt : MonoBehaviour
{

  public GameObject objPlayer;
  public GameObject objLimb;

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
    if (GUI.Button(new Rect(0, 200, 200, 50), "head teszt"))
    {
      SkinnedMeshRenderer[] meshDarabok = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
      foreach (SkinnedMeshRenderer meshDarab in meshDarabok)
      {
        Mesh mesh = meshDarab.sharedMesh;
        int index = -1;
        for (int i = 0; i < meshDarab.bones.Length; i++)
        {
          if (meshDarab.bones[i].gameObject == objPlayer)
          {
            index = i;
            break;
          }
        }
        if (index == -1)
        {
          Debug.LogError("Can not find such bone!");
        }
        else
        {
          Mesh újMesh = (Mesh)UnityEngine.Object.Instantiate(mesh);
          List<int> háromszögek = new List<int>(újMesh.triangles);
          Vector2[] uvMentés = újMesh.uv;
          Vector3[] normalMentés = újMesh.normals;
          for (int i = 0; i < mesh.boneWeights.Length; i++)
          {
            BoneWeight aktuálisSúly = mesh.boneWeights[i];
            if (aktuálisSúly.boneIndex0 == index)
            {
              //háromszögek.RemoveAll((int p) => { return p == i; });
            }
          }
          újMesh.triangles = háromszögek.ToArray();
          újMesh.uv = uvMentés;
          újMesh.normals = normalMentés;
          meshDarab.sharedMesh = újMesh;
        }
      }

      //objLimb.transform.position = Vector3.zero;
      //AddLimb(objLimb, objPlayer);
    }
  }

  void AddLimb(GameObject BonedObj, GameObject RootObj)
  {
    SkinnedMeshRenderer[] BonedObjects = BonedObj.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
    foreach (SkinnedMeshRenderer SkinnedRenderer in BonedObjects)
    {
      ProcessBonedObject(SkinnedRenderer, RootObj);
    }
  }

  GameObject ProcessBonedObject(SkinnedMeshRenderer ThisRenderer, GameObject RootObj)
  {
    GameObject NewObj = new GameObject(ThisRenderer.gameObject.name);
    NewObj.transform.parent = RootObj.transform;
    NewObj.AddComponent<SkinnedMeshRenderer>();
    var NewRenderer = NewObj.GetComponent<SkinnedMeshRenderer>();
    Transform[] MyBones = new Transform[ThisRenderer.bones.Length];
    for (int i = 0; i < ThisRenderer.bones.Length; i++)
    {
      MyBones[i] = FindChildByName(ThisRenderer.bones[i].name, RootObj.transform);
    }
    NewRenderer.bones = MyBones;
    NewRenderer.sharedMesh = ThisRenderer.sharedMesh;
    NewRenderer.materials = ThisRenderer.materials;
    return NewObj;
  }

  Transform FindChildByName(string ThisName, Transform ThisGObj)
  {
    if (ThisGObj.name == ThisName)
      return ThisGObj.transform;
    foreach (Transform child in ThisGObj)
    {
      Transform eredmény = FindChildByName(ThisName, child);
      if (eredmény != null) return eredmény;
    }
    return null;
  }
}
