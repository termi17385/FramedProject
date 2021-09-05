using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookMarker : MonoBehaviour
{
	private Rigidbody2D rb;

	private void Awake() => rb = GetComponentInParent<Rigidbody2D>();
	public void RotateToDirectionMoving()
	{
		Vector2 dir = rb.velocity;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				
		transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
	}
	
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, transform.right * 1);
	}
}
