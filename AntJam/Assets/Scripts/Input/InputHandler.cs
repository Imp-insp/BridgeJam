using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private AntInput antInput;
    private AntInput.AntActionsActions onAnt;
    
    private PlayerMotor _playerMotor;

    private void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>(); 
        antInput = new AntInput();
        onAnt = antInput.AntActions;

       
        
        /*onfoot.Jump.performed += ctx => motor.Jump();*/

       
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
        _playerMotor.ProcessMove(onAnt.Movement.ReadValue<Vector2>());
    }
}