using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUiManager : MonoBehaviour
{
    #region variables
    [SerializeField] private TextMeshProUGUI player1;
    [SerializeField] private TextMeshProUGUI player2;
    [SerializeField] private TextMeshProUGUI timer1;
    [SerializeField] private TextMeshProUGUI timer2;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button backToMenu;

    private GameManager gameManager;
    #endregion

    private void Start()
    {
        gameOver.SetActive(false);
        backToMenu.onClick.AddListener(OnMenuClick);
        gameManager = GameManager.Instance;
        player1.text = string.Format("{0}", gameManager.player1Name);
        player2.text = string.Format("{0}", gameManager.player2Name);
    }

    private void Update()
    {
        UpdateTime();
    }
    private void UpdateTime()
    {
        float minute = Mathf.FloorToInt(Mathf.Abs(gameManager.time1) / 60f);
        float second = Mathf.FloorToInt(Mathf.Abs(gameManager.time1) % 60f);
        timer1.text = string.Format("{0}min{1}s", minute, second);

        
        minute = Mathf.FloorToInt(Mathf.Abs(gameManager.time2) / 60f);
        second = Mathf.FloorToInt(Mathf.Abs(gameManager.time2) % 60f);
        timer2.text = string.Format("{0}min{1}s", minute, second);
    }

    private void OnMenuClick()
    {
        SceneManager.LoadScene(0);
    }

    public void PrintGameOver(bool winner)
    {
        gameOver.SetActive(true);
        gameOverText.text = string.Format("{0} won", winner ? gameManager.player1Name : gameManager.player2Name);
    }
}
