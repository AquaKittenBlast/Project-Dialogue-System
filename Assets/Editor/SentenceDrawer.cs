using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using log4net.Appender;
[CustomPropertyDrawer(typeof(Sentence))]

public class SentenceDrawer : PropertyDrawer
{
    #region Draws the entire sentence

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        float yPos = position.y;

        var sentenceId = property.FindPropertyRelative("sentenceId");
        var character = property.FindPropertyRelative("character");
        var speakerName = property.FindPropertyRelative("speakerName");
        var expression = property.FindPropertyRelative("expression");
        var onScreenPosition = property.FindPropertyRelative("onScreenPosition");
        var sentenceText = property.FindPropertyRelative("sentenceText");
        var lineJumpId = property.FindPropertyRelative("lineJumpId");
        var shouldShowSentence = property.FindPropertyRelative("shouldShowSentence");
        var flagLineJumpId = property.FindPropertyRelative("flagLineJumpId");
        var setsThisFlagToTrue = property.FindPropertyRelative("setsThisFlagToTrue");//flaglineset here
        var backgroundImage = property.FindPropertyRelative("backgroundImage");
        var backgroundMusic = property.FindPropertyRelative("backgroundMusic");
        var soundEffect = property.FindPropertyRelative("soundEffect");
        var isChoice = property.FindPropertyRelative("isChoice");
        var endsDialogue = property.FindPropertyRelative("endsDialogue");
        var choices = property.FindPropertyRelative("choices");

        DrawField(ref yPos, position, sentenceId);
        DrawField(ref yPos, position, character);
        DrawField(ref yPos, position, speakerName);
        if (character != null)
        {
            DrawExpressionDropdown(ref yPos, position, character, expression);
        }
        DrawField(ref yPos, position, onScreenPosition);
        DrawField(ref yPos, position, sentenceText);
        DrawField(ref yPos, position, lineJumpId);
        DrawFlagField(ref yPos, position, shouldShowSentence, "Should Show Sentence If");
        DrawField(ref yPos, position, flagLineJumpId);
        DrawFlagField(ref yPos, position, setsThisFlagToTrue, "Sets This Flag To True");
        DrawField(ref yPos, position, backgroundImage);
        DrawField(ref yPos, position, backgroundMusic);
        DrawField(ref yPos, position, soundEffect);
        DrawField(ref yPos, position, endsDialogue);
        DrawField(ref yPos, position, isChoice);
        if (isChoice.boolValue)
        {
            DrawChoicesField(ref yPos, position, choices);
        }

        EditorGUI.EndProperty();
    }
    #endregion

    #region Everything else


    private void DrawField(ref float yPos, Rect position, SerializedProperty prop)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float fieldHeight = lineHeight;

        if (prop.name == "sentenceText")
        {
            fieldHeight = lineHeight * 4.5f;
        }

        Rect rect = new Rect(position.x, yPos, position.width, fieldHeight);

        if (prop.name == "sentenceText")
        {
            prop.stringValue = EditorGUI.TextArea(rect, prop.stringValue);
        }
        else
        {
            EditorGUI.PropertyField(rect, prop);
        }

        yPos += fieldHeight + 4; 
    }

    private void DrawExpressionDropdown(ref float yPos, Rect position, SerializedProperty characterProp, SerializedProperty expressionProp)
    {
        Character characterObj = characterProp.objectReferenceValue as Character;

        string[] options = characterObj != null 
            ? characterObj.sprites.Select(s => s.expressionName).ToArray() 
            : new string[0];

        int currentIndex = Mathf.Max(0, System.Array.IndexOf(options, expressionProp.stringValue));

        int selectedIndex = EditorGUI.Popup(
            new Rect(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight),
            "Expression",
            currentIndex,
            options
        );

        if (options.Length > 0){expressionProp.stringValue = options[selectedIndex];}

        yPos += EditorGUIUtility.singleLineHeight + 4;
    }

    private void DrawFlagField(ref float yPos, Rect position, SerializedProperty shouldShowSentence, string labelName)
    {
        //sets the necessary variables for this script
        string currentValue = string.IsNullOrEmpty(shouldShowSentence.stringValue) ? "None" : shouldShowSentence.stringValue;
        Rect wholeRect = new Rect(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight);
        Rect fieldRect = EditorGUI.PrefixLabel(wholeRect, new GUIContent(labelName));

        //if you use a button in a script like this it returns trur if it has been clicked, and executes the code insidre
        if (EditorGUI.DropdownButton(fieldRect, new GUIContent(currentValue),FocusType.Passive)){ OpenFlagSearchPopup(shouldShowSentence); }
        yPos += EditorGUIUtility.singleLineHeight + 4;
    }


    private void OpenFlagSearchPopup(SerializedProperty target)
    {
        FlagDatabase db = AssetDatabase.LoadAssetAtPath<FlagDatabase>("Assets/Data/Databases/FlagDatabase.asset");
        if (db == null){return;}
        List<Flag> allFlags = db.allFlags;

        FlagSearchPopup.ShowPopup(db.allFlags, selectedFlag =>
        {
            target.stringValue = selectedFlag;
            target.serializedObject.ApplyModifiedProperties();
        });
    }

    private void DrawChoicesField(ref float yPos, Rect position, SerializedProperty prop)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;

        Rect rect = new Rect(position.x, yPos, position.width, lineHeight);
        EditorGUI.PropertyField(rect, prop);

        yPos += lineHeight + 4; 
    }

    //Ypos tells it where it goes, but the lineHeight variable gets used to tell the code how much space it should reserve for it
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = 4f;
        float totalHeight = 0f;

        totalHeight += (lineHeight + spacing) * 5; 
        totalHeight += (lineHeight * 4.5f) + spacing;
        totalHeight += (lineHeight + spacing) * 9;

        var isChoice = property.FindPropertyRelative("isChoice");
        if (isChoice.boolValue){
            var choices = property.FindPropertyRelative("choices");
            totalHeight += lineHeight * 4;
            float specificSpacing = 36f;
            for (int i = 0; i < choices.arraySize; i++)
            {
                totalHeight += EditorGUIUtility.singleLineHeight + specificSpacing;
            }
        }
        return totalHeight;
    }  

    
    #endregion
}
