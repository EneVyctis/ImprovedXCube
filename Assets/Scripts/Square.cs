using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : Block
{
    //A square needs every of its sides to be colored before being available.
    private bool isAvailable = false;

    //Store the square's neighborhood
    private Dictionary<String, GameObject> SquareNeighbor = new Dictionary<String, GameObject>();
    private Dictionary<String, GameObject> sides = new Dictionary<String, GameObject>();
    private void Update()
    {
            
    }

    /// <summary>
    /// Searches and stores its neighbors
    /// </summary>
    public void CalculateNeighborhood()
    {
        //Each square has exactly 4 sides. No exception. 
        sides.Add("Up", Search(0f, 0.5f));
        sides.Add("Down", Search(0f, -0.5f));
        sides.Add("Right", Search(0.5f, 0f));
        sides.Add("Left", Search(-0.5f, 0f));

        //Each square has up to 8 neighbors (square). Minimum is 3.
        AddSquareNeighbors(0f, 1f, "Up");
        AddSquareNeighbors(0f, -1f, "Down");
        AddSquareNeighbors(1f, 0f, "Right");
        AddSquareNeighbors(-1f, 0f, "Left");
        AddSquareNeighbors(1f, 1f, "TopRight");
        AddSquareNeighbors(-1f, 1f, "TopLeft");
        AddSquareNeighbors(1f, -1f, "DownRight");
        AddSquareNeighbors(-1f, -1f, "DownLeft");
    }

    /// <summary>
    /// Throw a ray to detect the presence of a gameobject. Returns it. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private GameObject Search(float x, float y)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + x, transform.position.y + y), Vector2.zero);
        if(hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Check the presence of a gameobject using search and, if it exists, add it to the neightbors' list.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="key"></param>
    private void AddSquareNeighbors(float x, float y, String key)
    {
        GameObject square = Search(x,y);
        if (square != null)
        {
            SquareNeighbor.Add(key, square);
        }
    }

    public override bool setSprite(bool team)
    {
        if (team && (hasColor == false) && isAvailable)
        {
            changeColor(team, lastBlue);
            return true;
        }
        if (!team && (hasColor == false) && isAvailable)
        {
            changeColor(team, lastRed);
            return true;
        }

        return false;
    }

    private void changeColor(bool team, Sprite sprite)
    {
        color = team;
        spriteRenderer.sprite = sprite;
        hasColor = true;
    }

    public void CheckAvailability()
    {
        int count = 0;
        foreach(KeyValuePair<string,GameObject> pair in sides)
        {
            if (pair.Value.GetComponent<Block>().hasColor)
            {
                count++;
            }
        }
        if (count == 4)
        {
            isAvailable = true;
        }
    }
}
