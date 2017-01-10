using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCScript : EntityScript
{

	public bool FirstConversation = true;
	public bool SecondConversation = true;

	//public bool QuestInProgres1 = false;
	//public bool QuestInProgres2 = false;
	public List<QuestScript> questList = new List<QuestScript>();

	public GameObject QuestTarget1;
	public GameObject QuestTarget2;
	public GameObject QuestReward1;

	protected AudioManagerScript AudioManager;

	// Use this for initialization
	new void Start () {
		base.Start();
		AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
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
			GameObject.Find("UI Handler").GetComponent<UIScript>().ShowConversationText("" +
			                                                                            "NPC: Ohh reinforcements! At last we are saved! You must be a scout, tell your army to hurry up.\n" +
			                                                                            "Player: Army? I am alone, there is noone else.\n" +
			                                                                            "NPC: What?? So alliance has left us here to rot... " +
			                                                                            "Look...I think their leader is at the church north of here. " +
			                                                                            "If you could somehow eradicate him from this world there might " +
			                                                                            "be a chance in getting out alive. But be careful, he is powerful and " +
			                                                                            "path is defended by many foes. From what I have seen he also has magic " +
			                                                                            "powers so that he can cheat death and send it against us....\n" +
			                                                                            "I will wait for you here. You know..logistics and planning. Well there is one more " +
																						"thing. I think his right hand lieutenant is at the old ruins, east of here. If you " +
			                                                                            "could take him out..well it might get easier for us..");

		}
		else
		{
			bool anyQuestCompleted = false;
			foreach (var quest in interactor.GetComponent<UnitScript>().QuestList)
			{
				if (quest.Type == QuestType.Kill && quest.Target!=null && !quest.Target.GetComponent<UnitScript>().Active && quest.Completed != true)
				{
					quest.Completed = true;
					//print("Quest "+ quest.Title +" finished");
					GameObject.Find("UI Handler")
						.GetComponent<UIScript>()
						.ShowConversationText("NPC: Ohh, you have killed him! Thank you soo much.. Here, take this. " +
						                      "A small treasure that has been in my family for decades. I hope it will help you in your quest one day...");

					// TODO: loot drop
					if (quest.Target == QuestTarget1)
					{
						interactor.GetComponent<UnitScript>().InventoryItemsList.Add(QuestReward1);
						interactor.GetComponent<UnitScript>().AbilityPoints += 2;

						// TODO: play sound
					}
					AudioManager.PlayQuestCompletedAudio();
					anyQuestCompleted = true;
				}
			}

			if (!anyQuestCompleted)
			{
				if (SecondConversation)
				{
					GameObject.Find("UI Handler")
						.GetComponent<UIScript>()
						.ShowConversationText("They came from the church...the big one is there..raising them " +
												"from the graveyard. I have seen..\n" +
					                            "The alliance send few soldiers..They tried to save us in vain. Who could ever have believed.. " +
						                      "It seems like nothing's been achieved. At the end the 12th army wanted to help and rescue us, civilians. " +
						                      "They tried to open a passage but because of that they could not hold the line. It was the end! " +
						                      "They tried to hold the corridor..for us to reach the river..");
					SecondConversation = false;
				}
				else
				{
					// TODO: play sound
					print("go to work");
					GameObject.Find("UI Handler")
						.GetComponent<UIScript>()
						.ShowConversationText("Don't just stand here..Go kill some of those beasts..");
				}
				
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
