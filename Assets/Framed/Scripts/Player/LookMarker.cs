using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookMarker : MonoBehaviour
{
	[SerializeField] private LayerMask ignorePlayer;
	private Rigidbody2D rb;
	private GameObject indicator = null;

	public bool hidden;
	
	private void Awake() => rb = GetComponentInParent<Rigidbody2D>();
	public void RotateToDirectionMoving()
	{
		Vector2 dir = rb.velocity;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				
		transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
	}
	
	public void Interact()
	{
		var dist = 1;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * dist, 1,ignorePlayer);
		Debug.DrawRay(transform.position, transform.right * dist, Color.red);

		GameObject hitObj = null; 
		if(hit.collider.gameObject != null) hitObj = hit.collider.gameObject;

		if(hitObj.CompareTag("HidingSpot"))
			if(Input.GetKeyDown(KeyCode.E))
			{
				hidden = true;
				HideUnHide(hitObj.transform.Find("HiddenIndicator").gameObject);  
			}
	}

	public void HideUnHide(GameObject _obj)
	{
		if(_obj == null) hidden = false;

		switch(hidden) {
			case true: _obj.SetActive(true); gameObject.SetActive(false); break;
			case false: indicator.SetActive(false); gameObject.SetActive(true); break;
		}

		indicator = _obj;
	}
	//ivate void OnDrawGizmos()
	//
	//	Gizmos.color = Color.green;
	//	Gizmos.DrawRay(transform.position, transform.right * 1);
	//
}
