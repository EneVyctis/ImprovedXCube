using System.Collections;
using System.Collections.Generic;
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
    /// Calculates each possible moove along with its score and store them in the "scores" dictionnary.
    /// </summary>
    /// <param name="Depth"></param>
    private void MiniMax(int Depth, Dictionary<int, Vector3Int> scores)
    {

        foreach (KeyValuePair<int, Block> pair in playableBlock)
        {
            int play1;
            int play2;
            bool color = GameManager.Instance.color;
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
                        CalculatesScores(play1, play2, scores);
                        couple.Value.AIFactoryReset();
                    }
                }
                pair.Value.AIFactoryReset();
            }
        }
    }

    private bool CalculatesScores(int play1, int play2, Dictionary<int, Vector3Int> scores)
    {
        Block block1 = playableBlock.GetValueOrDefault(play1);
        Block block2 = playableBlock.GetValueOrDefault(play2);

        if (block1.IsSquare())
        {
            if (block1.CheckAIEndGame())
            {
                scores.Add(scores.Count, new Vector3Int(1000, play1, play2));
                return true;
            }
            if(block2.IsSquare())
            {
                scores.Add(scores.Count, new Vector3Int(800, play1, play2));
                return true;
            }
        }
        if (block2.IsSquare())
        {
            if (block2.CheckAIEndGame())
            {
                scores.Add(scores.Count, new Vector3Int(1000, play1, play2));
                return true;
            }
            else
            {
                scores.Add(scores.Count, new Vector3Int(500, play1, play2));
                return true;
            }
        }
        
        //Give a random score so the AI choose a random play by default.
        scores.Add(scores.Count, new Vector3Int(Random.Range(0,100), play1, play2));

        return true;
    }
    
    public IEnumerator RunAI()
    {
        Dictionary<int, GameObject> historyList = GameManager.Instance.historyList;
        playableBlock.Remove(historyList.GetValueOrDefault(historyList.Count - 2 ).GetComponent<Block>().key);
        playableBlock.Remove(historyList.GetValueOrDefault(historyList.Count - 1).GetComponent<Block>().key);
        Dictionary<int, Vector3Int> scores = new Dictionary<int, Vector3Int>();
        MiniMax(1, scores);
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
