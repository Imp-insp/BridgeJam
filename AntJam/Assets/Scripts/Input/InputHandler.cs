using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private AntInput antInput;
    private AntInput.AntActionsActions onAnt;
    
    private Camera Camera => Camera.main;
    
    private PlayerMotor _playerMotor;
    private PlayerAnt _playerAnt;

    [Header("inputNumbers")] public static Vector2 mousePos;

    private void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>(); 
        _playerAnt = GetComponent<PlayerAnt>();
        antInput = new AntInput();
        onAnt = antInput.AntActions;
        
        onAnt.ShotChain.performed += ctx => _playerAnt.StartMakingBridge();

       
    }

    private void Update()
    {
        mousePos = Camera.ScreenToWorldPoint(onAnt.TargetPos.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onAnt.Enable();
    }

    private void OnDisable()
    {
        onAnt.Disable();
    }

    private void FixedUpdate()
    {
        _playerMotor.ProcessMovement(onAnt.Movement.ReadValue<Vector2>());
    }
}