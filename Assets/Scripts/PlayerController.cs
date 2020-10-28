using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rBody;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask isGrounded;
    public Transform circlePoint;
    public float radius;
    private bool groundedCheck;
    private bool isFacingRight = true;
    private Animator anim;

    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        //move
        float horiz = Input.GetAxisRaw("Horizontal");
        rBody.velocity = new Vector2(horiz * speed, rBody.velocity.y);

        //jump
        groundedCheck = Grounded();

        if (groundedCheck && Input.GetAxis("Jump") > 0)
        {
            rBody.AddForce(new Vector2(0, jumpForce));

        }

        if ((isFacingRight && rBody.velocity.x < 0) || (!isFacingRight && rBody.velocity.x > 0))
        {
            Flip();
        }

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        //    {
        //        anim.SetBool("composite", true);
        //    }
        //}

        anim.SetFloat("xVelocity", Math.Abs(horiz));
        anim.SetBool("jump", Input.GetAxis("Jump") > 0);
        anim.SetBool("isGrounded", groundedCheck);
        anim.SetFloat("yVelocity", Input.GetAxisRaw("Vertical"));

    }

    /// <summary>
    /// flip player
    /// </summary>
    private void Flip()
    {
        Vector3 temp = this.transform.localScale;
        temp.x *= -1;
        this.transform.localScale = temp;
        isFacingRight = !isFacingRight;
    }

    /// <summary>
    /// whether player is on the ground
    /// </summary>
    /// <returns></returns>
    bool Grounded()
    {
        return Physics2D.OverlapCircle(circlePoint.position, radius, isGrounded);
    }
}
