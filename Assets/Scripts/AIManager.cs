using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private Dictionary<int, bool> sideColor = new Dictionary<int, bool>();
    private Dictionary<int, bool> squareColor = new Dictionary<int, bool>();
    private Dictionary<Vector2, int> scores = new Dictionary<Vector2, int>();

    void Start()
    {
        foreach(KeyValuePair<int,GameObject> pair in GameManager.Instance.sideList)
        {
            sideColor.Add(pair.Key, pair.Value);
        }
        foreach (KeyValuePair<int, GameObject> pair in GameManager.Instance.squareList)
        {
            squareColor.Add(pair.Key, pair.Value);
        }
    }

}
