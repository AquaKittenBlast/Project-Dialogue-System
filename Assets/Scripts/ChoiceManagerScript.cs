using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManagerScript : MonoBehaviour
{
    public GameObject choiceBar;
    private List<GameObject> choiceBars = new List<GameObject>();

    public static ChoiceManagerScript instance{get; private set;}

    public void Awake()
    {
        if (instance == null) {instance = this;}
    }

    public void InstantiateChoices(List<string> choices)
    {
        //Actual Creation
        foreach (Transform child in transform){Destroy(child.gameObject);};
        foreach (string x in choices)
        {
            GameObject newChoiceBar = Instantiate(choiceBar, transform, false);
            ChoiceScript choiceScript = newChoiceBar.GetComponent<ChoiceScript>();
            choiceScript.choiceText = x;
            choiceBars.Add(newChoiceBar);
        }
        //Positioning
        float yAdd = 9f;
        foreach(GameObject choiceBar in choiceBars)
        {
            RectTransform rt = choiceBar.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, yAdd);
            yAdd -= 103f;
        } 
    }



    public void PolPot()
    {
        foreach (Transform child in transform){Destroy(child.gameObject);};
    }

}
