using UnityEngine;

public class BackgroundManagerScript : MonoBehaviour
{
    public SpriteRenderer sr;

    public void ChangeBackground(Sprite sprite)
    {
        sr.sprite = sprite;
    }
}
