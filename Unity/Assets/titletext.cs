using UnityEngine;
using System.Collections;

public class titletext : MonoBehaviour {
	int dir = 1;
	float size = 50;
	public float speed = 10;
	// Use this for initialization
	void Start () {
		//iTween.RotateBy(this.gameObject,
	}
	
	// Update is called once per frame
	void Update () {
		if (guiText.fontSize > 60) dir = -1;
		else if (guiText.fontSize < 40) dir = 1;

		size += dir * Time.deltaTime * speed;

		guiText.fontSize = (int) size;
	}
}
