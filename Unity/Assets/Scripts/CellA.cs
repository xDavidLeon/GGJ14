using UnityEngine;
using System.Collections;

public class CellA : MonoBehaviour {

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

	public void Step(PlayerA.TEAM team)
	{
		switch (team)
		{
		case PlayerA.TEAM.BLUE:
			renderer.material.color = Color.blue;
			break;
		case PlayerA.TEAM.GREEN:
			renderer.material.color = Color.green;
			break;
		case PlayerA.TEAM.RED:
			renderer.material.color = Color.red;
			break;
		case PlayerA.TEAM.YELLOW:
			renderer.material.color = Color.yellow;
			break;
		}
	}

	public void Clear()
	{
		renderer.material.color = Color.white;
	}


}
