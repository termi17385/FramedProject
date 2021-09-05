using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
    [SerializeField] private float moveSpeed;
	[SerializeField] private Animator anim;
	
	[NonSerialized] public Rigidbody2D rb;
    [NonSerialized] public LookMarker lookMarker;
    private float MoveSpeed => moveSpeed;
    
	[Header("Axis Indicators")]
    public float xSpeed;
    public float ySpeed;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        lookMarker = GetComponentInChildren<LookMarker>();
        lookMarker.hidden = false;
    }

    private void FixedUpdate() => rb.velocity = new Vector2(xSpeed * MoveSpeed, ySpeed * MoveSpeed);
    private void Update()
    {
		ySpeed = Input.GetAxis("Vertical");
		xSpeed = Input.GetAxis("Horizontal");

		var isMoving = xSpeed > 0.1f || xSpeed < -0.1f || ySpeed > 0.1f || ySpeed < -0.1f;
		anim.enabled = isMoving;
		
		if(isMoving)
		{
			lookMarker.RotateToDirectionMoving();
			if(lookMarker.hidden) lookMarker.HideUnHide(null); 
		}
		
       
		lookMarker.Interact();
    }
}
