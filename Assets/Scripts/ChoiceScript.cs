using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceScript : MonoBehaviour
{
    public Choice choice;

    public void Start()
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = choice.choiceText;
    }

    public void OnClick()
    {
        Debug.Log("clanker");
        ChoiceManagerScript.instance.PolPot();
        DialogueManagerScript.instance.StartDialogue(choice.resultingDialogue);
    }
}
