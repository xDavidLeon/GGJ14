﻿using UnityEngine;
using System.Collections;

public class TimeGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = ((int) Level.instance.levelTime).ToString();
	}
}
