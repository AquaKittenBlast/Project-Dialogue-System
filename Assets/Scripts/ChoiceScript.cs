using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceScript : MonoBehaviour
{
    public string choiceText;

    public void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = choiceText;
    }

    private void OnClick()
    {
        ChoiceManagerScript.instance.PolPot();
        Debug.Log(choiceText);
    }
}
