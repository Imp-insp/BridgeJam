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

    [Header("Control")] public bool constructing;

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

    public void MakeBridge(Vector3 endPosition)
    {
        var targetPos = chainStartPoint.position - endPosition;

        for (var i = 0; i < allAnts.Count; i++)
        {
            var ant =  allAnts[i];
            if (i == 0)
            {
                ant.Activate(chainStartPoint.position, targetPos);
            }
            else
            {
                ant.Activate(chainStartPoint.position, targetPos, allAnts[i-1].end.position );
            }
            
        }
    }

    public void AddAnt()
    {
        var newAnt = Instantiate(antRef, antSpawnPoint);
        allAnts.Add(newAnt);
    }
}