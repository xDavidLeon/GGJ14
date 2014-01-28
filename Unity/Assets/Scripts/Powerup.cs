using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {
	public enum POWERUP_TYPE
	{
		WALL = 0,
		SPEED = 1,
		EXPLOSION = 2,
		FREEZE = 3,
		SHOTGUN = 4
	};

	public POWERUP_TYPE power_type = POWERUP_TYPE.WALL;
	public GameObject mWall, mSpeed, mExplosion, mFreeze, mShotgun;

	// Use this for initialization
	void Start () {
		power_type = (POWERUP_TYPE) Random.Range(0,5);
		//power_type = POWERUP_TYPE.SHOTGUN;
		iTween.RotateBy(gameObject, iTween.Hash("x", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .0));
		Vector3 targetPos = transform.position;
		targetPos.y = 0.5f;
		iTween.MoveTo(gameObject, targetPos, 2.0f);

		switch (power_type) 
		{
		case POWERUP_TYPE.WALL:
			mWall.SetActive(true);
			break;
		case POWERUP_TYPE.SPEED:
			mSpeed.SetActive(true);
			
			break;
		case POWERUP_TYPE.EXPLOSION:
			mExplosion.SetActive(true);
			
			break;
		case POWERUP_TYPE.FREEZE:
			mFreeze.SetActive(true);
			
			break;
		case POWERUP_TYPE.SHOTGUN:
			mShotgun.SetActive(true);
			
			break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnTriggerEnter(Collider c)
	{
		//Debug.Log (c.gameObject.name);
		if (c.CompareTag("Player") == false) return;
		Player p = c.gameObject.GetComponent<Player> ();

		Level.instance.playPowerUpSound(power_type);
		
		switch (power_type) 
		{
			case POWERUP_TYPE.WALL:
				Level.instance.ActivateCells(c.gameObject.GetComponent<Player>().team);
				//Debug.Log("WALL!");
				break;
			case POWERUP_TYPE.SPEED:
				//if(p.speedUpActive) return; //Si ya tiene ese power activo no lo pilla
				p.speedPowerUpGet();
				//Debug.Log("SPEED!");
				break;
			case POWERUP_TYPE.EXPLOSION:
				Cell powerUpCell = Level.instance.GetCell((int)transform.position.x, (int)transform.position.z);
				Level.instance.explosion(powerUpCell.GetCellPos(), p.team);
				//Debug.Log("EXPLOSION!");
				break;
			case POWERUP_TYPE.FREEZE:
				Level.instance.freezeAllExcept(p.team);
				//Debug.Log("FREEZE!");
				break;
			case POWERUP_TYPE.SHOTGUN:
				//if(p.shotgunActive) return; //Si ya tiene ese power activo no lo pilla
				p.shotgunPowerUpGet();
				//Debug.Log("SHOTGUN!");
				break;
		}

		GameObject.Destroy(this.gameObject);
	}
}
