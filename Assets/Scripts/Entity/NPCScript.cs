using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : EntityScript
{

	public bool FirstConversation = true;

	//public bool QuestInProgres1 = false;
	//public bool QuestInProgres2 = false;


	// Use this for initialization
	new void Start () {
		base.Start();
	}


	public void StartConversation(GameObject interactor)
	{

		if (FirstConversation)
		{
			print("hi");
			FirstConversation = false;
		}
		else
		{
			print("go to work");
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
