using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class Skill
{
	public enum AvailabilityState { Locked, Unlocked, Learned }

	public AvailabilityState availabilityState;
	public string skillName;
	public string skillDescription;
	public int pointsCost;
	public List<RequiredSkill> skillRequirements;
	public List<SkillModifiers> skillModifiers;
	public Color skillUIColour;



	public Skill()
	{
		if (availabilityState == AvailabilityState.Locked)
		{
			SkillEvents.onSkillLearned += CheckRequirements;
		}
	}


	public void Unlock()
	{
		availabilityState = AvailabilityState.Unlocked;
		SkillEvents.SkillUnlocked(this);
		SkillEvents.onSkillLearned -= CheckRequirements;
		Debug.Log("Skill was unlocked: " + skillName);
	}


	public void Learn()
	{
		SkillEvents.SkillLearned(this);
		availabilityState = AvailabilityState.Learned;
		Debug.Log("Skill was learned: " + skillName);
	}


	void CheckRequirements(Skill skill)
	{
		if (availabilityState == AvailabilityState.Locked)
		{
			RequiredSkill requiredSkill = skillRequirements.FirstOrDefault(rs => rs.skillName == skill.skillName);
			if (requiredSkill != null && requiredSkill.completed == false)
			{
				requiredSkill.completed = true;
				if (skillRequirements.Any(s => s.completed == false) == false)
				{
					Unlock();
				}
			}
		}
	}


	public bool BuySkill()
	{
		bool hasEnoughResources = false;

		int currentAmount = PlayerData.Instance.GetUpgradePoints();

		if (currentAmount >= pointsCost)
		{
			hasEnoughResources = true;
			SkillEvents.SkillBought(pointsCost);
			Learn();
		}

		return hasEnoughResources;
	}
}



[System.Serializable]
public class RequiredSkill
{
	public string skillName;
	public bool completed;
}


[System.Serializable]
public class SkillModifiers
{
	public enum ModifierType { Gold, Steel, Wood, Water, Population, Food }

	public ModifierType modifier;
	public float amount;
}