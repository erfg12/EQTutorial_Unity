using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScroll : MonoBehaviour
{
    public ScrollRect sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        Canvas.ForceUpdateCanvases();
        sr.verticalScrollbar.value = 0f;
        Canvas.ForceUpdateCanvases();
    }
}
