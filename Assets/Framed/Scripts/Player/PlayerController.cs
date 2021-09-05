using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Move Speed")]
    [SerializeField] private float moveSpeed;
    
	[NonSerialized] public Rigidbody2D rb;
    private float MoveSpeed => moveSpeed;
    private LookMarker lookMarker;
    
	[Header("Axis Indicators")]
    public float xSpeed;
    public float ySpeed;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        lookMarker = GetComponentInChildren<LookMarker>();
    }

    private void FixedUpdate() => rb.velocity = new Vector2(xSpeed * MoveSpeed, ySpeed * MoveSpeed);
    private void Update()
    {
       ySpeed = Input.GetAxis("Vertical");
       xSpeed = Input.GetAxis("Horizontal");

       var isMoving = xSpeed > 0.1f || xSpeed < -0.1f || ySpeed > 0.1f || ySpeed < -0.1f;
       if(isMoving) lookMarker.RotateToDirectionMoving();
    }
}
