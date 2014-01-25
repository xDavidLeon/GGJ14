using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float maxSpeed = 1;
	public float accelerationFactor = 0.9f;
	public Vector3 desiredSpeed = Vector3.zero;
	public Vector3 currentSpeed = Vector3.zero;

	CharacterController controller;

	public float shootForce = 10;
	public GameObject bullet;

	public Level.TEAM team = Level.TEAM.BLUE; 

	void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

	// Use this for initialization
	void Start () 
	{
		controller.detectCollisions = true;

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
	
	// Update is called once per frame
	void Update () {
		desiredSpeed = Vector3.zero;
		
		if (Input.GetKey(KeyCode.A))
		{
			desiredSpeed.x = -1;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			desiredSpeed.x = 1;
		}
		if (Input.GetKey(KeyCode.W))
		{
			desiredSpeed.z = 1;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			desiredSpeed.z = -1;
		}
		desiredSpeed.Normalize();
		desiredSpeed *= maxSpeed;
		currentSpeed = currentSpeed * accelerationFactor + desiredSpeed * (1-accelerationFactor);
		controller.SimpleMove(currentSpeed);

//		Vector3 mousePos = Input.mousePosition;
//		mousePos.z = 30.0f;
//		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
//		Vector3 shootDir = mousePos - transform.position;
//		shootDir.y = 0;
//		shootDir.Normalize();
//		Debug.DrawRay(transform.position,shootDir,Color.red);

//		if (Input.GetMouseButtonDown(0))
//		{
//			GameObject b = GameObject.Instantiate(bullet, transform.position + shootDir, Quaternion.identity) as GameObject;
//			//b.GetComponent<Bullet>().direction = shootDir;
//			b.rigidbody.AddForce(shootDir * shootForce, ForceMode.Impulse);
//
//			//Debug.Log(shootDir);
//		}

//		RaycastHit hit;
//		if (Physics.Raycast(transform.position, Vector3.down, out hit))
//		{
//			Debug.Log ("RAY - " + gameObject.name + " " + hit.collider.name);
//			Cell c = hit.collider.gameObject.GetComponent<Cell>();
//			c.Step(team);
//		}

		Cell cell = Level.instance.GetCell(GetCellPos());
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
