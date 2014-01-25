using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {
	public enum POWERUP_TYPE
	{
		WALL = 0,
		SPEED = 1,
		EXPLOSION = 2,
		FREEZE = 3
	};

	public POWERUP_TYPE power_type = POWERUP_TYPE.WALL;

	// Use this for initialization
	void Start () {
		power_type = (POWERUP_TYPE) Random.Range(0,4);

		iTween.RotateBy(gameObject, iTween.Hash("x", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .0));
		Vector3 targetPos = transform.position;
		targetPos.y = 0.5f;
		iTween.MoveTo(gameObject, targetPos, 2.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnTriggerEnter(Collider c)
	{
		Debug.Log (c.gameObject.name);
		if (c.CompareTag("Player") == false) return;
		Level.instance.ActivateCells(c.gameObject.GetComponent<Player>().team);
		GameObject.Destroy(this.gameObject);
	}
}
