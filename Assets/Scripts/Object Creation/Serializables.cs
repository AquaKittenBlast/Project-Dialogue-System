using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    public string sentenceId;
    public Character character;
    public string speakerName;
    public string expression;
    public ScreenPosition onScreenPosition;
    public string sentenceText;
    public string lineJumpId;
    public Sprite backgroundImage;
    public AudioClip backgroundMusic;
    public AudioClip soundEffect;
    public bool endsDialogue;
    public bool isChoice;
    public List<Choice> choices;
}

[System.Serializable]
public class ExpressionSprite
{
    public Sprite sprite;
    public string expressionName;
}

[System.Serializable]
public class Choice
{
    public string choiceText;
    public string lineJumpId;
}