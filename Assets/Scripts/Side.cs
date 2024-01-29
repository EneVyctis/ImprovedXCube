using System;
using System.Collections.Generic;
using UnityEngine;

public class Side : Block
{
    
    private bool isVertical;

    private Dictionary<String, GameObject> squareNeighbor = new Dictionary<String, GameObject>();

    private void Awake()
    {
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
            changeColor(team, lastBlue);
            return true;
        }
        if (!team && (hasColor == false))
        {
            changeColor(team, lastRed);

            return true;
        }

        return false;
    }

    private void changeColor(bool team, Sprite sprite )
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
}
