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


    [Header("Colectables Positions")] public Transform foodCool;
    public Transform eggCool;
    public Transform secretCool;
    
    [Header("Animation")] [SerializeField] private float checkAnimDuration;
    [SerializeField] private float yPos;
    [SerializeField] private float centerDuration;

    [Header("Achivement")] [SerializeField]
    private string checkPointAchivementTxt;

    [SerializeField] private string secretAchivementTxt;
    [SerializeField] private string noBridgesAchivementTxt;

    public int secretsFound;
    [SerializeField] private int maxSecrets;

    [Header("Ui")] [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI checkPointText;
    [SerializeField] private TextMeshProUGUI secretText;
    [SerializeField] private GameObject movementX;

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
        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    public void Die()
    {
        transform.position = lastPos;
        transform.rotation = lastRot;
        PlayerAnt.Instance.DeactivateBridge();
        AudioManager.Instance.Play("Death");
    }



    public void SetCheckPoint(Vector2 newPos, Quaternion newRot)
    {
        if (newPos == lastPos) return;
        StartCoroutine(PlayAchivementAnimation(checkPointAchivementTxt));
        lastPos = newPos;
        lastRot = newRot;
    }

    public void AquireSecret()
    {
        secretsFound++;
        secretText.text = secretsFound + "/" + maxSecrets;
        StartCoroutine(PlayAchivementAnimation(secretAchivementTxt));
    }

    public IEnumerator PlayAchivementAnimation(string achivementTxt)
    {
        checkPointText.transform.DOKill();
        checkPointText.text = achivementTxt;
        
        checkPointText.transform.DOLocalMoveX(-Screen.width, 0f);
        checkPointText.transform.DOLocalMoveX(0, checkAnimDuration);
        yield return new WaitForSeconds(checkAnimDuration);
        
        yield return new WaitForSeconds(centerDuration);
        
        checkPointText.transform.DOLocalMoveX(Screen.width, checkAnimDuration);
        yield return new WaitForSeconds(checkAnimDuration);
    }

    public void NoBridgesAnimation()
    {
        StartCoroutine(PlayAchivementAnimation(noBridgesAchivementTxt));
    }

    public void ChangeMovement()
    {
        PlayerMotor.invertedMovement = !PlayerMotor.invertedMovement;
        movementX.SetActive(PlayerMotor.invertedMovement);
       
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