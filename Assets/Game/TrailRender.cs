using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRender : MonoBehaviour {

	List<Matrix4x4> TrailPositions;
	[Range(0.001f, 1)]
	public float MinSnapshotDistance = 1;

	public bool ShowDebug = false;
	public bool ShowTrail = true;

	Vector3 GetPosition(Matrix4x4 Matrix)
	{
		return Matrix.MultiplyPoint(Vector3.zero);
	}

	Vector3? GetLastPosition()
	{
		if (TrailPositions == null || TrailPositions.Count == 0)
			return null;
		var TailMtx = TrailPositions[TrailPositions.Count - 1];
		var TailPos = GetPosition(TailMtx);
		return TailPos;
	}

	void AddPosition(Matrix4x4 Pos)
	{
		if (TrailPositions == null)
			TrailPositions = new List<Matrix4x4>();
		TrailPositions.Add(Pos);
		//Debug.Log("added " + TrailPositions.Count + "th position");
	}

	void FixedUpdate () 
	{
		var NewMatrix = this.transform.localToWorldMatrix;
		var LastPosition = GetLastPosition();
		var NewPosition = GetPosition(NewMatrix);

		if (!LastPosition.HasValue)
		{
			AddPosition(NewMatrix);
		}
		else
		{
			var Distance = Vector3.Distance(LastPosition.Value, NewPosition);
			if (Distance > MinSnapshotDistance)
				AddPosition(NewMatrix);
		}

	}


	void Update()
	{
		DrawTrail();
	}

	void DrawTrail()
	{
		if (!ShowTrail)
			return;
		if (TrailPositions == null)
			return;
		
		var mf = GetComponent<MeshFilter>();
		var mesh = mf.sharedMesh;
		var mr = GetComponent<MeshRenderer>();
		var mat = mr.sharedMaterial;

		//	gr: limit to 1023
		//	when > 1000, add fricton so card stops
		Graphics.DrawMeshInstanced( mesh, 0, mat, TrailPositions.ToArray() );
	}


	void OnDrawGizmos()
	{
		if (TrailPositions == null)
			return;
		if (!ShowDebug)
			return;
		
		Gizmos.matrix = Matrix4x4.identity;
		Gizmos.color = Color.red;

		foreach ( var Mtx in TrailPositions )
		{
			var Pos = GetPosition(Mtx);
			Gizmos.DrawSphere( Pos, MinSnapshotDistance );
		}
	}
}
