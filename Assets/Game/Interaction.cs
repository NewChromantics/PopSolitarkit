using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {

	public Transform SpawnParent { get { return Camera.main.transform; }}

	public Vector3 SpawnFromCameraPosition;
	public Quaternion SpawnFromCameraRotation;

	public InitialForce Template;
	bool DoneFirstCard = false;

	[Range(-180, 180)]
	public float RandomAngleDeviation = 5;

	void InitTemplate()
	{
		//	todo: handle when not direct decendent of camera
		if (Template.transform.parent != SpawnParent)
			throw new System.Exception("Expecting template to be child of spawn parent");

		SpawnFromCameraPosition = Template.transform.localPosition;
		SpawnFromCameraRotation = Template.transform.localRotation;
		Template.enabled = false;
		Template.gameObject.GetComponent<TrailRender>().enabled = false;
	}

	void TriggerFirstCard()
	{
		Template.gameObject.SetActive(false);
		TriggerNextCard();
	}

	void TriggerNextCard()
	{
		CreateNewCard();
	}

	InitialForce CreateNewCard()
	{
		bool EnablePhysics = true;
		var NewCard = GameObject.Instantiate(Template);
		NewCard.ForceAngle = 180 + Random.Range(-RandomAngleDeviation, RandomAngleDeviation);
		NewCard.enabled = EnablePhysics;
		NewCard.gameObject.GetComponent<TrailRender>().enabled = true;
		var SpawnPos = SpawnParent.localToWorldMatrix.MultiplyPoint(SpawnFromCameraPosition);
		var SpawnRot = SpawnParent.rotation * SpawnFromCameraRotation;
		NewCard.transform.position = SpawnPos;
		NewCard.transform.rotation = SpawnRot;
		NewCard.gameObject.SetActive(true);
		return NewCard;
	}

	void OnEnable()
	{
		InitTemplate();
	}

	void Update () {

		if ( Input.GetMouseButtonDown(0) )
		{
			if (!DoneFirstCard)
			{
				TriggerFirstCard();
			}
			else
			{
				TriggerNextCard();
			}
		}

	}
}
