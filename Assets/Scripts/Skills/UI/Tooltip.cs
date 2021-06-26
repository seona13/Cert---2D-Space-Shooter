using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Tooltip : MonoBehaviour
{
	public static Tooltip Instance { get; private set; }

	[SerializeField]
	private Text _description;
	[SerializeField]
	private Text _modifiers;
	[SerializeField]
	private Text _cost;



	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}

		gameObject.SetActive(false);
	}


	void LateUpdate()
	{
		transform.position = Input.mousePosition;
	}


	public void Show(Skill skill)
	{
		_description.text = skill.skillDescription;

		string mods = string.Join("\n", skill.skillModifiers.Select(m => string.Format("+{0} {1}", m.amount, m.modifier)));
		_modifiers.text = mods;

		_cost.text = skill.pointsCost + " points";

		gameObject.SetActive(true);
	}


	public void Hide()
	{
		gameObject.SetActive(false);
	}
}