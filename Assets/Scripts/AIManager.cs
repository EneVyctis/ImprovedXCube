using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class AIManager : MonoBehaviour
{
    private Dictionary<int, Block> playableBlock = new Dictionary<int, Block>();
    private Dictionary<int, Vector2> playablePositions = new Dictionary<int, Vector2>();
    private int totalOfBlocks;

    void Start()
    {
        foreach(KeyValuePair<int,GameObject> pair in GameManager.Instance.sideList)
        {
            playableBlock.Add(pair.Key, pair.Value.GetComponent<Block>());
            playablePositions.Add(pair.Key, new Vector2(pair.Value.transform.position.x, pair.Value.transform.position.y));
        }
        foreach (KeyValuePair<int, GameObject> pair in GameManager.Instance.squareList)
        {
            playableBlock.Add(pair.Key, pair.Value.GetComponent<Block>());
            playablePositions.Add(pair.Key, new Vector2(pair.Value.transform.position.x, pair.Value.transform.position.y));
        }
        totalOfBlocks = playableBlock.Count;
    }

    /// <summary>
    /// Calculates each possible moove along with its score and store them in the "scores" dictionnary.
    /// </summary>
    /// <param name="Depth"></param>
    private void MiniMax(int Depth, Dictionary<int, Vector3Int> scores)
    {
        if (playableBlock.Count >= 60)
        {
            int play1 = playRandomBlock();
            int play2 = playRandomBlock();
            CalculatesScores(play1, play2, scores);
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
        Dictionary<int, GameObject> playsList = GameManager.Instance.playsList;
        playableBlock.Remove(playsList.GetValueOrDefault(playsList.Count - 2 ).GetComponent<Block>().key);
        playableBlock.Remove(playsList.GetValueOrDefault(playsList.Count - 1).GetComponent<Block>().key);
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
        GameManager.Instance.AICheckAndChangeBlock(playablePositions.GetValueOrDefault(scores.GetValueOrDefault(playScore).y));
        GameManager.Instance.AICheckAndChangeBlock(playablePositions.GetValueOrDefault(scores.GetValueOrDefault(playScore).z));
        return true;
    }


    /// <summary>
    /// Returns a random available block's number.
    /// </summary>
    /// <returns></returns>
    private int playRandomBlock()
    {
        int selected = Random.Range(0, totalOfBlocks);
        if(playableBlock.GetValueOrDefault(selected) == null || playableBlock.GetValueOrDefault(selected).hasColor == true || playableBlock.GetValueOrDefault(selected).IsSquareAndAvailable() == false)
        {
            return playRandomBlock();
        }

        else
        {
            return selected;
        }
    }

}
