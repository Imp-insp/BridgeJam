using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private AntInput antInput;
    private AntInput.AntActionsActions onAnt;
    
    private PlayerMotor _playerMotor;
    private PlayerAnt _playerAnt;

    private void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>(); 
        _playerAnt = GetComponent<PlayerAnt>();
        antInput = new AntInput();
        onAnt = antInput.AntActions;

       
        
        /*onAnt.Shot.performed += ctx => _playerAnt.Jump();*/

       
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