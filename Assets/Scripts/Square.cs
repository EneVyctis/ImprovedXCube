using System;
using System.Collections.Generic;
using UnityEngine;

public class Square : Block
{
    //A square needs every of its sides to be colored before being available.
    private bool isAvailable = false;

    //Store the square's neighborhood
    private Dictionary<String, GameObject> squareNeighbor = new Dictionary<String, GameObject>();
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
            squareNeighbor.Add(key, square);
        }
    }

    public override bool setSprite(bool team)
    {
        if (team && (hasColor == false) && isAvailable)
        {
            changeColor(team, lastBlue);
            CheckEndGame();
            return true;
        }
        if (!team && (hasColor == false) && isAvailable)
        {
            changeColor(team, lastRed);
            CheckEndGame();
            return true;
        }

        return false;
    }

    private void changeColor(bool team, Sprite sprite)
    {
        blockColor = team;
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

    /// <summary>
    /// Is called whenever a bloc got a color, checks if something appends (victory/defeat).
    /// </summary>
    private void CheckEndGame()
    {
        CheckDirectLine("Up", "Down");
        CheckDirectLine("TopRight", "DownLeft");
        CheckDirectLine("TopLeft", "DownRight");
        CheckDirectLine("Right", "Left");
        foreach (KeyValuePair<String,GameObject> pair in squareNeighbor)
        {
            if(CheckNeighborColor(pair.Key, pair.Value, blockColor, 1))
            {
                GameManager.Instance.GameHasEnded(blockColor);
            }
        }
    }


    /// <summary>
    /// Return true if 3 squares with the same color are align and if the 3rd square is put at the extremity , else false.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="neighbor"></param>
    /// <param name="color"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool CheckNeighborColor(String key,GameObject neighbor, bool color, int count)
    {
        count++;

        //Just need to check if 3 aligns square has the same color, not more
        if(count > 3)
        {
            return true;
        }
        //Case of a border square
        if(neighbor == null)
        {
            return false;
        }

        if(neighbor.TryGetComponent<Square>(out Square square))
        {
            if (square.hasColor)
            {
                if(square.blockColor == color)
                {
                    return CheckNeighborColor(key, square.squareNeighbor.GetValueOrDefault(key), color,count);
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Covers the case when the 3rd square for the winning line is put in the middle of the line (the other function CheckNeighborColor only covers an extreme 3rd square).
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    private void CheckDirectLine(String key1, String key2)
    {
        //Mandatory cause some neighbors are null and so can't have a square component.
        if ((squareNeighbor.GetValueOrDefault(key1) != null) && (squareNeighbor.GetValueOrDefault(key2) != null))
        {
            //Mandatory condition cause square blocks are instatiate with blockColor value at false.
            if ((squareNeighbor.GetValueOrDefault(key1).GetComponent<Square>().hasColor) && (squareNeighbor.GetValueOrDefault(key2).GetComponent<Square>().hasColor == true))
            {
                if ((squareNeighbor.GetValueOrDefault(key1).GetComponent<Square>().blockColor == blockColor) && (squareNeighbor.GetValueOrDefault(key2).GetComponent<Square>().blockColor == blockColor))
                {
                    GameManager.Instance.GameHasEnded(blockColor);
                }
            }
        }
    }
}
