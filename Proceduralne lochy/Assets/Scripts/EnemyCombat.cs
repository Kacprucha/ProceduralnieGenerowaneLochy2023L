using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    Transform attackPoint = null;

    float attackRange = 1f;

    LayerMask enemyLayer;

    float attackRate = 2f;
    float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (this.GetComponent<EnemyMovment> ().IsChaising ())
            {
                Attack ();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    public void Inicilize (Transform trans, LayerMask layer, float attRat, float range = 1f)
    {
        attackPoint = trans;
        enemyLayer = layer;
        attackRate = attRat;
        attackRange = range; 
    }

    public void Attack ()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll (attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D player in hitEnemies)
        {
            int attack = this.GetComponent<EnemyInfo> ().DealedDameged ();
            int defence = player.GetComponent<PlayerInfo> ().Defence;
            int damage = (attack - defence) < 1 ? 1 : attack - defence;
            player.GetComponent<PlayerInfo> ().CheangeHealth (-damage);
        }
    }

    private void OnDrawGizmosSelected ()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere (attackPoint.position, attackRange);
    }
}
