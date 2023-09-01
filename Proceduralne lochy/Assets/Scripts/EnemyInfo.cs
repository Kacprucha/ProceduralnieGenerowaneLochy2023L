using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    int health = 0;
    int damage = 0;
    int defence = 0;

    public void Inicialize (int h, int dmg, int def)
    {
        health = h;
        damage = dmg;
        defence = def;
    }

    public void GetHit (int attack)
    {
        int damage = (attack - ((attack * defence) / 100)) < 1 ? 1 : attack - ((attack * defence) / 100);
        health = health - damage;

        if (health <= 0)
            Destroy (this.gameObject);
    }

    public int DealedDameged ()
    {
        return damage;
    }
}
