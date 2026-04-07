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
    public SpriteRenderer textBoxVisual;
    public SpriteRenderer nameBoxVisual;
    public ChoiceManagerScript choiceManager;
    public Slider slider;   

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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isChoosing){
                if (isTyping == true){
                    StopCoroutine(typeTextCoroutine);
                    characterManager.EndAnimationsEarly();
                    isTyping = false;
                    textBox.text = dialogueSentence.sentenceText;
                }
                else    {
                textBox.text = "";
                nameBox.text = "";
                NextSentence();
                }
            }
        }
        SetTextBoxOpacity(slider.value);
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
        nameBox.text = dialogueSentence.character.characterName;
        typeTextCoroutine = StartCoroutine(TypeText(dialogueSentence));
        //Shows the right expression and position
        foreach (ExpressionSprite sprite in dialogueSentence.character.sprites)
        {
            if (sprite.expression == dialogueSentence.expression){characterManager.ChangeImage(sprite.sprite); break;}
        }
        bool fadeIn = lastCharacter != dialogueSentence.character;
        characterManager.MoveImage(dialogueSentence.onScreenPosition, fadeIn);
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
        //Background stuff
        if (dialogueSentence.backgroundImage != null) {bgManager.ChangeBackground(dialogueSentence.backgroundImage);}
        if (dialogueSentence.backgroundMusic != null) {audioManager.ChangeMusic(dialogueSentence.backgroundMusic);}
        lastCharacter = dialogueSentence.character;
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
        if (currentDialogue == null){Debug.Log("No dialogue selected/provided."); return;}
        dialogueSentenceIndex++;
        if (dialogueSentenceIndex >= currentDialogue.sentences.Count)
        {
            EndDialogue();
            return;
        }
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


    public void SetTextBoxOpacity(float value)
    {
        nameBoxVisual.color = new Color(nameBoxVisual.color.r, nameBoxVisual.color.g, nameBoxVisual.color.b, value);
        textBoxVisual.color = new Color(textBoxVisual.color.r, textBoxVisual.color.g, textBoxVisual.color.b, value);
    }
}
