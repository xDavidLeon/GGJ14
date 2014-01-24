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
		if (Input.GetKey(KeyCode.A))
		{
			desiredSpeed.x = -1 * maxSpeed;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			desiredSpeed.x = 1 * maxSpeed;
		}
		if (Input.GetKey(KeyCode.W))
		{
			desiredSpeed.z = 1 * maxSpeed;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			desiredSpeed.z = -1 * maxSpeed;
		}
		currentSpeed = currentSpeed * accelerationFactor + desiredSpeed * (1-accelerationFactor);
		controller.SimpleMove(currentSpeed);

		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 30.0f;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		Vector3 shootDir = mousePos - transform.position;
		shootDir.y = 0;
		shootDir.Normalize();
		Debug.DrawRay(transform.position,shootDir,Color.red);

		if (Input.GetMouseButtonDown(0))
		{
			GameObject b = GameObject.Instantiate(bullet, transform.position + shootDir, Quaternion.identity) as GameObject;
			//b.GetComponent<Bullet>().direction = shootDir;
			b.rigidbody.AddForce(shootDir * shootForce, ForceMode.Impulse);
			//Debug.Log(shootDir);
		}
	}
}
