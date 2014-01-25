using UnityEngine;
using System.Collections;

public class PlayerA : MonoBehaviour {
	public float maxSpeed = 1;
	public float accelerationFactor = 0.9f;
	public Vector3 desiredSpeed = Vector3.zero;
	public Vector3 currentSpeed = Vector3.zero;
	Vector3 shootDir;

	public bool A, B, X, Y, StartB, SelectB, LB, RB;
	public float LT, RT, padX, padY;

	CharacterController controller;

	public float shootForce = 10;
	public GameObject bullet;

	void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		desiredSpeed = Vector3.zero;

	//MOVEMENT
		//KEYBOARD
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

		//CONTROLLER
		desiredSpeed.x = Input.GetAxis("C1_LeftX");
		desiredSpeed.z = Input.GetAxis("C1_LeftY");

		A = Input.GetButton ("C1_A");
		B = Input.GetButton ("C1_B");
		X = Input.GetButton ("C1_X");
		Y = Input.GetButton ("C1_Y");
		LB = Input.GetButton ("C1_LB");
		RB = Input.GetButton ("C1_RB");
		StartB = Input.GetButton ("C1_Start");
		SelectB = Input.GetButton ("C1_Select");

		padX = Input.GetAxis("C1_DPadX");
		padY = Input.GetAxis("C1_DPadY");
		LT = Input.GetAxis ("C1_Triggers");



		desiredSpeed.Normalize ();
		desiredSpeed *= maxSpeed;
		currentSpeed = currentSpeed * accelerationFactor + desiredSpeed * (1-accelerationFactor);
		controller.SimpleMove(currentSpeed);

	//SHOOT
		//KEYBOARD
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 30.0f;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		shootDir = mousePos - transform.position;
		shootDir.y = 0;
		shootDir.Normalize();

		if (Input.GetMouseButtonDown(0))
		{
			GameObject b = GameObject.Instantiate(bullet, transform.position + shootDir, Quaternion.identity) as GameObject;
			//b.GetComponent<Bullet>().direction = shootDir;
			b.rigidbody.AddForce(shootDir * shootForce, ForceMode.Impulse);
			//Debug.Log(shootDir);
		}

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


		Debug.DrawRay(transform.position,shootDir,Color.red);
	}
}
