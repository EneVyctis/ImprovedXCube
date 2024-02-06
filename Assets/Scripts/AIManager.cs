using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private Dictionary<int, Block> playableBlock = new Dictionary<int, Block>();
    private Dictionary<int, Vector2> playablePositions = new Dictionary<int, Vector2>();
    private Dictionary<int, Vector2> scores = new Dictionary<int, Vector2>();

    void Start()
    {
        foreach(KeyValuePair<int,GameObject> pair in GameManager.Instance.sideList)
        {
            playableBlock.Add(pair.Key, pair.Value.GetComponent<Block>());
            playablePositions.Add(pair.Key, pair.Value.transform.position);
        }
        foreach (KeyValuePair<int, GameObject> pair in GameManager.Instance.squareList)
        {
            playableBlock.Add(pair.Key, pair.Value.GetComponent<Block>());
            playablePositions.Add(pair.Key, pair.Value.transform.position);
        }
    }

    /// <summary>
    /// Calculates each possible moove along with its score and store them in the "scores" dictionnary.
    /// </summary>
    /// <param name="Depth"></param>
    private void MiniMax(int Depth)
    {
        foreach(KeyValuePair<int,Block> pair in playableBlock)
        {
            int play1;
            int play2;
            bool color = GameManager.Instance.color;
            if(pair.Value.hasColor == false)
            {
                pair.Value.SetAIColor(color);
                play1 = pair.Key;
                foreach(KeyValuePair<int,Block> couple in playableBlock)
                {
                    couple.Value.SetAIColor(color);
                    play2 = couple.Key;
                }
            }
        }
    }

    private void CalculatesScores(int play1, int play2) 
    {
            
    }

    public void RunAI()
    {
        
    }


}
