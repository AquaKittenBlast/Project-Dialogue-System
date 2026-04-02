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

    public void InstantiateChoices(List<Choice> choices)
    {
        //Actual Creation
        foreach (Transform child in transform){Destroy(child.gameObject);};
        foreach (Choice x in choices)
        {
            GameObject newChoiceBar = Instantiate(choiceBar, transform, false);
            ChoiceScript choiceScript = newChoiceBar.GetComponent<ChoiceScript>();
            choiceScript.choice = x;
            choiceBars.Add(newChoiceBar);
        }
        //Positioning
        float yAddendum = 207f;
        foreach(GameObject choiceBar in choiceBars)
        {
            RectTransform rt = choiceBar.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, yAddendum);
            yAddendum -= 103f;
        } 
    }



    public void PolPot()
    {
        foreach (Transform child in transform){Destroy(child.gameObject);};
    }

}
