using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    public Character character;
    public ExpressionType expression;
    public ScreenPosition onScreenPosition;
    [TextArea(5, 10)]
    public string sentenceText;
    public Sprite backgroundImage;
    public AudioClip backgroundMusic;
    public bool isChoice;
    public List<Choice> choices;
}

[System.Serializable]
public class ExpressionSprite
{
    public Sprite sprite;
    public ExpressionType expression;
}

[System.Serializable]
public class Choice
{
    public string choiceText;
    public Dialogue resultingDialogue;
}
