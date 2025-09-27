using System;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private Rigidbody2D _rb;
    
    [Header("Movement")]
    public float antSpd;

    [SerializeField] private bool isFlipped;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void ProcessMove(Vector2 direction)
    {
        if (direction.x != 0)
        {
            var dir = direction.x;
             if (!isFlipped)
             {
                 _rb.linearVelocityX = dir * antSpd * Time.deltaTime * 100;
                 return;
             }
             _rb.linearVelocityY = dir * antSpd *  Time.deltaTime * 100;
        }
        else
        {
            _rb.linearVelocityX = 0;
            _rb.linearVelocityY = 0;
        }
    }
}
