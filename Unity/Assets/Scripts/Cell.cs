using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
	public Level.TEAM cellTeam = Level.TEAM.NONE;
	public bool isActivated = false;

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

	public void Step(Level.TEAM team)
	{
		cellTeam = team;
		switch (team)
		{
		case Level.TEAM.BLUE:
			renderer.material.color = Color.blue;
			break;
		case Level.TEAM.GREEN:
			renderer.material.color = Color.green;
			break;
		case Level.TEAM.RED:
			renderer.material.color = Color.red;
			break;
		case Level.TEAM.YELLOW:
			renderer.material.color = Color.yellow;
			break;
		}
	}

	public void Clear()
	{
		cellTeam = Level.TEAM.NONE;
		renderer.material.color = Color.white;
	}

	public bool ActivateCell()
	{
		if (isActivated) return false;
		if (HasPlayerOnTop()) return false;
		isActivated = true;
		iTween.MoveAdd(this.gameObject,new Vector3(0,1,0),2.0f);
		return true;
	}

	public bool HasPlayerOnTop()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		Vector2 pos = GetCellPos();
		foreach (GameObject g in players)
		{
			Vector2 pos2 = g.GetComponent<Player>().GetCellPos();
			if (pos.x == pos2.x && pos.y == pos2.y) 
			{
				return true;
			}
		}
		return false;
	}

	public Vector2 GetCellPos()
	{
		return new Vector2((int)transform.position.z,(int)transform.position.x);
	}
}
