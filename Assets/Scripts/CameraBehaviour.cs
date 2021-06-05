using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehaviour : MonoBehaviour
{
	[SerializeField]
	private float _shakeDuration = 0.15f;
	[SerializeField]
	private float _shakeMagnitude = 0.4f;



	void OnEnable()
	{
		Player.onUpdateLives += TakeDamage;
	}


	void OnDisable()
	{
		Player.onUpdateLives -= TakeDamage;
	}


	void TakeDamage(int amount)
	{
		StartCoroutine(Shake(_shakeDuration, _shakeMagnitude));
	}


	IEnumerator Shake(float duration, float magnitude)
	{
		Vector3 originalPos = transform.position;
		float elapsed = 0f;

		while (elapsed < duration)
		{
			float x = Random.Range(-1f, 1f) * magnitude;
			float y = Random.Range(-1f, 1f) * magnitude;

			transform.position = new Vector3(x, y, transform.position.z);
			elapsed += Time.deltaTime;
			yield return 0;
		}
	}
}