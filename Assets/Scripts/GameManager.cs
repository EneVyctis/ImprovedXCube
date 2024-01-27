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
    #endregion

    void Start()
    {
        GeneratePositions();
        DrawMap();
    }

    void Update()
    {
        
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
}
