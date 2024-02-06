using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class AIManager : MonoBehaviour
{
    private Dictionary<int, Block> playableBlock = new Dictionary<int, Block>();
    private Dictionary<int, Vector2> playablePositions = new Dictionary<int, Vector2>();

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
    private void MiniMax(int Depth, Dictionary<int, Vector2Int> scores)
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
                    CalculatesScores(play1, play2, scores);
                    couple.Value.AIFactoryReset();
                }
                pair.Value.AIFactoryReset();
            }
        }
    }

    private bool CalculatesScores(int play1, int play2, Dictionary<int, Vector2Int> scores) 
    {
        Block block1 = playableBlock.GetValueOrDefault(play1);
        Block block2 = playableBlock.GetValueOrDefault(play2);
        //If one of the tested moves is a game ending moves, it gets a really high score of 500
        if (block1.CheckAIEndGame() == true || block2.CheckAIEndGame() == true)
        {
            scores.Add(500,new Vector2Int(play1, play2));
            return true;
        }
        
        if (block1.IsSquareAndAvailable())
        {
            //If the 2nd play end the game, it's still 500.
            if (block2.CheckAIEndGame())
            {
                scores.Add(500, new Vector2Int(play1, play2));
                return true;
            }
            //If we can fill 2 square instead of one, it's better, 400.
            if(block2.IsSquareAndAvailable())
            {
                scores.Add(400, new Vector2Int(play1, play2));
                return true;
            }
            //If block one can be fill, it's still fine 300.
            else
            {
                scores.Add(300, new Vector2Int(play1, play2));
                return true;
            }
        }
        //In the case of a side play1, if play2 is a square, its worth 200.
        if (block2.IsSquareAndAvailable())
        {
            scores.Add(200, new Vector2Int(play1, play2));
            return true;
        }

        //For now, any other moves worth 100.
        scores.Add(100, new Vector2Int(play1, play2));
        return false;
    }

    public void RunAI()
    {
        Dictionary<int, Vector2Int> scores = new Dictionary<int, Vector2Int>();
        MiniMax(1, scores);
        int playScore = 100;
        foreach(KeyValuePair<int, Vector2Int> pair in scores)
        {
            if(pair.Key> playScore)
            {
                playScore = pair.Key;
            }
        }
        GameManager.Instance.CheckAndChangeBlock(playablePositions.GetValueOrDefault(scores.GetValueOrDefault(playScore).x));
        GameManager.Instance.CheckAndChangeBlock(playablePositions.GetValueOrDefault(scores.GetValueOrDefault(playScore).y));
    }


}
