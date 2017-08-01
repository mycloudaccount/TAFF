using UnityEngine;
using System.Collections;

public class RiverFlow : MonoBehaviour {
	
	public Vector2 range = new Vector2(0.1f, 1);
	public float speed = 1;
	public float riverSpeed = 0.1f;
	float[] randomTimes;
	Mesh mesh;
	Renderer rend;
	public Texture[] textures;
	public float changeInterval = 0.33F;

	void Start(){
		mesh = GetComponent<MeshFilter>().mesh;
		rend = GetComponent<Renderer>();

		int i = 0;
		randomTimes = new float[mesh.vertices.Length];

		while (i < mesh.vertices.Length) {
			randomTimes[i] = Random.Range(range.x, range.y);

			i++;
		}

	}

	void Update() {
		mesh = GetComponent<MeshFilter>().mesh;
		rend = GetComponent<Renderer>();

		Vector3[] vertices = mesh.vertices;
		//Vector3[] normals = mesh.normals;
		int i = 0;
		while (i < vertices.Length) {
			vertices[i].y = 1 * Mathf.PingPong(Time.time*speed, randomTimes[i]);
			i++;
		}
		mesh.vertices = vertices;

		float offsetLoc = Time.time * riverSpeed;
		Vector2 offset = new Vector2 (0, offsetLoc);
		///int index = Mathf.FloorToInt(Time.time / changeInterval);
		//index = index % textures.Length;
		//rend.material.mainTexture = textures[index];
		//rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));

		rend.material.mainTextureOffset = offset;﻿

	}
}