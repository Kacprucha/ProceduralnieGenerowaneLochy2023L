using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;

    [SerializeField]
    public Rigidbody2D movingElement;

    [SerializeField]
    public PlayerCombat playerCombat;

    Vector2 mousePosition;
    Vector2 movment;

    void Update()
    {
        float moveX = Input.GetAxisRaw ("Horizontal");
        float moveY = Input.GetAxisRaw ("Vertical");

        if (Input.GetMouseButtonDown (0))
        {
            playerCombat.Attack ();
        }

        movment = new Vector2 (moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
    }

    private void FixedUpdate ()
    {
        //movingElement.MovePosition (movingElement.position + movment * moveSpeed * Time.fixedDeltaTime);
        movingElement.velocity = new Vector2 (movment.x * moveSpeed, movment.y * moveSpeed);

        Vector2 aimDirection = mousePosition - movingElement.position;
        float aimAngle = Mathf.Atan2 (aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        movingElement.rotation = aimAngle;
    }
}
