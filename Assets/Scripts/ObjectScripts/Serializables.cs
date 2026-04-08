using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    public string sentenceId;
    public Character character;
    public string expression;
    public ScreenPosition onScreenPosition;
    public string sentenceText;
    public string lineJumpId;
    public Sprite backgroundImage;
    public AudioClip backgroundMusic;
    public bool isChoice;
    public List<string> choices;
}

[System.Serializable]
public class ExpressionSprite
{
    public Sprite sprite;
    public string expressionName;
}