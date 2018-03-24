using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

	public List<Texture> CardFaces;
	public string CardFaceUniform = "CardFront";

	void OnEnable()
	{
		var CardFace = CardFaces[Random.Range(0, CardFaces.Count)];
		var mr = GetComponent<MeshRenderer>();
		var mat = mr.material;
		mat.SetTexture(CardFaceUniform,CardFace);
	}

}
