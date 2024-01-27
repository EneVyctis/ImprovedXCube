using UnityEngine;

public class Block : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    //Store all possible Sprites for this block
    [SerializeField] private Sprite lastBlue;
    [SerializeField] private Sprite lastRed;
    [SerializeField] private Sprite Blue;
    [SerializeField] private Sprite Red;



    public bool setSprite(bool color)
    {
        if(color)
        {
            spriteRenderer.sprite = lastBlue;

            return true;
        }
        if(!color) 
        {
            spriteRenderer.sprite = lastRed;
        }

        return false;
    }
}
