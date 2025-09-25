using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public float m_interval = 3;
	public GameObject m_moveTarget;
	public FlyingShield m_flyingShieldPrefab;

	private float m_lastSpawn = -1;

	[SerializeField] private GameObject monsterPrefab;

	void Update () {
		if (Time.time > m_lastSpawn + m_interval) {
			//Создание монстра из префаба
			var monster = Instantiate(monsterPrefab);
			monster.transform.position = transform.position;
			monster.GetComponent<Monster>().m_moveTarget = m_moveTarget;

			
			if (m_flyingShieldPrefab != null)
				Instantiate(m_flyingShieldPrefab, monster.transform);
			
			m_lastSpawn = Time.time;
		}
	}
}
