using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Account : MonoBehaviour
{
    [SerializeField] private Button backToMenu;
    [SerializeField] private TextMeshProUGUI pseudo;
    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            pseudo.text = PlayerPrefs.GetString("PlayerName");
            Debug.Log("Game data loaded!");
        }
        backToMenu.onClick.AddListener(OnBackToMenu);
    }

    private void OnBackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    #region Account details
    public string playerName;
    public int elo;
    #endregion

    public void ChangePlayerName(string newName)
    {
        pseudo.text = newName;
        playerName = newName;
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
    }
}
