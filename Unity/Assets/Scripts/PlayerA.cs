using UnityEngine;
using System.Collections;

public class PlayerA : MonoBehaviour {
	public enum TEAM
	{
		BLUE = 1,
		RED = 2,
		GREEN = 3,
		YELLOW = 4
	};

	public float maxSpeed = 1;
	public float accelerationFactor = 0.9f;
	public Vector3 desiredSpeed = Vector3.zero;
	public Vector3 currentSpeed = Vector3.zero;
	Vector3 shootDir;

	CharacterController controller;

	public float shootForce = 10;
	public GameObject bullet;

	public TEAM team = TEAM.BLUE; 

	void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

	// Use this for initialization
	void Start () {
		controller.detectCollisions = true;
		
		switch (team)
		{
		case TEAM.BLUE:
			renderer.material.color = Color.blue;
			break;
		case TEAM.GREEN:
			renderer.material.color = Color.green;
			break;
		case TEAM.RED:
			renderer.material.color = Color.red;
			break;
		case TEAM.YELLOW:
			renderer.material.color = Color.yellow;
			break;
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		desiredSpeed = Vector3.zero;

	//MOVEMENT
		//CONTROLLER
		desiredSpeed.x = Input.GetAxis("C1_LeftX");
		desiredSpeed.z = Input.GetAxis("C1_LeftY");
		desiredSpeed.Normalize ();
		desiredSpeed *= maxSpeed;
		currentSpeed = currentSpeed * accelerationFactor + desiredSpeed * (1-accelerationFactor);
		controller.SimpleMove(currentSpeed);

	//SHOOT
		//CONTROLLER
		shootDir.x = Input.GetAxis ("C1_RightX");
		shootDir.z = -Input.GetAxis ("C1_RightY");
		shootDir.y = 0;
		shootDir.Normalize();

		if (Input.GetButtonDown("C1_RB"))
		{
			GameObject b = GameObject.Instantiate(bullet, transform.position + shootDir, Quaternion.identity) as GameObject;
			//b.GetComponent<Bullet>().direction = shootDir;
			b.rigidbody.AddForce(shootDir * shootForce, ForceMode.Impulse);
			//Debug.Log(shootDir);
		}

		CellA cell = Level.instance.GetCell(GetCellPos());
		if (IsSharingCell()) 
		{
			cell.Clear();
		}
		else 
		{
			cell.Step(team);
		}
	}

	void OnCollisionEnter(Collision c)
	{
		Debug.Log (this.gameObject.name + " - " + c.gameObject.name);
	}
	
	bool IsSharingCell()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		Vector2 pos = GetCellPos();
		foreach (GameObject g in players)
		{
			if (g == gameObject) continue;
			Vector2 pos2 = g.GetComponent<Player>().GetCellPos();
			if (pos.x == pos2.x && pos.y == pos2.y) 
			{
				Debug.Log ("Sharing cell " + pos.x + "," + pos.y);
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
