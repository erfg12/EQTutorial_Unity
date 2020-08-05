using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trigger : MonoBehaviour
{
    public Text TextBox;
    public ScrollRect sr;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        // add text to text area
        TextBox.text += '\n' + "<color=white>Prumpy Irontoe says, 'Ho there stranger! You must be new to this land. Most folks you encounter will not be as talkative as I am, but if you Left Click on them with your mouse, and then press the H key, they will open up a bit. Try it out on me.'</color>" + '\n';

        // expand text box
        TextBox.rectTransform.sizeDelta = new Vector2(TextBox.rectTransform.sizeDelta.x, TextBox.rectTransform.sizeDelta.y + 80);

        // scroll to bottom
        //Canvas.ForceUpdateCanvases();
        //sr.verticalScrollbar.value = 0f;
        //Canvas.ForceUpdateCanvases();

        // hide message box
        gameObject.SetActive(false);
    }
}
