using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	//public float velocity = 10;
	//public Vector3 direction;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		//transform.position += direction * velocity * Time.deltaTime;
	}

	void OnCollisionEnter(Collision c)
	{
		Debug.Log ("BAM");
		rigidbody.useGravity = true;
		//Debug.Log(c.gameObject.name);
	}

}
