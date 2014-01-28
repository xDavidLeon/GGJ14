using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	bool toDie = false;
	public GameObject parentGO;
	public Vector3 direction;
	public AudioClip hitCell1, hitCell2;
	public AudioClip hitPlayer1, hitPlayer2, hitPlayer3;

	void Start () {
		StartCoroutine(DieTimer(7));

		Color c = parentGO.renderer.material.color;
		renderer.material.color = c;
		light.color = c;
		c.a = 0.5f;
		GetComponent<TrailRenderer>().material.SetColor("_TintColor", c);
	}

	void Update () {
		transform.LookAt(transform.position + rigidbody.velocity);
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

			int s = Random.Range(0,10);
			if (s < 3) audio.PlayOneShot(hitPlayer1);
			else if(s < 6) audio.PlayOneShot(hitPlayer2);
			else audio.PlayOneShot(hitPlayer3);

			StartCoroutine(p.Knockback(direction));
		}
		else if (c.gameObject.CompareTag("Cell"))
		{
			int s = Random.Range(0,10);
			if (s < 5) audio.PlayOneShot(hitCell1);
			else audio.PlayOneShot(hitCell2);

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
