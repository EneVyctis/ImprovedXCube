using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Account : MonoBehaviour
{
    public static Account Instance => instance;
    private static Account instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Account details
    public string playerName;
    public int elo;
    #endregion

    public void ChangePlayerName(string newName)
    {
        playerName = newName;
    }
}
