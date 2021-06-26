using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SkillNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Text _skillName;
	[SerializeField]
	private Image _nodeImage;
	[SerializeField]
	private Connector _connector;
	[SerializeField]
	private Outline _outline;

	[Space(10)]

	[SerializeField]
	private Color _unlearnedSkill;
	[SerializeField]
	private Color _learnedSkill;
	[SerializeField]
	private Color _lockedSkill;

	[HideInInspector]
	public Skill skill;



	public void Initialise(Skill skill)
	{
		this.skill = skill;
		_skillName.text = skill.skillName;
		_outline.effectColor = skill.skillUIColour;

		if (skill.availabilityState == Skill.AvailabilityState.Locked)
		{
			_nodeImage.color = _lockedSkill;
			SkillEvents.onSkillUnlocked += UnlockedSkill;
		}

		GetComponent<Button>().onClick.AddListener(LearnSkill);
	}


	public void LearnSkill()
	{
		if (skill.availabilityState == Skill.AvailabilityState.Unlocked && skill.BuySkill())
		{
			_nodeImage.color = _learnedSkill;
		}
	}


	public void UnlockedSkill(Skill skill)
	{
		if (skill == this.skill)
		{
			_nodeImage.color = _unlearnedSkill;
			SkillEvents.onSkillUnlocked -= UnlockedSkill;
		}
	}


	public void Connect(List<SkillNode> nodes, Transform parent)
	{
		foreach (RequiredSkill skill in skill.skillRequirements)
		{
			SkillNode connectedNode = nodes.Find(s => s.skill.skillName == skill.skillName);
			Connector nodeConnector = Instantiate(_connector, parent);
			nodeConnector.MakeConnections(transform.position, connectedNode.transform.position, connectedNode.skill.skillUIColour);
		}
	}


	public void OnPointerEnter(PointerEventData eventData)
	{
		Tooltip.Instance.Show(skill);
	}


	public void OnPointerExit(PointerEventData eventData)
	{
		Tooltip.Instance.Hide();
	}
}