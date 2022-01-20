using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats")]
public class Stats : ScriptableObject
{
    [SerializeField] private int damage;
    [SerializeField] private int maxHealth;
    [SerializeField] private List<GameObject> wayPoints;

    public int Damage()
    {
        return damage;
    }
    public int MaxHealth()
    {
        return maxHealth;
    }
    public Vector2 WayPoints(int size)
    {
        return wayPoints[size].transform.position;
    }
}