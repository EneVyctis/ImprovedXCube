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
    private void MiniMax(int Depth, Dictionary<int, Vector3Int> scores)
    {
        if (playableBlock.Count >= 40)
        {
            int play1 = playRandomBlock();
            int play2 = playRandomBlock();
            GameManager.Instance.CheckAndChangeBlock(playablePositions.GetValueOrDefault(play1));
            GameManager.Instance.CheckAndChangeBlock(playablePositions.GetValueOrDefault(play2));
        }
        else
        {
            foreach (KeyValuePair<int, Block> pair in playableBlock)
            {
                int play1;
                int play2;
                bool color = GameManager.Instance.color;
                if (pair.Value.hasColor == false)
                {
                    pair.Value.SetAIColor(color);
                    play1 = pair.Key;
                    foreach (KeyValuePair<int, Block> couple in playableBlock)
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
    }

    private bool CalculatesScores(int play1, int play2, Dictionary<int, Vector3Int> scores) 
    {
        Block block1 = playableBlock.GetValueOrDefault(play1);
        Block block2 = playableBlock.GetValueOrDefault(play2);
        //If one of the tested moves is a game ending moves, it gets a really high score of 500
        if (block1.CheckAIEndGame() == true || block2.CheckAIEndGame() == true)
        {
            scores.Add(500,new Vector3Int(play1, play2));
            return true;
        }
        if (block1.IsSquareAndAvailable())
        {
            //If the 2nd play end the game, it's still 500.
            if (block2.CheckAIEndGame())
            {
                scores.Add(scores.Count, new Vector3Int(500,play1, play2));
                return true;
            }
            //If we can fill 2 square instead of one, it's better, 400.
            if(block2.IsSquareAndAvailable())
            {
                scores.Add(scores.Count, new Vector3Int(400, play1, play2));
                return true;
            }
            //If block one can be fill, it's still fine 300.
            else
            {
                scores.Add(scores.Count, new Vector3Int(300,play1, play2));
                return true;
            }
        }
        //In the case of a side play1, if play2 is a square, its worth 200.
        if (block2.IsSquareAndAvailable())
        {
            scores.Add(scores.Count, new Vector3Int(200,play1, play2));
            return true;
        }

        //For now, any other moves worth 100.
        scores.Add(scores.Count, new Vector3Int(100,play1, play2));
        return false;
    }

    public bool RunAI()
    {
        Dictionary<int, Vector3Int> scores = new Dictionary<int, Vector3Int>();
        MiniMax(1, scores);
        int playScore = 0;
        foreach(KeyValuePair<int, Vector3Int> pair in scores)
        {
            if(pair.Value.x> scores.GetValueOrDefault(playScore).x)
            {
                playScore = pair.Value.x;
            }
        }
        GameManager.Instance.CheckAndChangeBlock(playablePositions.GetValueOrDefault(scores.GetValueOrDefault(playScore).y));
        GameManager.Instance.CheckAndChangeBlock(playablePositions.GetValueOrDefault(scores.GetValueOrDefault(playScore).z));
        return true;
    }


    /// <summary>
    /// Returns a random available block's number.
    /// </summary>
    /// <returns></returns>
    private int playRandomBlock()
    {
        int selected = Random.Range(0, playableBlock.Count);
        if(playableBlock.GetValueOrDefault(selected).hasColor == true)
        {
            return playRandomBlock();
        }

        else
        {
            return selected;
        }
    }

}
