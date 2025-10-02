using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Animation")] [SerializeField] private float checkAnimDuration;
    [SerializeField] private float centerOffSet;
    [SerializeField] private float centerDuration;
    
    
    [Header("Ui")] [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI checkPointText;
    private Camera _mainCamera;

    [Header("Save")] [SerializeField] private Vector2 lastPos;
    [SerializeField] private Quaternion lastRot;
    
    private void Awake()
    {
        #region Singleton

        if (!Instance) Instance = this;
        else Destroy(gameObject);

        #endregion
    }

    private void Start()
    {
        _mainCamera =  Camera.main;
        
        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    public void Die()
    {
        transform.position = lastPos;
        transform.rotation = lastRot;
        PlayerAnt.Instance.DeactivateBridge();
    }

    public void SetCheckPoint(Vector2 newPos, Quaternion newRot)
    {
        if (newPos == lastPos) return;
        StartCoroutine(CheckPointAnimation());
        lastPos = newPos;
        lastRot = newRot;
    }

    private IEnumerator CheckPointAnimation()
    {
        checkPointText.transform.DOLocalMoveX(0/*-centerOffSet*/, checkAnimDuration);
            yield return new WaitForSeconds(checkAnimDuration);
            /*checkPointText.transform.DOLocalMoveX(0+centerOffSet, centerDuration);*/
            yield return new WaitForSeconds(centerDuration);
            checkPointText.transform.DOLocalMoveX(_mainCamera.pixelWidth, checkAnimDuration);
            yield return new WaitForSeconds(checkAnimDuration);
            checkPointText.transform.position = new Vector2(-_mainCamera.pixelWidth, checkPointText.transform.position.y);

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
