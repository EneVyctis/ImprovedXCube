using UnityEngine;

public class Block : MonoBehaviour
{
    public bool hasColor = false;
    public SpriteRenderer spriteRenderer;
    public bool color;

    //Store all possible Sprites for this block
    [SerializeField] public Sprite lastBlue;
    [SerializeField] public Sprite lastRed;
    [SerializeField] private Sprite blue;
    [SerializeField] private Sprite red;



    public virtual bool setSprite(bool color)
    {
        if (color && (hasColor == false))
        {
            color = true;
            spriteRenderer.sprite = lastBlue;
            hasColor = true;
            return true;
        }
        if (!color && (hasColor == false))
        {
            color = false;
            spriteRenderer.sprite = lastRed;
            hasColor = true;
            return true;
        }

        return false;
    }

    public void setDefinitiveSprite()
    {
        if (color)
        {
            spriteRenderer.sprite = blue;
        }
        if(!color)
        {
            spriteRenderer.sprite = red;
        }
    }
}
