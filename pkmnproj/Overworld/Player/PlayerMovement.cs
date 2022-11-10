using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float movementspeed = 8f;
	
	private Animator anim;

	void Start()
	{
		anim = GetComponent<Animator>();
	}
}
