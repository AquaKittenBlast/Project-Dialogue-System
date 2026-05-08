using TMPro;
using UnityEngine;

public class DialogueEntryScript : MonoBehaviour
{
    public string charName;
    public string dialogue;

    public void Start()
    {
        TextMeshProUGUI charNameText = transform.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI dialogueText = transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        charNameText.text = charName;
        dialogueText.text = dialogue;
    }
}
