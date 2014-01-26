using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
	public Level.TEAM cellTeam = Level.TEAM.NONE;
	public bool isActivated = false;
	public int life = 0;
	public int maxLife = 1;

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
	}

	public void Step(Level.TEAM team)
	{
		if (isActivated) return;
		cellTeam = team;
		Color c = Color.white;
		switch (team)
		{
		case Level.TEAM.BLUE:
			c = new Color(0.0f/255.0f,76.0f/255.0f,133.0f/255.0f);
			renderer.material.SetColor("_DetailColor", c);
			particleSystem.startColor = c;
			break;
		case Level.TEAM.GREEN:
			c = new Color(39.0f/255.0f,105.0f/255.0f,22.0f/255.0f);
			renderer.material.SetColor("_DetailColor", c);
			particleSystem.startColor = c;
			break;
		case Level.TEAM.RED:
			c = new Color(98.0f/255.0f,14.0f/255.0f,10.0f/255.0f);
			renderer.material.SetColor("_DetailColor", c);
			particleSystem.startColor = c;
			break;
		case Level.TEAM.YELLOW:
			c = new Color(128.0f/255.0f,115.0f/255.0f,44.0f/255.0f);
			renderer.material.SetColor("_DetailColor", c);
			particleSystem.startColor = c;
			break;
		}
	}

	public void Clear()
	{
		if (isActivated) return;
		cellTeam = Level.TEAM.NONE;
		renderer.material.SetColor("_DetailColor", Color.white);
	}

	public bool ActivateCell()
	{
		if (isActivated) return false;
		particleSystem.Play();
		life = maxLife;
		if (HasPlayerOnTop()) return false;
		isActivated = true;
		Vector3 targetPos = transform.position;
		targetPos.y = 0.5f;
		iTween.MoveTo (this.gameObject, targetPos , 2.0f);
		return true;
	}

	public void GetHit()
	{
		life -= 1;
		if (life <= 0)
		{
			Restart(false);
		}
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

	public void Restart(bool restartColor)
	{
		if (isActivated) 
		{
			particleSystem.Stop();
			Vector3 targetPos = transform.position;
			targetPos.y = -0.5f;
			iTween.MoveTo(this.gameObject,targetPos,0.25f);
		}
		if (restartColor)
		{
			cellTeam = Level.TEAM.NONE;
			renderer.material.SetColor("_DetailColor", Color.white);
		}
		isActivated = false;
	}
}
