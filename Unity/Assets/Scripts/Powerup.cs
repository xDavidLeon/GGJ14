using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.RotateBy(gameObject, iTween.Hash("x", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .0));
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnTriggerEnter(Collider c)
	{
		Debug.Log (c.gameObject.name);
		Level.instance.ActivateCells(c.gameObject.GetComponent<Player>().team);
		GameObject.Destroy(this.gameObject);
	}
}
