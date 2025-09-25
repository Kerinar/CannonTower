using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

public class CannonTower : MonoBehaviour {
	public float m_shootInterval = 0.5f;
	public float m_range = 4f;
	public GameObject m_projectilePrefab;
	public Transform m_shootPoint;

	private float m_lastShotTime = -0.5f;

	[SerializeField] private float rotationSpeed = 2f;

	[SerializeField] private bool parabolicMode = false;
	private bool isPreviousModeParabolic = false;

	void Update () {
		if (m_projectilePrefab == null || m_shootPoint == null)
			return;

		foreach (var monster in FindObjectsOfType<Monster>()) {
			if (Vector3.Distance (transform.position, monster.transform.position) > m_range)
				continue;

			//Обычный режим стрельбы
			if (parabolicMode == false)
			{
				isPreviousModeParabolic = false;
            }
			//Переключение на режим стрельбы по параболе
			if (parabolicMode == true && isPreviousModeParabolic == false)
			{
				transform.rotation = new Quaternion(0, 0, 0, 0);
				isPreviousModeParabolic = true;
			}

            var monsterToCannon = transform.position - monster.transform.position;

            if (parabolicMode == true)
			{
				monsterToCannon.y = 0;
            }

			//Рассчет упреждения без учета угла
            var monsterDistance = monsterToCannon.magnitude;
            var projectileSpeed = m_projectilePrefab.GetComponent<CannonProjectile>().m_speed;
            var marginValue = monster.m_speed * (monsterDistance / projectileSpeed);

			//Рассчет угла для упреждения
            var monsterDestination = monster.m_moveTarget.transform.position - monster.transform.position;

            var angle = Vector3.Angle(monsterToCannon, monsterDestination);
            var angleRad = angle * Mathf.Deg2Rad;
            var angleSin = Mathf.Sin(angleRad);

            marginValue = marginValue * angleSin;

			//Рассчет точки упреждения относительно пушки
            var lookAtMonster = monster.transform.position - transform.position;
            var shootPoint = monsterDestination.normalized * marginValue;
            shootPoint = shootPoint + lookAtMonster;

            if (parabolicMode == true)
            {
                shootPoint.y = 0;
            }

			//Поворот пушки
            var targetRotation = Quaternion.LookRotation(shootPoint);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (m_lastShotTime + m_shootInterval > Time.time)
				continue;

			//Выстрел
			var projectile = Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);

			//Задаем режим стрельбы для снарядов
			if(parabolicMode == true)
			{
				projectile.GetComponent<CannonProjectile>().parabolicMode = true;
			}

			m_lastShotTime = Time.time;
		}

	}
}
