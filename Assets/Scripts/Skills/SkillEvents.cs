using System;
using UnityEngine;


public class SkillEvents : MonoBehaviour
{
	public static Action<Skill> onSkillLearned;
	public static Action<Skill> onSkillUnlocked;
	public static Action<Skill, int> onSkillBought;



	public static void SkillLearned(Skill skill)
	{
		onSkillLearned?.Invoke(skill);
	}


	public static void SkillUnlocked(Skill skill)
	{
		onSkillUnlocked?.Invoke(skill);
	}


	public static void SkillBought(Skill skill, int amount)
	{
		onSkillBought?.Invoke(skill, amount);
	}
}