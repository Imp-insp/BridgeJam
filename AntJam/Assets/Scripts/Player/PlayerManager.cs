using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private void Awake()
    {
        #region Singleton

        if (!Instance) Instance = this;
        else Destroy(gameObject);

        #endregion
    }

    public static void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
