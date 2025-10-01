using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Ui")] [SerializeField] private GameObject pauseMenu;

    [Header("Save")] [SerializeField] private Vector2 lastSave;
    private void Awake()
    {
        #region Singleton

        if (!Instance) Instance = this;
        else Destroy(gameObject);

        #endregion
    }

    private void Start()
    {
        lastSave = transform.position;
    }

    public void Die()
    {
        transform.position = lastSave;
    }

    public void SetCheckPoint(Vector2 newSave)
    {
        lastSave = newSave;
    }

    public void PauseMenu()
    {
        if (!pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
