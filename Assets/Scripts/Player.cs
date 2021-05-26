using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
	public float moveSpeed = 10f;

	private Vector3 startPos = new Vector3(0, -2.5f, 0);
	private Vector3 moveVec = Vector3.zero; // movement direction of player



	void Start()
	{
		transform.position = startPos;
	}


	void Update()
	{
		transform.Translate(moveVec * moveSpeed * Time.deltaTime);
	}


	public void OnMove(InputValue input)
	{
		Vector2 inputVec = input.Get<Vector2>();

		moveVec = new Vector3(inputVec.x, inputVec.y, 0);
	}
}