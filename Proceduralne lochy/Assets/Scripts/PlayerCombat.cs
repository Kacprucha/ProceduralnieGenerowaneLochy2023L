using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    Transform attackPoint;

    [SerializeField]
    float attackRange = 0.5f;

    [SerializeField]
    LayerMask enemyLayer;

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll (attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyInfo> ().GetHit (this.GetComponent<PlayerInfo> ().Damage);
        }
    }

    private void OnDrawGizmosSelected ()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere (attackPoint.position, attackRange);
    }
}
