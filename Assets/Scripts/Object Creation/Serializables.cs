using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    //Basic Information
    public string sentenceId;
    public Character character;
    public string speakerName;
    public string expression;
    public ScreenPosition onScreenPosition;
    public string sentenceText;
    //Advanced Information
    public string lineJumpId;
    public string shouldShowSentence = "None";
    public string flagLineJumpId;
    public string setsThisFlagToTrue = "None";
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

[System.Serializable]
public class Flag
{
    public string flagName;
    public int value;
}