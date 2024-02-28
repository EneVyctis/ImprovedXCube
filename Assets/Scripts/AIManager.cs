using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private Dictionary<int, Block> playableBlock = new Dictionary<int, Block>();
    private Dictionary<int, Vector2> playablePositions = new Dictionary<int, Vector2>();

    void Start()
    {
        foreach(KeyValuePair<int,GameObject> pair in GameManager.Instance.blocksList)
        {
            playableBlock.Add(pair.Key, pair.Value.GetComponent<Block>());
            playablePositions.Add(pair.Key, new Vector2(pair.Value.transform.position.x, pair.Value.transform.position.y));
        }
    }

    /// <summary>
    /// Manage the MiniMax algorithm.
    /// </summary>
    /// <param name="Depth"></param> 
    /// <param name="color"></param>
    /// <param name="scores"></param>
    private IEnumerator MiniMax(int depth, bool color, Dictionary<int, Vector3Int> scores)
    {
        foreach (KeyValuePair<int, Block> pair in playableBlock)
        {
            int play1;
            int play2;
            if (pair.Value.hasColor == false && pair.Value.IsAvailable() == true)
            {
                play1 = pair.Key;
                pair.Value.SetAIColor(color);
                foreach (KeyValuePair<int, Block> couple in playableBlock)
                {   
                    if (couple.Value.hasColor == false && couple.Value.IsAvailable() == true)
                    {
                        int score = 0;
                        play2 = couple.Key;
                        couple.Value.SetAIColor(color);
                        score += CalculatesScores(play1, play2, color);
                        for(int k=1; k<depth; k++)
                        {
                            color = !color;
                            yield return StartCoroutine(AiTurn(score, color));
                        }
                        scores.Add(scores.Count, new Vector3Int(score, play1, play2));
                        couple.Value.AIFactoryReset();
                    }
                }
                pair.Value.AIFactoryReset();
            }
        }
        yield return null;
    }

    /// <summary>
    /// Is used by MiniMax() while deth>1 to simulate a turn and change the score according to the outcome. 
    /// </summary>
    /// <param name="score"></param>
    /// <param name="color"></param>
    private IEnumerator AiTurn(int score, bool color)
    {
        foreach (KeyValuePair<int, Block> pair in playableBlock)
        {
            int play1;
            int play2;
            if (pair.Value.hasColor == false && pair.Value.IsAvailable() == true)
            {
                play1 = pair.Key;
                pair.Value.SetAIColor(color);
                foreach (KeyValuePair<int, Block> couple in playableBlock)
                {
                    if (couple.Value.hasColor == false && couple.Value.IsAvailable() == true)
                    {
                        play2 = couple.Key;
                        couple.Value.SetAIColor(color);
                        score += CalculatesScores(play1, play2, color);
                        couple.Value.AIFactoryReset();
                    }
                }
                pair.Value.AIFactoryReset();
            }
        }
        yield return null;
    }

    /// <summary>
    /// Calculates and returns the score of a playmoove. The stronger this function get, the strongest the AI. 
    /// </summary>
    /// <param name="play1"></param>
    /// <param name="play2"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    private int CalculatesScores(int play1, int play2,bool color)
    {
        Block block1 = playableBlock.GetValueOrDefault(play1);
        Block block2 = playableBlock.GetValueOrDefault(play2);
        if (!color)
        {
            if (block1.IsSquare())
            {
                if (block1.CheckAIEndGame())
                {
                    return 1000;
                }
                if (block2.IsSquare())
                {
                    return 800;
                }
            }
            if (block2.IsSquare())
            {
                if (block2.CheckAIEndGame())
                {
                    return 1000;
                }
                else
                {
                    return 500;
                }
            }

            return Random.Range(0, 100);
        }
        else
        {
            if (block1.IsSquare())
            {
                if (block1.CheckAIEndGame())
                {
                    return -10000;
                }
                if (block2.IsSquare())
                {
                    return -1100;
                }
            }
            if (block2.IsSquare())
            {
                if (block2.CheckAIEndGame())
                {
                    return -10000;
                }
                else
                {
                    return -300;
                }
            }
            return -Random.Range(0, 100);
        }
    }
    

    /// <summary>
    /// Manage the AI turn. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator RunAI()
    {
        Dictionary<int, GameObject> historyList = GameManager.Instance.historyList;
        playableBlock.Remove(historyList.GetValueOrDefault(historyList.Count - 2 ).GetComponent<Block>().key);
        playableBlock.Remove(historyList.GetValueOrDefault(historyList.Count - 1).GetComponent<Block>().key);
        Dictionary<int, Vector3Int> scores = new Dictionary<int, Vector3Int>();
        if (playableBlock.Count < 10)
        {
            yield return StartCoroutine(MiniMax(5, GameManager.Instance.color, scores));
        }
        if ( 10 <= playableBlock.Count && playableBlock.Count < 30)
        {
            yield return StartCoroutine(MiniMax(3, GameManager.Instance.color, scores));
        }
        else
        {
            yield return StartCoroutine(MiniMax(1, GameManager.Instance.color, scores));
        }
        int playScore = 0;
        foreach(KeyValuePair<int, Vector3Int> pair in scores)
        {
            if(pair.Value.x> scores.GetValueOrDefault(playScore).x)
            {
                playScore = pair.Key;
            }
        }
        playableBlock.Remove(scores.GetValueOrDefault(playScore).y);
        playableBlock.Remove(scores.GetValueOrDefault(playScore).z);
        GameManager.Instance.AICheckAndChangeBlock(playablePositions.GetValueOrDefault(scores.GetValueOrDefault(playScore).y));
        yield return new WaitForEndOfFrame();
        GameManager.Instance.AICheckAndChangeBlock(playablePositions.GetValueOrDefault(scores.GetValueOrDefault(playScore).z));
        yield return new WaitForEndOfFrame() ;
        GameManager.Instance.isAiPlaying = false;
    }

}
