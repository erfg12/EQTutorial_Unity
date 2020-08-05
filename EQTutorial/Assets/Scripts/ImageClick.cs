using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageClick : MonoBehaviour, IPointerClickHandler
{
    public Text TextBox;
    public ScrollRect sr;
    public string MessageOnClose = "";
    public int LineHeight = 40;

    public void OnPointerClick(PointerEventData eventData)
    {
        // add text to text area
        TextBox.text += MessageOnClose + '\n';

        // expand text box
        TextBox.rectTransform.sizeDelta = new Vector2(TextBox.rectTransform.sizeDelta.x, TextBox.rectTransform.sizeDelta.y + LineHeight);

        // scroll to bottom
        Canvas.ForceUpdateCanvases();
        sr.verticalScrollbar.value = 0f;
        Canvas.ForceUpdateCanvases();

        // hide message box
        gameObject.SetActive(false);
    }
}
