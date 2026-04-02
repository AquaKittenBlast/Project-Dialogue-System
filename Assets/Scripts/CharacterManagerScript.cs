using System.Collections;
using Mono.Cecil;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CharacterManagerScript : MonoBehaviour
{
    [SerializeField]private SpriteRenderer sr;
    [SerializeField]private Transform tr;
    [SerializeField]private float moveTime;
    [SerializeField]private float fadeTime;

    private Coroutine animateMoveCharacterCoroutine;
    private Coroutine fadeInCharacterCoroutine;
    private Vector2 targetPos;

    private float fadeTimer;

    public void Awake()
    {
        tr.position = new Vector2(0,tr.position.y);
    } 

    public void ChangeImage(Sprite sprite)
    {
        sr.sprite = sprite;
    }

    public void clearImage()
    {
        sr.sprite = null;
    }

    public void MoveImage(ScreenPosition pos, bool fadeIn)
    {
        float targetPosX = 0;
        switch (pos)
        {
            case ScreenPosition.Left: targetPosX = -4; break;
            case ScreenPosition.Middle: targetPosX = 0; break;
            case ScreenPosition.Right: targetPosX = 4; break;
        }
        targetPos = new Vector2(targetPosX, tr.position.y);
        
        if (fadeIn)
        {
            tr.position = targetPos;
            fadeInCharacterCoroutine = StartCoroutine(fadeInCharacter());
        }
        else
        {
            animateMoveCharacterCoroutine = StartCoroutine(animateMoveCharacter(targetPos));
        }
    }

    public void EndAnimationsEarly()
    {
        if (fadeInCharacterCoroutine != null &&  animateMoveCharacterCoroutine != null)
        {
            StopCoroutine(fadeInCharacterCoroutine);
            StopCoroutine(animateMoveCharacterCoroutine);
            tr.position = targetPos;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        }
    }

    private IEnumerator animateMoveCharacter(Vector2 targetPos)
    {
        float moveTimer = 0;
        Vector2 startPos = tr.position;

        while (moveTimer < moveTime)
        {
            moveTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(moveTimer / moveTime);
            float easedT = Mathf.SmoothStep(0f, 1f, t);
            Vector2 newPos = Vector2.Lerp(startPos, targetPos, easedT);
            tr.position = newPos;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator fadeInCharacter()
    {
        Color originalColor = sr.color;
        originalColor.a = 0;
        sr.color = originalColor;  
        fadeTimer = 0;
        while (fadeTimer < fadeTime)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 1, fadeTimer / fadeTime);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return new WaitForFixedUpdate();
        }
    }

    

}
