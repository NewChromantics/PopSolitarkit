using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialForce : MonoBehaviour {

	[Range(0, 9.8f)]
	public float GravityForce = 9.8f;

	public static Vector2 AngleRadianToVector2(float radian, float Length = 1)
	{
		return new Vector2(Mathf.Sin(radian) * Length, Mathf.Cos(radian) * Length);
	}

	public static Vector2 AngleDegreeToVector2(float degree, float Length = 1)
	{
		return AngleRadianToVector2(degree * Mathf.Deg2Rad, Length);
	}


	[Range(-180, 180)]
	public float ForceAngle = 0;
	[Range(0, 10)]
	public float ForceStrengthMin = 1;
	[Range(0, 10)]
	public float ForceStrengthMax = 1;
	public float ForceStrength { get { return Random.Range(ForceStrengthMin, ForceStrengthMax); } }

	[Range(0, 10)]
	public float VerticalForceStrengthMin = 1;
	[Range(0, 10)]
	public float VerticalForceStrengthMax = 1;
	public float VerticalForceStrength { get { return Random.Range(VerticalForceStrengthMin, VerticalForceStrengthMax); } }


	public ForceMode AddForceMode;

	public bool AddOnce = false;

	[Range(0, 10)]
	public float GizmoSize = 1;

	public Color GizmoColour = Color.red;

	public Vector3		LocalForceVector
	{
		get
		{
			var Dir2 = AngleDegreeToVector2(ForceAngle, 1);
			return new Vector3(Dir2.x*ForceStrength, 0, Dir2.y*ForceStrength);
		}
	}
	public Vector3 WorldForceVector
	{
		get
		{
			var LocalForce = LocalForceVector;
			var LocalToWorld = Matrix4x4.TRS(this.transform.position, this.transform.rotation, Vector3.one);
			//var LocalToWorld = this.transform.localToWorldMatrix;
			var WorldForce = LocalToWorld.MultiplyVector(LocalForce);
			WorldForce.y = VerticalForceStrength;
			return WorldForce;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.matrix = Matrix4x4.identity;
		Gizmos.color = GizmoColour;

		var Size = GizmoSize ;//* this.transform.localScale.magnitude;
		var VectorPos = this.transform.position + WorldForceVector;

		Gizmos.DrawWireSphere(VectorPos, Size);
		Gizmos.DrawLine(this.transform.position, VectorPos);
	}

	void OnEnable()
	{
		Debug.Log(Physics.gravity);
		Physics.gravity = new Vector3(0, -GravityForce, 0);

		var rb = GetComponent<Rigidbody>();
		//rb.AddForce(WorldForceVector * Time.fixedDeltaTime, AddForceMode);
		rb.isKinematic = false;
		rb.AddForce(WorldForceVector, AddForceMode);
	}

	void Update()
	{
		if (!AddOnce)
		{
			OnEnable();
		}
	}

}
