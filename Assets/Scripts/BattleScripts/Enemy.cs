using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Enemy : ScriptableObject
{
    // Start is called before the first frame update
    public int health;

    public Sprite enemySprite;
    public GameObject[] enemiesAttacks;
}
