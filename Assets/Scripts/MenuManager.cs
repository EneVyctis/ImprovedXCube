using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Buttons
    [SerializeField] private Button local;
    [SerializeField] private Button versusAI;
    [SerializeField] private Button multiplayer;
    #endregion
    void Start()
    {
        local.onClick.AddListener(OnLocalClick);
    }

    private void OnLocalClick()
    {
        SceneManager.LoadScene(1);
    }
}
