using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	bool toDie = false;
	public GameObject parentGO;
	public Vector3 direction;

	void Start () {
		StartCoroutine(DieTimer(7));

		Color c = parentGO.renderer.material.color;
		renderer.material.color = c;
		light.color = c;
		c.a = 0.5f;
		GetComponent<TrailRenderer>().material.SetColor("_TintColor", c);
	}

	void Update () {
	}

	void OnCollisionEnter(Collision c)
	{
		if (toDie) return;
		rigidbody.useGravity = true;
		toDie = true;
		StartCoroutine(DieTimer(3));

		if (c.gameObject.CompareTag("Player") && c.gameObject != this.parentGO)
		{
			Player p = c.gameObject.GetComponent<Player>();
			Debug.Log("BAM");
			StartCoroutine(p.Knockback(direction));
		}
		else if (c.gameObject.CompareTag("Cell"))
		{
			audio.Play();	
			//Player p = this.parentGO.GetComponent<Player>();
			Cell cell = c.gameObject.GetComponent<Cell>();
			//if (cell.cellTeam != p.team)
			//{
				cell.GetHit();
			//}
		}

	}

	IEnumerator DieTimer(float t)
	{
		for (float i = t; i > 0; i-= Time.deltaTime)
		{
			yield return 0;
		}

		GameObject.Destroy(this.gameObject);
	}

}
