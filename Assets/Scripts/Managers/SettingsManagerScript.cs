using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.UI;


public class SettingsManagerScript : MonoBehaviour
{
    public static SettingsManagerScript instance;

    [Header("External References")]
    public GameObject settingsUI;
    public Slider opacitySlider;
    public Slider textSpeedSlider;
    public Image textBoxVisual;
    public Image nameBoxVisual;

    public void Awake()
    {
        if (instance == null){instance = this;}
        settingsUI.SetActive(false);
    }

    public void Start()
    {
        SetTextBoxOpacity(opacitySlider.value);
        SetTextSpeedVariable(textSpeedSlider.value);
    }

    public void SetTextBoxOpacity(float value)
    {
        nameBoxVisual.color = new Color(nameBoxVisual.color.r, nameBoxVisual.color.g, nameBoxVisual.color.b, value);
        textBoxVisual.color = new Color(textBoxVisual.color.r, textBoxVisual.color.g, textBoxVisual.color.b, value);
    }

    public void SetTextSpeedVariable(float value)
    {
        float actualSpeed = 0.1f - value + 0.01f;
        DialogueManagerScript.instance.typingSpeed = actualSpeed;
    }

    public void FlipSettingsActive()
    {
        settingsUI.SetActive(!settingsUI.activeSelf);
        if (settingsUI.activeSelf)
        {
            HistoryManagerScript.instance.historyUI.SetActive(false);
        }
    }

}
