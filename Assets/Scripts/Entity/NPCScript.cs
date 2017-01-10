using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCScript : EntityScript
{

	public bool FirstConversation = true;

	//public bool QuestInProgres1 = false;
	//public bool QuestInProgres2 = false;
	public List<QuestScript> questList = new List<QuestScript>();

	public GameObject QuestTarget1;
	public GameObject QuestTarget2;
	public GameObject QuestReward1;

	// Use this for initialization
	new void Start () {
		base.Start();
		/*
		questList.Add(new QuestScript(
			QuestType.Kill,
			"Kill Perkele",
			"Perkele is a threat. It must be killed for the greater good!",
			true,
			false,
			GameObject.Find("Perkele")
		));
		*/

		questList.Add(new QuestScript(
			QuestType.Kill,
			"Kill Skeleton King",
			"He is there. At the church. Looking at the bones. Using them. Find him. Kill him.",
			true,
			false,
			QuestTarget1
		));

		questList.Add(new QuestScript(
			QuestType.Kill,
			"Find and kill his lieutenant",
			"His minions are strong. Killing their leaders will make them weaker.",
			true,
			false,
			QuestTarget2
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

			interactor.GetComponent<UnitScript>().Abilities.Add(new AbilityScript("Fireball", AbilityType.Fireball, 5, -15, 10, 10, 20, GameObject.Find("UISpritesFireball").transform.GetComponent<SpriteRenderer>().sprite));
			interactor.GetComponent<UnitScript>().Abilities.LastOrDefault().Description = "The most magnificent fireball that will kill balls";

			// TODO: play sound
		}
		else
		{
			bool anyQuestCompleted = false;
			foreach (var quest in interactor.GetComponent<UnitScript>().QuestList)
			{
				if (quest.Type == QuestType.Kill && quest.Target!=null && !quest.Target.GetComponent<UnitScript>().Active && quest.Completed != true)
				{
					quest.Completed = true;
					print("Quest "+ quest.Title +" finished");

					// TODO: loot drop
					if (quest.Target == QuestTarget1)
					{
						interactor.GetComponent<UnitScript>().InventoryItemsList.Add(QuestReward1);
						interactor.GetComponent<UnitScript>().AbilityPoints += 2;

						// TODO: play sound
					}

					anyQuestCompleted = true;
				}
			}

			if (!anyQuestCompleted)
			{
				// TODO: play sound
				print("go to work");
			}
			

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
