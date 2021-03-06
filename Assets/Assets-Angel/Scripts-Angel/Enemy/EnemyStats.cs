using System;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    [Header("Health")]
    public float enemyHealth = 100f;

    [Header("Settings")]
    public float personalSpace;
    public float personalSpaceBack;

    [Header("Attack Settings")]
    public int enemyDamage;
    public float attackDelay;
    public float enemyVision;

  
}
