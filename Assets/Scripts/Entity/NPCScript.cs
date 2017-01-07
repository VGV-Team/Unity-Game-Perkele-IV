using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : EntityScript
{

	public bool FirstConversation = true;

	//public bool QuestInProgres1 = false;
	//public bool QuestInProgres2 = false;
	public List<QuestScript> questList = new List<QuestScript>();

	// Use this for initialization
	new void Start () {
		base.Start();

		questList.Add(new QuestScript(
			QuestType.Kill,
			"Kill Perkele",
			"Perkele is a threat. It must be killed for the greater good!",
			true,
			false,
			GameObject.Find("Perkele")
		));

		questList.Add(new QuestScript(
			QuestType.Kill,
			"Find and kill his lieutenant",
			"His minions are strong. Killing their leaders will make them weaker.",
			true,
			false,
			GameObject.Find("Enemy Lieutenant")
		));
	}


	public void StartConversation(GameObject interactor)
	{
		foreach (var quest in questList)
		{
			if(!interactor.GetComponent<UnitScript>().QuestList.Contains(quest))
				interactor.GetComponent<UnitScript>().QuestList.Add(quest);
		}

		if (FirstConversation)
		{
			print("hi");
			FirstConversation = false;

			// play sound
		}
		else
		{
			foreach (var quest in interactor.GetComponent<UnitScript>().QuestList)
			{
				if (quest.Type == QuestType.Kill && quest.Target!=null && !quest.Target.GetComponent<UnitScript>().Active)
				{
					quest.Completed = true;
					print("Quest "+ quest.Title +" finished");

					// TODO: loot drop
				}
			}

			print("go to work");

			// play some other sound
		}

		/*
		if (QuestInProgres1)
		{
			print("quest 1 done, go quest 2");
			QuestInProgres1 = false;
		}
		if (QuestInProgres2)
		{
			print("quest 2 done");
			QuestInProgres2 = false;
		}
		else
		{
			print("no more quest");
		}
		*/
	}
}
