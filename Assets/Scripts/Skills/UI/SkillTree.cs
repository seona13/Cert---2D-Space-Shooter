using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillTree : MonoBehaviour
{
	private List<SkillGroup> _skillGroups;
	[SerializeField]
	private Transform _skillPanel;
	[SerializeField]
	private GameObject _skillRow;
	[SerializeField]
	private SkillNode _skillNode;
	private List<SkillNode> _skillNodes = new List<SkillNode>();



	void Start()
	{
		_skillGroups = FindObjectOfType<SkillDatabase>().skillGroups;
		StartCoroutine(BuildTree());
	}


	IEnumerator BuildTree()
	{
		for (int i = 0; i < _skillGroups.Count; i++)
		{
			Transform row = Instantiate(_skillRow, _skillPanel).transform;

			for (int j = 0; j < _skillGroups[i].skills.Count; j++)
			{
				SkillNode node = Instantiate(_skillNode, row);
				_skillNodes.Add(node);
				node.Initialise(_skillGroups[i].skills[j]);
			}
		}

		yield return 0;

		for (int i = 0; i < _skillNodes.Count; i++)
		{
			_skillNodes[i].Connect(_skillNodes, _skillPanel);
		}
	}
}