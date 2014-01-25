using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public enum INPUT
	{
		KEYBOARD1 = 0,
		KEYBOARD2 = 1,
		JOYA = 2,
		JOYB = 3
	};

	public INPUT input = INPUT.KEYBOARD1;

	public float maxSpeed = 1;
	public float accelerationFactor = 0.9f;
	public Vector3 desiredSpeed = Vector3.zero;
	public Vector3 currentSpeed = Vector3.zero;
	public bool knocked = false;
	public Vector3 knockedDirection = Vector3.zero;

	public AudioClip shoot1, shoot2;

	CharacterController controller;

	public float shootForce = 10;
	public GameObject bullet;
	private float bulletTimer = 0;
	public float bulletMaxTimer = 0.5f;
	public GameObject shootHolder;

	public Level.TEAM team = Level.TEAM.BLUE; 

	void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

	void Start () 
	{
		controller.detectCollisions = true;

		switch (team)
		{
		case Level.TEAM.BLUE:
			renderer.material.color = new Color(194.0f/255.0f,250.0f/255.0f,255.0f/255.0f);
			light.color = new Color(194.0f/255.0f,250.0f/255.0f,255.0f/255.0f);
			break;
		case Level.TEAM.GREEN:
			renderer.material.color = Color.green;
			light.color = Color.green;
			break;
		case Level.TEAM.RED:
			renderer.material.color = Color.red;
			light.color = Color.red;
			break;
		case Level.TEAM.YELLOW:
			renderer.material.color = new Color(251.0f/255.0f,239.0f/255.0f,96.0f/255.0f);
			light.color = new Color(251.0f/255.0f,239.0f/255.0f,96.0f/255.0f);
			break;

		}
	}

	void Update () 
	{
		if (Level.instance.gameOver) 
		{
			controller.SimpleMove(Vector3.zero);
			return;
		}

		if (knocked) 
			controller.SimpleMove(knockedDirection*maxSpeed*2);
		else UpdateInput();

		Debug.DrawRay(transform.position,Vector3.forward);

		if(bulletTimer<bulletMaxTimer) bulletTimer+=Time.deltaTime;

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

	void UpdateInput()
	{
		desiredSpeed = Vector3.zero;

		switch (input)
		{
		case INPUT.KEYBOARD1:
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
			transform.LookAt(transform.position + currentSpeed);

			if (Input.GetKey(KeyCode.F))
			{
				ShootKeyboard ();
			}

			break;
		case INPUT.KEYBOARD2:
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				desiredSpeed.x = -1;
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				desiredSpeed.x = 1;
			}
			if (Input.GetKey(KeyCode.UpArrow))
			{
				desiredSpeed.z = 1;
			}
			else if (Input.GetKey(KeyCode.DownArrow))
			{
				desiredSpeed.z = -1;
			}

			desiredSpeed.Normalize();
			desiredSpeed *= maxSpeed;
			currentSpeed = currentSpeed * accelerationFactor + desiredSpeed * (1-accelerationFactor);
			controller.SimpleMove(currentSpeed);
			transform.LookAt(transform.position + currentSpeed);

			if (Input.GetKey(KeyCode.RightShift))
			{
				ShootKeyboard ();
			}
			break;
			
		case INPUT.JOYA:
			desiredSpeed.x = Input.GetAxis("C1_LeftX");
			desiredSpeed.z = Input.GetAxis("C1_LeftY");

			desiredSpeed.Normalize();
			desiredSpeed *= maxSpeed;
			currentSpeed = currentSpeed * accelerationFactor + desiredSpeed * (1-accelerationFactor);
			controller.SimpleMove(currentSpeed);
			transform.LookAt(transform.position + currentSpeed);

			if (Input.GetAxis("C1_Triggers") < 0) ShootJoystick ();
			break;
		case INPUT.JOYB:
			desiredSpeed.x = Input.GetAxis("C2_LeftX");
			desiredSpeed.z = Input.GetAxis("C2_LeftY");

			desiredSpeed.Normalize();
			desiredSpeed *= maxSpeed;
			currentSpeed = currentSpeed * accelerationFactor + desiredSpeed * (1-accelerationFactor);
			controller.SimpleMove(currentSpeed);
			transform.LookAt(transform.position + currentSpeed);

			if (Input.GetAxis("C2_Triggers") < 0) ShootJoystick ();
			break;
		}


	}

	void OnCollisionEnter(Collision c)
	{
		//Debug.Log (this.gameObject.name + " - " + c.gameObject.name);
	}

	void ShootKeyboard()
	{
		if (bulletTimer < bulletMaxTimer) return;
		bulletTimer = 0;

		Vector3 bulletPos = gameObject.transform.localPosition + gameObject.transform.forward;
		GameObject b = GameObject.Instantiate(bullet, bulletPos, Quaternion.identity) as GameObject;
		//b.GetComponent<Bullet>().direction = shootDir;
		Vector3 bulletDir = transform.forward * shootForce;
		b.GetComponent<Bullet>().parentGO = this.gameObject;
		b.rigidbody.AddForce(bulletDir, ForceMode.Impulse);
		bulletDir.Normalize();
		b.GetComponent<Bullet>().direction = bulletDir;

		int s = Random.Range(0,10);
		if (s < 5) audio.PlayOneShot(shoot1);
		else audio.PlayOneShot(shoot2);
	}

	void ShootJoystick()
	{
		if (bulletTimer < bulletMaxTimer) return;
		bulletTimer = 0;
		Vector3 bulletDir = Vector3.zero;
		if (this.input == INPUT.JOYA) bulletDir = new Vector3(Input.GetAxis("C1_RightX"),0,-Input.GetAxis("C1_RightY"));
		else if (this.input == INPUT.JOYB) bulletDir = new Vector3(Input.GetAxis("C2_RightX"),0,-Input.GetAxis("C2_RightY"));
		bulletDir.Normalize();
		
		Vector3 bulletPos = transform.position + bulletDir;
		bulletPos.y = transform.position.y;
		GameObject b = GameObject.Instantiate(bullet, bulletPos, Quaternion.identity) as GameObject;
		//b.GetComponent<Bullet>().direction = shootDir;
		b.GetComponent<Bullet>().parentGO = this.gameObject;
		b.rigidbody.AddForce(bulletDir*shootForce, ForceMode.Impulse);
		b.GetComponent<Bullet>().direction = bulletDir;

		int s = Random.Range(0,10);
		if (s < 5) audio.PlayOneShot(shoot1);
		else audio.PlayOneShot(shoot2);
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

	public IEnumerator Knockback(Vector3 direction)
	{
		knocked = true;
		knockedDirection = direction;
		float startTime = Time.time;
		while(Time.time < (startTime + 0.5f)){
			//controller.SimpleMove(direction*maxSpeed*2);
			yield return 0;
		}
		knocked = false;
	}

}
