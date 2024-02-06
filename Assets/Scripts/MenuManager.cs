using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Buttons
    [SerializeField] private Button local;
    [SerializeField] private Button versusAI;
    [SerializeField] private Button multiplayer;
    [SerializeField] private Button account;
    #endregion
    void Start()
    {
        GameModeManager.isLocal = false;
        GameModeManager.isVersusAi = false;
        GameModeManager.isMultiplayer = false;
        local.onClick.AddListener(OnLocalClick);
        versusAI.onClick.AddListener(OnAIClick);
        account.onClick.AddListener(OnAccountClick);
    }

    private void OnLocalClick()
    {
        GameModeManager.isLocal = true;
        SceneManager.LoadScene(1);
    }

    private void OnAccountClick()
    {
        SceneManager.LoadScene("Account");
    }

    private void OnAIClick()
    {
        GameModeManager.isVersusAi = true;
        SceneManager.LoadScene(1);
    }
}
