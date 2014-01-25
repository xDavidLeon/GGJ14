using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnCollisionEnter(Collision c)
	{
		Debug.Log ("Ouch! " + this.gameObject.name + " - " + c.gameObject.name);
	}

	public void Step(Player.TEAM team)
	{
		switch (team)
		{
		case Player.TEAM.BLUE:
			renderer.material.color = Color.blue;
			break;
		case Player.TEAM.GREEN:
			renderer.material.color = Color.green;
			break;
		case Player.TEAM.RED:
			renderer.material.color = Color.red;
			break;
		case Player.TEAM.YELLOW:
			renderer.material.color = Color.yellow;
			break;
		}
	}

	public void Clear()
	{
		renderer.material.color = Color.white;
	}


}
