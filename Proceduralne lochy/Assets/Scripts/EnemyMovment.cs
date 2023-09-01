using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovment : MonoBehaviour
{
    protected Transform target;
    protected Vector2 moveDirection;
    protected float speed= 2;

    protected bool goChaise = false;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find ("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target && goChaise)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg - 180f;
            GetComponent<Rigidbody2D> ().rotation = angle;
            moveDirection = direction;
        }
    }

    private void FixedUpdate ()
    {
        if (target && goChaise)
        {
            GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveDirection.x, moveDirection.y) * speed;
        }
    }

    public void InicializeChase ()
    {
        goChaise = true;
    }

    public bool IsChaising ()
    {
        return goChaise;
    }

    public void ChangeSpeed (float s)
    {
        speed = s;
    }
}
