using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class EnemyManager {

    [HideInInspector]public GameObject m_Instane;

    public Transform m_SpawnPoint;

    public void DisableEnemy()
    {
        m_Instane.SetActive(false);
    }


    public void EnableEnemy()
    {
        m_Instane.SetActive(true);
    }


    public void SpawnEnemy()
    {
        m_Instane.transform.position = m_SpawnPoint.position;
        m_Instane.transform.rotation = m_SpawnPoint.rotation;
    }
}
