using UnityEngine;
using System.Collections;

public class CannonProjectile : MonoBehaviour {
	public float m_speed = 10f;
	public float acceleration = 15f;
	public int m_damage = 10;

	private float destroyTime = 0f;

	public bool parabolicMode = false;

	private Vector3 downSpeed = Vector3.zero;

	void Update () {
		var translation = transform.forward * m_speed * Time.deltaTime;

		//Движение снаряда по параболе
		if (parabolicMode == true)
		{
			downSpeed += Vector3.down * (acceleration * Time.deltaTime * Time.deltaTime) / 2;
            translation += downSpeed;

		}

		transform.Translate (translation);

		//Уничтожение снаряда через время
		if(destroyTime >= 4f)
		{
			Destroy (gameObject);
		}

		destroyTime += Time.deltaTime;
	}

	void OnTriggerEnter(Collider other) {
		var monster = other.gameObject.GetComponent<Monster> ();
		if (monster == null)
			return;

		monster.m_hp -= m_damage;
		if (monster.m_hp <= 0) {
			Destroy (monster.gameObject);
		}
		Destroy (gameObject);
	}
}
