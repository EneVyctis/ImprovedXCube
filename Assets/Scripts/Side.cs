using System;
using System.Collections.Generic;
using UnityEngine;

public class Side : Block
{
    #region variables
    private bool isVertical;

    private Dictionary<String, GameObject> squareNeighbor = new Dictionary<String, GameObject>();
    #endregion

    private void Awake()
    {
        //A side block can either be vertical or horizontal. 
        if(gameObject.transform.eulerAngles.z != 0)
        {
            isVertical = true;
        }
        else
        {
            isVertical = false;
        }
    }

    public override bool setSprite(bool team)
    {
        if (team && (hasColor == false))
        {
            ChangeColor(team, lastBlue);
            return true;
        }
        if (!team && (hasColor == false))
        {
            ChangeColor(team, lastRed);

            return true;
        }

        return false;
    }

    private void ChangeColor(bool team, Sprite sprite )
    {
        blockColor = team;
        spriteRenderer.sprite = sprite;
        hasColor = true;
        foreach (KeyValuePair<String, GameObject> pair in squareNeighbor)
        {
            pair.Value.GetComponent<Square>().CheckAvailability();
        }
    }

    /// <summary>
    /// Searches and stores its neighbors
    /// </summary>
    public void CalculateNeighborhood()
    {
        if (isVertical == true)
        {
            AddSideNeighbors(0.5f, 0f, "Right");
            AddSideNeighbors(-0.5f, 0f, "Left");
        }

        if (isVertical == false) 
        {
            AddSideNeighbors(0f, 0.5f, "Up");
            AddSideNeighbors(0f, -0.5f, "Down");
        }
    }

    /// <summary>
    /// Check the presence of a gameobject using search and, if it exists, add it to the neighbors' list.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="key"></param>
    private void AddSideNeighbors(float x, float y, String key)
    {
        GameObject side = Search(x, y);
        if (side != null)
        {
            squareNeighbor.Add(key, side);
        }
    }

    /// <summary>
    /// Same as SetColor but for simulations.
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    #region AI functions
    public override bool SetAIColor(bool team)
    {
        if (team && (hasColor == false))
        {
            blockColor = team;
            hasColor = true;
            foreach (KeyValuePair<String, GameObject> pair in squareNeighbor)
            {
                pair.Value.GetComponent<Square>().CheckAvailability();
            }
            return true;
        }
        if (!team && (hasColor == false))
        {
            blockColor = team;
            hasColor = true;
            foreach (KeyValuePair<String, GameObject> pair in squareNeighbor)
            {
                pair.Value.GetComponent<Square>().CheckAvailability();
            }
            return true;
        }

        return false;
    }

    /// <summary>
    /// Undo changes due to simulations.
    /// </summary>
    /// <returns></returns>
    public override bool AIFactoryReset()
    {
        blockColor = false;
        hasColor = false;
        foreach (KeyValuePair<String, GameObject> pair in squareNeighbor)
        {
            pair.Value.GetComponent<Square>().CheckAvailability();
        }
        return true;
    }
    #endregion
}
