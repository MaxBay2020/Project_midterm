using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


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

    //UGUI
    public Text spaceText;
    public Image spaceImage;
    public Text wasdText;
    public Image wasdImage;
    public Text openText;

    //arrow
    public Transform arrow;

    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //UGUI tip
        wasdText.DOFade(0.3f, 0.8f).SetLoops(-1, LoopType.Yoyo);

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

    /// <summary>
    /// player exits zone, wasd tip is gone
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "wasdTipTrigger")
        {
            wasdText.DOKill();
            wasdText.DOFade(0, 0.8f);
            wasdImage.DOFade(0, 0.8f);
            StartCoroutine(VanishAfterSeconds(1f, wasdImage.gameObject));
        }

        if(collision.gameObject.tag == "spaceTipGone")
        {
            spaceText.DOKill();
            spaceText.DOFade(0, 0.8f);
            spaceImage.DOFade(0, 0.8f);
            StartCoroutine(VanishAfterSeconds(1f, spaceImage.gameObject));
        }
    }

    IEnumerator VanishAfterSeconds(float seconds,GameObject gameObject)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// play enters, space tip shows or arrow shows
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "spaceTipTrigger")
        {
            spaceImage.gameObject.SetActive(true);
            spaceText.gameObject.SetActive(true);
            spaceText.DOFade(0.3f, 0.8f).SetLoops(-1, LoopType.Yoyo);
        }
        if(collision.gameObject.tag== "arrowShowTrigger")
        {
            arrow.gameObject.SetActive(true);
        }
        if (collision.gameObject.tag == "chestTrigger")
        {
            openText.gameObject.SetActive(true);
            openText.DOFade(0.3f, 0.8f).SetLoops(-1, LoopType.Yoyo);

        }

    }

    /// <summary>
    /// player stays in zone, show tip: press E to open the chest
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "chestTrigger" && Input.GetKeyDown(KeyCode.E))
        {

            Debug.Log(123);
            openText.DOKill();
            openText.DOFade(0, 0.8f);
            StartCoroutine(VanishAfterSeconds(1f, openText.gameObject));

        }
    }
}
