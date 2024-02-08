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

    public virtual bool CheckAIEndGame()
    {
        return false;
    }

    public virtual bool setSprite(bool color)
    {
        if (color && (hasColor == false))
        {
            blockColor = true;
            spriteRenderer.sprite = lastBlue;
            hasColor = true;
            return true;
        }
        if (!color && (hasColor == false))
        {
            blockColor = false;
            spriteRenderer.sprite = lastRed;
            hasColor = true;
            return true;
        }

        return false;
    }

    public virtual bool IsSquareAndAvailable()
    {
        return true;
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

    #region AIfunctions
    /// <summary>
    /// Use by AI to simulates a play
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public virtual bool SetAIColor(bool color)
    {
        if (color && (hasColor == false))
        {
            blockColor = true;
            hasColor = true;
            return true;
        }
        if (!color && (hasColor == false))
        {
            blockColor = false;
            hasColor = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Undo changes due to simulations.
    /// </summary>
    /// <returns></returns>
    public bool AIFactoryReset()
    {
        blockColor = false;
        hasColor = false;

        return true;
    }
    #endregion

}
