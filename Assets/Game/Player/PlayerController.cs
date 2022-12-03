using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float minimalAcceptableVelocity = 0.01f;

    [SerializeField] private bool shouldTurn = true;

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [Space(10)]
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [Space(5)]
    [SerializeField, Range(0,1f)] private float inertia;
    [SerializeField] private float friction;
    
    [HideInInspector] public bool stopMovement = false;
    private Rigidbody2D physicalBody => GetComponent<Rigidbody2D>();

    private Vector2 movementDirection, frictionAmount;
    private Vector2 targetSpeed, actualForce, delta;
    private bool isRight = true;


    void Update()
    {
        movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        targetSpeed = movementDirection * (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);
        #region Acceleration&Deceleration
        delta = targetSpeed - physicalBody.velocity;
        Vector2 accelerationRate = new Vector2(
            Mathf.Abs(targetSpeed.x) >= minimalAcceptableVelocity ? acceleration : deceleration,
            Mathf.Abs(targetSpeed.y) >= minimalAcceptableVelocity ? acceleration : deceleration);
        actualForce = new Vector2(
            Mathf.Pow(Mathf.Abs(delta.x) * accelerationRate.x, inertia) * Mathf.Sign(delta.x),
            Mathf.Pow(Mathf.Abs(delta.y) * accelerationRate.y, inertia) * Mathf.Sign(delta.y));
        #endregion
        #region Friction
        float frictionX = 0, frictionY = 0;
        if (Mathf.Abs(movementDirection.x) <= minimalAcceptableVelocity) {
            frictionX = Mathf.Min(Mathf.Abs(physicalBody.velocity.x), Mathf.Abs(friction));
        }
        if (Mathf.Abs(movementDirection.y) <= minimalAcceptableVelocity)
        {
            frictionY = Mathf.Min(Mathf.Abs(physicalBody.velocity.y), Mathf.Abs(friction));
        }
        frictionAmount = new Vector2(frictionX, frictionY) * physicalBody.velocity.normalized;
        #endregion
        Player.inst.animator.AnimateWalk(movementDirection);
        if (!shouldTurn) return;
        TurnSelf();
    }


    private void TurnSelf()
    {
        if(Input.GetKeyDown(KeyCode.D) && !isRight || Input.GetKeyDown(KeyCode.A) && isRight)
        {
            isRight = !isRight;
            Vector2 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }


    private void FixedUpdate()
    {
        physicalBody.AddForce(actualForce, ForceMode2D.Force);
        physicalBody.AddForce(-frictionAmount, ForceMode2D.Impulse);
    }
}
