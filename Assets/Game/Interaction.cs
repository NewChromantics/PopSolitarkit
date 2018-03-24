using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {

	public bool PauseFirstCard = true;
	public InitialForce Template;
	bool DoneFirstCard = false;
	InitialForce FirstCard = null;


	void TriggerNextCard()
	{
		if (!PauseFirstCard)
			DoneFirstCard = true;

		if (!DoneFirstCard)
		{
			DoneFirstCard = true;
			FirstCard = CreateNewCard(false);
			return;
		}

		if (FirstCard != null)
		{
			FirstCard.enabled = true;
			FirstCard = null;
			return;
		}

		CreateNewCard(true);
	}

	InitialForce CreateNewCard(bool EnablePhysics)
	{
		var NewCard = GameObject.Instantiate(Template);
		NewCard.ForceAngle = Random.Range(-180, 180);
		NewCard.enabled = EnablePhysics;
		NewCard.gameObject.SetActive(true);
		return NewCard;
	}

	void OnEnable()
	{
		Template.gameObject.SetActive(false);
		TriggerNextCard();
	}

	void Update () {

		if ( Input.GetMouseButtonDown(0) )
		{
			TriggerNextCard();
		}

	}
}
