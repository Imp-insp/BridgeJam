using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnt : MonoBehaviour
{
    public static PlayerAnt Instance;

    [Header("References")] [SerializeField]
    private Ant antRef;

    [SerializeField] private Transform chainStartPoint;


    [Header("Stats")] 
    public float antMoveTime;
    [SerializeField] private float bridgeCd;

    [Header("Ants")] [SerializeField] private int antAmount;

    [SerializeField] private int foodAmount;
    private List<Ant> allAnts = new();

    [Header("Ui")] [SerializeField] private GameObject arrow;
    [SerializeField] private TextMeshProUGUI antAmountText;
    [SerializeField] private List<Image> foodNodes;


    [Header("Control")] public bool areWalking;
    public static bool hitWall;
    private Coroutine _bridgeCoroutine;

    private bool _bridgeOnCd;

    private void Awake()
    {
        #region Singleton

        if (!Instance) Instance = this;
        else Destroy(gameObject);

        #endregion
    }

    private void Start()
    {
        for (var i = 0; i < antAmount; i++)
        {
            AddAnt();
        }

        foreach (var img in foodNodes)
        {
            img.enabled = false;
        }
    }

    private void Update()
    {
        var direction = InputHandler.mousePos - (Vector2)chainStartPoint.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        arrow.transform.rotation = Quaternion.Slerp(arrow.transform.rotation, targetRotation, Time.deltaTime * 10);
    }

    public void StartMakingBridge()
    {
        if (PlayerMotor.WalkingOnAnts)
        {
            PlayerManager.Instance.NoBridgesAnimation();
            return;
        }
        if (_bridgeOnCd) return;
        StartCoroutine(BridgeCd());

        if (_bridgeCoroutine != null)
        {
            AudioManager.Instance.Play("Ant Back");
            DeactivateBridge();
            return;
        }

        _bridgeCoroutine = StartCoroutine(MakeBridge());
    }

    public void DeactivateBridge()
    {
        if (_bridgeCoroutine != null) StopCoroutine(_bridgeCoroutine);
        _bridgeCoroutine = null;
        foreach (var ant in allAnts)
        {
            ant.DeactivateColl();
        }

        UpdtUi(allAnts.Count);
    }

    private IEnumerator BridgeCd()
    {
        _bridgeOnCd = true;
        yield return new WaitForSeconds(bridgeCd);
        _bridgeOnCd = false;
    }

    private IEnumerator MakeBridge()
    {
        if (Time.timeScale == 0) yield break;

        AudioManager.Instance.Play("Shoot");

        var direction = InputHandler.mousePos - (Vector2)chainStartPoint.position;
        var startPos = chainStartPoint.position;
        for (var i = 0; i < allAnts.Count; i++)
        {
            var ant = allAnts[i];
            areWalking = true;

            ant.transform.position = startPos;
            UpdtUi(allAnts.Count - i - 1);
            switch (i)
            {
                case 0:
                    ant.Activate(direction);
                    break;
                default:
                    ant.Activate(allAnts[i - 1].end.position, direction, i);
                    break;
            }

            while (areWalking)
            {
                if (hitWall) break;
                yield return null;
            }
        }

        hitWall = false;
    }

    private void UpdtUi(int antUiAmount)
    {
        if (antUiAmount != -1) antAmountText.text = antUiAmount.ToString();
        for (var i = 0; i < foodAmount; i++)
        {
            foodNodes[i].enabled = true;
        }
    }

    public void AddAnt()
    {
        var newAnt = Instantiate(antRef);

        allAnts.Add(newAnt);
        UpdtUi(allAnts.Count);
    }

    public void AddFood(int value)
    {
        foodAmount += value;
        UpdtUi(-1);
        if (foodAmount < 3) return;
        CompleteFood();
    }

    private void CompleteFood()
    {
        foodAmount -= 3;
        AddAnt();

        foreach (var img in foodNodes)
        {
            img.enabled = false;
        }
    }
}