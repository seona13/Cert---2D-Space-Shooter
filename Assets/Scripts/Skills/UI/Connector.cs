using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Connector : MonoBehaviour
{
	[SerializeField]
	private UnityEngine.UI.Image _image;



	public void MakeConnections(Vector3 fromPoint, Vector3 toPoint, Color colour)
	{
		Vector3 centrePosition = (fromPoint + toPoint) / 2;
		Vector3 direction = Vector3.Normalize(fromPoint - toPoint);

		transform.position = centrePosition;
		transform.right = direction;
		transform.localScale = new Vector3(Vector3.Distance(fromPoint, toPoint) / 10, 1, 1);
		transform.SetAsFirstSibling();

		_image.color = colour;
	}
}