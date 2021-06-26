using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillDatabase : MonoBehaviour
{
	public List<SkillGroup> skillGroups;
}



[System.Serializable]
public class SkillGroup
{
	public List<Skill> skills;
}