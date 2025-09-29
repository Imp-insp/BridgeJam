using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnt : MonoBehaviour
{
    public static PlayerAnt Instance;

    [Header("References")] [SerializeField]
    private Ant antRef;

    [SerializeField] private Transform chainStartPoint;
    [SerializeField] private Transform antSpawnPoint;
    [Header("Stats")] [SerializeField] private int antAmount;

    [Header("Ants")] [SerializeField] private GameObject arrow;
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

    private void Update()
    {
        var direction = InputHandler.mousePos - (Vector2) transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        arrow.transform.rotation = Quaternion.Slerp(arrow.transform.rotation, targetRotation, Time.deltaTime * 10);
    }

    public void StartMakingBridge(Vector3 endPos)
    {
        StartCoroutine(MakeBridge(endPos));
    }
    
    private IEnumerator MakeBridge(Vector3 endPosition)
    {
        var targetPos = chainStartPoint.position - endPosition;
        var startPos = chainStartPoint.position;
        
        for (var i = 0; i < allAnts.Count; i++)
        {
            var ant =  allAnts[i];
            ant.transform.SetParent(gameObject.transform.parent);
            areWalking = true;
            if (i == 0)
            {
                ant.Activate(startPos, targetPos);
            }
            else
            {
                ant.Activate(startPos, targetPos, allAnts[i-1].end.position );
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