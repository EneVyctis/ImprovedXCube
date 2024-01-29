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
        local.onClick.AddListener(OnLocalClick);
        account.onClick.AddListener(OnAccountClick);
    }

    private void OnLocalClick()
    {
        SceneManager.LoadScene(1);
    }

    private void OnAccountClick()
    {
        SceneManager.LoadScene("Account");
    }
}
