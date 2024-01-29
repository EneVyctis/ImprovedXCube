using UnityEngine;

public class Block : MonoBehaviour
{
    public bool hasColor = false;
    public SpriteRenderer spriteRenderer;
    public bool blockColor;

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
        if (blockColor)
        {
            spriteRenderer.sprite = blue;
        }
        if(!blockColor)
        {
            spriteRenderer.sprite = red;
        }
    }

    /// <summary>
    /// Throw a ray to detect the presence of a gameobject. Returns it. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public GameObject Search(float x, float y)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + x, transform.position.y + y), Vector2.zero);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }
}
