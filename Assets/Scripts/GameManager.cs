using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance => instance;
    private static GameManager instance;

    #region variables
    //All of the following dictionnaries store useful pieces of information for map's elements and are used to generate the map. The last parameter of the Vector3 is the rotation.
    public Dictionary<Int32, Vector3> squarePositions = new Dictionary<int, Vector3>();
    public Dictionary<Int32, Vector3> sidePositions = new Dictionary<int, Vector3>();
    public Dictionary<Int32, GameObject> sideList = new Dictionary<Int32, GameObject>();
    public Dictionary<Int32, GameObject> squareList = new Dictionary<Int32, GameObject>();

    //These two dictionnaries are used not for the generation but for every other steps of the game.
    public Dictionary<Int32, GameObject> blocksList = new Dictionary<Int32, GameObject>();
    public Dictionary<Int32, GameObject> historyList = new Dictionary<Int32, GameObject>();

    //Prefabs for the map.
    [SerializeField] private GameObject square;
    [SerializeField] private GameObject side;

    [SerializeField] private AIManager aI;
    [SerializeField] private GameUiManager uiManager;

    //Game's variables.
    public bool color = true;
    private int remainingActions = 2;
    private bool isGameOver;
    public bool isAiPlaying;
    public string player1Name;
    public string player2Name;
    public float time1;
    public float time2;
    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            player1Name = PlayerPrefs.GetString("PlayerName");
        }
        if (GameModeManager.isVersusAi)
        {
            player2Name = "AI";
        }
    }

    void Start()
    {
        isGameOver = false;
        GeneratePositions();
        DrawMap();
        ManageNeighbors();
            
    }

    void Update()
    {
        if (isGameOver == false)
        {
            UpdateAndManageTime();
            if (remainingActions <= 0)
            {
                color = !color;
                remainingActions = 2;
            }
            if (GameModeManager.isVersusAi == false || color == true)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    CheckAndChangeBlock(touch.position);
                }
            }
            else
            {
                if (color == false && isAiPlaying == false)
                {
                    isAiPlaying = true;
                    StartCoroutine(aI.RunAI());
                }
            }
        }
    }


    #region Set-Up Functions

    /// <summary>
    /// Generates all coordonates of squares and sides and store them in squarepositions and sidepositions
    /// </summary>
    private void GeneratePositions()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                squarePositions.Add(squarePositions.Count, new Vector3(-1.5f + j, 1.5f - i, 0f));
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
    /// Instantiate all gameObject based on squarePositions and sidePositions dictionnaries. 
    /// </summary>
    private void DrawMap()
    {
        foreach (KeyValuePair<int, Vector3> pair in squarePositions)
        {
            GameObject clone = Instantiate(square, pair.Value, Quaternion.identity);
            squareList.Add(squareList.Count, clone);
            blocksList.Add(blocksList.Count, clone);
            clone.GetComponent<Block>().key = squareList.Count - 1;
        }

        foreach (KeyValuePair<int, Vector3> pair in sidePositions)
        {
            GameObject clone = Instantiate(side, pair.Value, Quaternion.Euler(new Vector3(0f, 0f, pair.Value.z)));
            sideList.Add(squareList.Count + sideList.Count, clone);
            blocksList.Add(blocksList.Count, clone);
            clone.GetComponent<Block>().key = blocksList.Count - 1;
        }
    }

    /// <summary>
    /// Initialize all neighbors
    /// </summary>
    private void ManageNeighbors()
    {
        foreach (KeyValuePair<Int32, GameObject> pair in squareList)
        {
            pair.Value.GetComponent<Square>().CalculateNeighborhood();
        }
        foreach (KeyValuePair<Int32, GameObject> pair in sideList)
        {
            pair.Value.GetComponent<Side>().CalculateNeighborhood();
        }
    }

    /// <summary>
    /// Attemps to play an action when the screen is touched.
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    #endregion


    #region Game functions
    ///<summary>
    /// Just a intermediate functions that calls setSprite functions with the correct parameters.
    ///</summary>
    ///<param name="collider"></param><param name="color"></param>
    private bool ChangeState(GameObject collider, bool color)
    {
        if (collider.TryGetComponent<Block>(out Block block))
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
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero);
        if (hit.collider != null)
        {
            GameObject block = hit.collider.gameObject;
            if (ChangeState(block, color))
            {
                historyList.Add(historyList.Count, block);
                remainingActions -= 1;
            }
        }
    }

    /// <summary>
    /// Manage end of the game (yeah really informative i know).
    /// </summary>
    /// <param name="winner"></param>
    public void GameHasEnded(bool winner)
    {
        uiManager.PrintGameOver(winner);
        isGameOver = true;
    }

    #endregion


    /// <summary>
    /// Same as the CheckAndChangeBlock() function but adapt to AI's uses.
    /// </summary>
    /// <param name="position"></param>
    public void AICheckAndChangeBlock(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject block = hit.collider.gameObject;
            if (ChangeState(block, color))
            {
                historyList.Add(historyList.Count, block);
                remainingActions -= 1;
            }
        }
    }

    /// <summary>
    /// Manage and actualize player's timers.
    /// </summary>
    private void UpdateAndManageTime()
    {
        if (color == true)
        {
            time1 -= Time.deltaTime;
        }
        if (color == false)
        {
            time2 -= Time.deltaTime;
        }

        //Only one can go below 0 before the game ends. Color will thus be the looser.
        if(time1 < 0 || time2<0)
        {
            GameHasEnded(!color);
        }
    }
}
