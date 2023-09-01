using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    [SerializeField]
    protected List<EnemyMovment> enemies = new List<EnemyMovment> ();

    int hight;
    int wight;
    int area;

    public void Setup (int w, int h, TileType type)
    {
        wight = w;
        hight = h;
        area = w * h;

        int ammountOfEnemies = area / 20;
        int random;
        float x = this.GetComponent<RectTransform> ().position.x, y = this.GetComponent<RectTransform> ().position.y, enemyX, enemyY, attackRate;

        EnemySpriteContener enemyHelper = GameObject.Find ("EnemySpriteContener").GetComponent<EnemySpriteContener> ();

        if (type == TileType.Normal)
        {
            for (int i = 0; i < ammountOfEnemies; i++)
            {
                GameObject enemy = new GameObject ();
                enemy.name = "Enemy";
                enemy.layer = 7;
                enemy.AddComponent<SpriteRenderer> ();
                enemy.AddComponent<EnemyInfo> ();

                random = Random.Range (1, 100);

                if (random <= 30)
                {
                    enemy.GetComponent<SpriteRenderer> ().sprite = enemyHelper.GetVampireSprite ();
                    enemy.GetComponent<EnemyInfo> ().Inicialize (60, 30, 10);
                    attackRate = 2f;
                }
                else
                {
                    enemy.GetComponent<SpriteRenderer> ().sprite = enemyHelper.GetSkeletonSprite ();
                    enemy.GetComponent<EnemyInfo> ().Inicialize (15, 10, 0);
                    attackRate = 0.5f;
                }

                enemy.GetComponent<SpriteRenderer> ().sortingOrder = 1;
                enemy.AddComponent<Rigidbody2D> ();
                enemy.GetComponent<Rigidbody2D> ().gravityScale = 0;
                enemy.AddComponent<BoxCollider2D> ();

                enemyX = Random.Range (x - (wight / 2) + 2, x + (wight / 2) - 2);
                enemyY = Random.Range (y - (hight / 2) + 2, y + (hight / 2) - 2);

                enemy.transform.position = new Vector2 (enemyX, enemyY);

                enemy.AddComponent<EnemyMovment> ();
                enemies.Add (enemy.GetComponent<EnemyMovment> ());

                enemy.AddComponent<EnemyCombat> ();
                enemy.GetComponent<EnemyCombat> ().Inicilize (enemy.transform, enemyHelper.PlayerLayer, attackRate);
            }
        }
        else if (type == TileType.Boss)
        {
            GameObject boss = new GameObject ();
            boss.name = "Boss";
            boss.layer = 7;
            boss.AddComponent<SpriteRenderer> ();
            boss.AddComponent<EnemyInfo> ();

            random = Random.Range (1, 100);

            if (random <= 30)
            {
                boss.GetComponent<SpriteRenderer> ().sprite = enemyHelper.GetVampireSprite ();
            }
            else
            {
                boss.GetComponent<SpriteRenderer> ().sprite = enemyHelper.GetSkeletonSprite ();
            }

            boss.GetComponent<SpriteRenderer> ().sortingOrder = 1;
            boss.AddComponent<Rigidbody2D> ();
            boss.GetComponent<Rigidbody2D> ().gravityScale = 0;
            boss.AddComponent<BoxCollider2D> ();

            boss.transform.localScale = new Vector2 (5, 5);
            boss.transform.position = new Vector2 (x, y);

            boss.GetComponent<EnemyInfo> ().Inicialize (120, 60, 5);

            boss.AddComponent<EnemyMovment> ();
            boss.AddComponent<EnemyMovment> ().ChangeSpeed (0.5f);
            enemies.Add (boss.GetComponent<EnemyMovment> ());

            attackRate = 0.4f;
            boss.AddComponent<EnemyCombat> ();
            boss.GetComponent<EnemyCombat> ().Inicilize (boss.transform, enemyHelper.PlayerLayer, attackRate, 5);
        }
    }

    public void AnableEmemies ()
    {
        foreach (EnemyMovment em in enemies)
            em.InicializeChase ();

        Destroy (this.gameObject);
    }
}
