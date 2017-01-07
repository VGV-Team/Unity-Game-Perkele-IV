using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
	Kill,
	Deliver
}

public class QuestScript
{
	public QuestType Type;
	public string Title;
	public string Description;
	public bool Received;
	public bool Completed;
	public GameObject Target;

	public QuestScript()
	{
		Title = "";
		Description = "";
		Received = false;
		Completed = false;
	}

	public QuestScript(QuestType questType, string title, string description, bool received = false, bool completed = false, GameObject target = null)
	{
		Type = questType;
		Title = title;
		Description = description;
		Received = received;
		Completed = completed;
		Target = target;
	}
}
