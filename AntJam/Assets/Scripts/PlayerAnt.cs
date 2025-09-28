using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnt : MonoBehaviour
{
    public static PlayerAnt Instance;

    [Header("References")] [SerializeField]
    private Ant antRef;

    [SerializeField] private Transform chainStartPoint;
    [SerializeField] private Transform antSpawnPoint;
    [Header("Stats")] [SerializeField] private int antAmount;

    [Header("Ants")] [SerializeField] private Image arrow;
    [SerializeField] private List<Ant> allAnts = new();

    [Header("Control")] public bool areWalking;
    public bool constructing;

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
    }

    public void StartMakingBridge(Vector3 endPos)
    {
        StartCoroutine(MakeBridge(endPos));
    }
    
    private IEnumerator MakeBridge(Vector3 endPosition)
    {
        var targetPos = chainStartPoint.position - endPosition;

        for (var i = 0; i < allAnts.Count; i++)
        {
            var ant =  allAnts[i];
            ant.transform.SetParent(gameObject.transform.parent);
            areWalking = true;
            if (i == 0)
            {
                ant.Activate(chainStartPoint.position, targetPos);
            }
            else
            {
                ant.Activate(chainStartPoint.position, targetPos, allAnts[i-1].end.position );
            }
            while (areWalking) yield return null;
        }
    }

    public void AddAnt()
    {
        var newAnt = Instantiate(antRef);
        newAnt.transform.position = Vector3.zero;
        allAnts.Add(newAnt);
        
    }
}