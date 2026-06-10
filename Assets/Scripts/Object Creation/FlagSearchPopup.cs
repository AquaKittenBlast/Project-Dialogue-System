using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlagSearchPopup : EditorWindow
{
    //Variables for the window
    private List<Flag> allFlags;
    private string searchText = "";
    private Vector2 scrollPos;

    //Event to trigger
    private Action<string> onFlagSelected;

    public static void ShowPopup(List<Flag> allFlags, Action<string> onFlagSelected)
    {
        FlagSearchPopup window = CreateInstance<FlagSearchPopup>();

        window.titleContent = new GUIContent("Flag Selection Window");
        window.allFlags = allFlags;
        window.onFlagSelected = onFlagSelected;

        window.position = new Rect(
            Screen.width / 1.5f,
            Screen.height / 3f,
            400,
            300);

        window.ShowUtility();
    }

    private void OnGUI()
    {
        GUILayout.Label("Flag Search", EditorStyles.boldLabel);
        searchText = EditorGUILayout.TextField(searchText);
        EditorGUILayout.Space();
        DrawFlagList();
    }


    private void DrawFlagList()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (GUILayout.Button("None"))
        {
            onFlagSelected?.Invoke("None");
            Close();
        }

        foreach (Flag flag in allFlags)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                if (!flag.flagName.ToLower().Contains(searchText.ToLower())){ continue; }
            }

            string buttonText = $"{flag.flagName} - Value: {ReturnsFormattedValue(flag.value)}";
            if (GUILayout.Button(buttonText))
            {
                onFlagSelected?.Invoke(flag.flagName);
                Close();
            }
        }
        EditorGUILayout.EndScrollView();
    }


    private string ReturnsFormattedValue(int value)
    {
        string output;
        switch (value)
        {
            case 0: output = "False"; break;
            case 1: output = "True"; break;
            default: output = $"{value}"; break;
        }
        return output;
    }
}