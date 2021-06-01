using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Explosion : MonoBehaviour
{
	public static event Action onExplosion;



    void OnEnable()
    {
		onExplosion?.Invoke();
    }
}