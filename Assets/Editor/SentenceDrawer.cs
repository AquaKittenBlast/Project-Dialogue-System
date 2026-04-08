using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
[CustomPropertyDrawer(typeof(Sentence))]

public class SentenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        float yPos = position.y;

        var sentenceId = property.FindPropertyRelative("sentenceId");
        var character = property.FindPropertyRelative("character");
        var expression = property.FindPropertyRelative("expression");
        var onScreenPosition = property.FindPropertyRelative("onScreenPosition");
        var sentenceText = property.FindPropertyRelative("sentenceText");
        var backgroundImage = property.FindPropertyRelative("backgroundImage");
        var backgroundMusic = property.FindPropertyRelative("backgroundMusic");
        var isChoice = property.FindPropertyRelative("isChoice");
        var choices = property.FindPropertyRelative("choices");

        DrawField(ref yPos, position, sentenceId);
        DrawField(ref yPos, position, character);
        DrawField(ref yPos, position, expression);
        DrawField(ref yPos, position, onScreenPosition);
        DrawField(ref yPos, position, sentenceText);
        DrawField(ref yPos, position, backgroundImage);
        DrawField(ref yPos, position, backgroundMusic);
        DrawField(ref yPos, position, isChoice);
        if (isChoice.boolValue)
        {
            DrawChoicesField(ref yPos, position, choices);
        }

        EditorGUI.EndProperty();
    }


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
            // Multi-line text area
            prop.stringValue = EditorGUI.TextArea(rect, prop.stringValue);
        }
        else
        {
            EditorGUI.PropertyField(rect, prop);
        }

        yPos += fieldHeight + 4; 
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

        totalHeight += (lineHeight + spacing) * 4; 
        totalHeight += (lineHeight * 4.5f) + spacing;
        totalHeight += (lineHeight + spacing) * 3;

        var isChoice = property.FindPropertyRelative("isChoice");
        if (isChoice.boolValue)
        {
            var choices = property.FindPropertyRelative("choices");
            float choicesAmount = choices.arraySize;
            totalHeight += lineHeight + spacing * 12;
            totalHeight += choicesAmount * 1;
        }

        return totalHeight;
    }  
}
