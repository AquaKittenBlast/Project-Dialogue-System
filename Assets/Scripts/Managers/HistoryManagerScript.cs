using UnityEngine;

public class HistoryManagerScript : MonoBehaviour
{
    public static HistoryManagerScript instance;

    [Header("External Refs")]
    public GameObject historyUI;
    public GameObject dialogueEntry;
    public GameObject entryContainer;

    public void Awake()
    {
        if(instance == null){instance = this;}
    }

    public void FlipHistoryActive()
    {
        historyUI.SetActive(!historyUI.activeSelf);
        if (historyUI.activeSelf)
        {
            SettingsManagerScript.instance.settingsUI.SetActive(false);
        }
    }

    public void CreateNewDialogueEntry(string charName, string dialogText)
    {
        GameObject newEntry = Instantiate(dialogueEntry, entryContainer.transform, false);
        DialogueEntryScript des = newEntry.GetComponent<DialogueEntryScript>();
        des.charName = charName;
        des.dialogue = dialogText;
    }

    
}
