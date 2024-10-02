using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private bool isDead;
    private bool isGround;
    public bool isGrapplingJump;
    [SerializeField]
    private float jumpForce;
    public int jumpCount;

    private GameObject hook;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        hook = GameObject.Find("Hook");
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0)
            && jumpCount < 1)
        {
            jumpCount = 1;
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.AddForce(new Vector2(0, jumpForce));

            hook.gameObject.SetActive(false);
        }
        else if(isGrapplingJump)
        {
            Hookingjump();
        }

        if(isGround)
        {
            if (transform.position.x > -5.35f)
            {
                transform.Translate(Vector2.left * 0.005f * Time.timeScale);
            }
            if (transform.position.x < -5.35f)
            {
                transform.Translate(Vector2.right * 0.005f * Time.timeScale);
            }
        }
    }

    private void Die()
    {
        if(!isDead)
        {
            if (hook == null)
                return;
            myAnimator.SetTrigger("Die");
            myRigidbody.gravityScale = 0;
            myRigidbody.velocity = Vector2.zero;

            hook.gameObject.SetActive(false);
            isDead = true;
            GameManager.instance.GameOver();
        }
    }

    private void Hookingjump()
    {
        isGrapplingJump = false;
        myAnimator.ResetTrigger("Hooking");
        myAnimator.SetTrigger("HookJump");

        myRigidbody.gravityScale = 4;
        myRigidbody.velocity = Vector3.zero;

        myRigidbody.AddForce(new Vector2((transform.position.x) * 20f, jumpForce * 1.4f));
        
    }

    public void DieAnimation()
    {
        myRigidbody.gravityScale = 2f;
        //myRigidbody.AddForce(new Vector2(0, -500f));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Dead" && !isDead && !this.GetComponent<GrapplingHook>().isAttach)
        {
            Die();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGround = true;
            hook.gameObject.SetActive(true);
            myAnimator.ResetTrigger("Hooking");
            myAnimator.ResetTrigger("HookJump");
            myAnimator.SetBool("IsGround",true);
            isGrapplingJump = false;
            jumpCount = 0;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(jumpCount == 0)
        {
            Die();
        }
        isGround = false;
        myAnimator.SetBool("IsGround", false);
    }
}
