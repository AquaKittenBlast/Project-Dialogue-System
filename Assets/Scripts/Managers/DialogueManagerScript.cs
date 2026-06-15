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

public class DialogueManagerScript : MonoBehaviour
{
    [Header("Options")]
    public float typingSpeed;
    public float dialogueBoxOpacity;

    [Header("Flags")]
    public bool isTyping = false;
    public bool isChoosing = false;

    [Header("External references")]
    public BackgroundManagerScript bgManager;
    public AudioManagerScript audioManager;
    public CharacterManagerScript characterManager;
    public TextMeshProUGUI textBox;
    public TextMeshProUGUI nameBox;
    public GameObject uiVisuals;
    public ChoiceManagerScript choiceManager;
    public FlagDatabase allFlags;

    [SerializeField]
    private List<Dialogue> allDialogues;
    private Dialogue currentDialogue;
    private int dialogueSentenceIndex;
    private Sentence dialogueSentence;
    private Coroutine typeTextCoroutine;
    private Character lastCharacter;

    public static DialogueManagerScript instance{ get; private set;}

    void Awake()
    {
        if (instance == null){instance = this;}
        uiVisuals.SetActive(false);
        //Testing
        StartDialogue(allDialogues[0]);
    }

    public void OnClickedMouseLeftButtonAsInTheThingOnTheLeftOfTheMouse(){
        if (!isChoosing){
            if (isTyping == true){
                StopCoroutine(typeTextCoroutine);
                characterManager.EndAnimationsEarly();
                isTyping = false;
                textBox.text = dialogueSentence.sentenceText;
            }
            else{
                textBox.text = "";
                nameBox.text = "";
                NextSentence();
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null){Debug.Log("Provided dialogue was null"); return;}
        textBox.text = "";
        nameBox.text = "";
        currentDialogue = dialogue;
        dialogueSentenceIndex = 0;
        uiVisuals.SetActive(true);
        ShowLine();
    }

    void ShowLine()
    {
        //Gets the right sentence from the dialogue
        dialogueSentence = currentDialogue.sentences[dialogueSentenceIndex];
        //Shows the text
        if (dialogueSentence.speakerName != ""){nameBox.text = dialogueSentence.speakerName;}
        else if (dialogueSentence.character){nameBox.text = dialogueSentence.character.name;}
        else {nameBox.text = lastCharacter.name;}
        typeTextCoroutine = StartCoroutine(TypeText(dialogueSentence));
        //Shows the right expression and position
        if (dialogueSentence.character)
        {
            foreach (ExpressionSprite sprite in dialogueSentence.character.sprites)
            {
                if (sprite.expressionName == dialogueSentence.expression){characterManager.ChangeImage(sprite.sprite); break;}
            }
        }
        //Shows the correct position
        bool fadeIn = false;
        if (dialogueSentence.character)
        {
            fadeIn = lastCharacter != dialogueSentence.character;
        }
        characterManager.MoveImage(dialogueSentence.onScreenPosition, fadeIn);
        string historySpeakerName;
        //History logic
        if (dialogueSentence.speakerName != ""){historySpeakerName = dialogueSentence.speakerName;}
        else if (dialogueSentence.character){historySpeakerName = dialogueSentence.character.characterName;}
        else {historySpeakerName = lastCharacter.name;}
        HistoryManagerScript.instance.CreateNewDialogueEntry(historySpeakerName, dialogueSentence.sentenceText);
        //Choice logic
        if (dialogueSentence.isChoice)
        {
            isChoosing = true;
            if (dialogueSentence.choices.Count == 0){
                Debug.Log("Ischoice is true yet no choices exist"); 
                StopCoroutine(typeTextCoroutine);
                EndDialogue(); 
                return;
            }
            choiceManager.InstantiateChoices(dialogueSentence.choices);
        }
        //Flag setting logic
        if (dialogueSentence.setsThisFlagToTrue != "None")
        {
            Flag foundFlag = allFlags.allFlags.Find(s => s.flagName == dialogueSentence.setsThisFlagToTrue);
            if (foundFlag != null){foundFlag.value++;} else {Debug.Log($"There is no flag with the name {dialogueSentence.setsThisFlagToTrue}");}
            
            Debug.Log(foundFlag.value);
        }

        //Background & audio
        if (dialogueSentence.backgroundImage) {bgManager.ChangeBackground(dialogueSentence.backgroundImage);}
        if (dialogueSentence.backgroundMusic) {audioManager.ChangeMusic(dialogueSentence.backgroundMusic);}
        if (dialogueSentence.soundEffect){AudioManagerScript.instance.PlaySFX(dialogueSentence.soundEffect);}
        if (dialogueSentence.character){lastCharacter = dialogueSentence.character;}
    }

    void EndDialogue()
    {
        currentDialogue = null;
        uiVisuals.SetActive(false);
        characterManager.clearImage();
        textBox.text = "";
        nameBox.text = "";
        lastCharacter = null;
    }

    void NextSentence()
    {
        //basic nullguard & dialogue ender
        if (currentDialogue == null){Debug.Log("No dialogue selected/provided."); return;}
        if (dialogueSentence.endsDialogue){EndDialogue(); return;}
        //Linejump Id logic
        else if (dialogueSentence.lineJumpId != ""){
            int foundSentenceIndex = currentDialogue.sentences.FindIndex(s => s.sentenceId == dialogueSentence.lineJumpId);
            if (foundSentenceIndex == -1)
            {
                Debug.Log($"No sentence found with id {dialogueSentence.lineJumpId}");
            }
            else {dialogueSentenceIndex = foundSentenceIndex;}
        }
        else{dialogueSentenceIndex++;}
        //Checks if the current sentence is allowed to be displayed due to flags
        bool invalidSentence = true;
        do
        {
            if (dialogueSentenceIndex >= currentDialogue.sentences.Count)
            {
                EndDialogue();
                return;
            }  

            Sentence currentSentence = currentDialogue.sentences[dialogueSentenceIndex];
            if (FlagNameToBool(currentSentence.shouldShowSentence) || currentSentence.shouldShowSentence == "None")
            {
                invalidSentence = false;
            }
            else {
                if (currentSentence.flagLineJumpId != "")
                {
                    int foundSentenceIndex = currentDialogue.sentences.FindIndex(s => s.sentenceId == currentSentence.flagLineJumpId);
                    if (foundSentenceIndex == -1)
                    {
                        Debug.Log($"No sentence found with id {currentSentence.flagLineJumpId}");
                    }
                    else {dialogueSentenceIndex = foundSentenceIndex;}
                    
                }
                else{dialogueSentenceIndex++;}
            }    
        }
        while (invalidSentence);
        ShowLine();
    }

    #region Helper functions
    public void SkipToLineForChoice(string idToJumpTo)
    {
        textBox.text = "";
        nameBox.text = "";
        dialogueSentenceIndex = currentDialogue.sentences.FindIndex(s => s.sentenceId == idToJumpTo);
        if (dialogueSentenceIndex >= currentDialogue.sentences.Count)
        {
            EndDialogue();
            return;
        }
        isChoosing = false;
        ShowLine();
    }

    private IEnumerator TypeText(Sentence sentence)
    {
        string text = sentence.sentenceText;
        isTyping = true;

        foreach (char c in text)
        {
            textBox.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        isTyping = false;
    }

    public bool FlagNameToBool(string name)
    {
        foreach (Flag f in allFlags.allFlags)
        {
            if (f.flagName == name)
            {
                switch (f.value)
                {
                    case 0: return false;
                    case 1: return true;
                    default: Debug.Log("You used a numbered variable instead of a number, dumbass"); return true;
                }
            }
        }
        return false;
    }
    #endregion

}
