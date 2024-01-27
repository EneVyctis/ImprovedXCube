using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    #region variables
    //Both dictionnaries store useful pieces of information for map's elements. The last parameter of the Vector3 is the rotation.
    private Dictionary<Int32,Vector3> squarePositions = new Dictionary<int, Vector3>();
    private Dictionary<Int32, Vector3> sidePositions = new Dictionary<int, Vector3>();
    [SerializeField] private GameObject square;
    [SerializeField] private GameObject side; 

    //For the whole game, true means blue player, false means red player
    private bool color = true;
    private int remainingActions = 2;
    #endregion

    void Start()
    {
        GeneratePositions();
        DrawMap();
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Debug.Log("Touched");
            Touch touch = Input.GetTouch(0);

            Vector2 screenPosition = touch.position;

            //Store the touch position in world space
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            CheckAndChangeBlock(touchPosition);
        }

        if(remainingActions == 0)
        {
            color = !color;
            remainingActions = 2;
        }
    }

    /// <summary>
    /// Generates all coordonates of squares and sides and store them in squarepositions and sidepositions
    /// </summary>
    private void GeneratePositions()
    {
        for (int i =  0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                squarePositions.Add(squarePositions.Count,new Vector3(-1.5f + j, 1.5f - i,0f));
            }
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                sidePositions.Add(sidePositions.Count, new Vector3(-2f + j, 1.5f - i, 90f));
                sidePositions.Add(sidePositions.Count, new Vector3(-1.5f + i, 2f - j, 0f));
            }
        }
    }

    /// <summary>
    /// Instatiate all gameObject based on squarePositions and sidePositions dictionnaries. 
    /// </summary>
    private void DrawMap()
    {
        foreach(KeyValuePair<int,Vector3> pair in squarePositions)
        {
            Instantiate(square, pair.Value, Quaternion.identity);
        }

        foreach(KeyValuePair <int,Vector3> pair in sidePositions)
        {
            Instantiate(side, pair.Value, Quaternion.Euler(new Vector3(0f,0f,pair.Value.z)));
        }
    }

    /// <summary>
    /// Attemps to play an action when the screen is touched.
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    private bool ChangeState(GameObject collider, bool color)
    {
        if(collider.TryGetComponent<Block>(out Block block))
        {
            if (block.setSprite(color))
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// Check if the touch was on a gameObject. If it does, try to play the moove and decrease the remaining actions.
    /// </summary>
    /// <param name="position"></param>
    public void CheckAndChangeBlock(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                remainingActions -= 1;
                if(ChangeState(hit.collider.gameObject, color))
                {
                    remainingActions -= 1;
                }
            }
        }
    }
}
