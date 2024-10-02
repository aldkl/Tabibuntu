using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public LineRenderer line;
    public Transform hook;
    public Transform HookGoPos;
    public Vector3 StartHookPos;

    private bool isHookActive;
    private bool isLineMax;
    public bool isAttach;

    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.01f;
        line.SetPosition(0, transform.position + StartHookPos);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
        isHookActive = false;
        isAttach = false;
        playerController = gameObject.GetComponent<PlayerController>();
    }
    private void Update()
    {
        line.SetPosition(0, transform.position + StartHookPos);
        line.SetPosition(1, hook.position);
        if(!isHookActive && !isAttach)
        {
            hook.position = transform.position + StartHookPos;
        }
        if(GameManager.instance.isGameover)
        {
            return; 
        }
        if (Input.GetMouseButtonDown(1) && playerController.jumpCount == 1 && !isHookActive)
        {
            hook.position = transform.position + StartHookPos;
            isHookActive = true;
            isLineMax = false;
            hook.gameObject.SetActive(true);
            hook.GetComponent<AudioSource>().Play();
        }
        if (isHookActive && !isLineMax && !isAttach)
        {
            hook.Translate(HookGoPos.localPosition.normalized * Time.deltaTime * 20);

            if (Vector2.Distance(transform.position, hook.position) > 7f)
            {
                isLineMax = true;
            }
        }
        else if (isHookActive && isLineMax && !isAttach)
        {
            //    hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * 15);
            //    if(Vector2.Distance(transform.position, hook.position) < 0.1f)
            //    {
            //    }


            isHookActive = false;
            isLineMax = false;
            hook.gameObject.SetActive(false);
        }
        else if(isAttach)
        {
            GameManager.instance.SetChildCollidersActive(false);
            playerController.GetComponent<Animator>().SetTrigger("Hooking");
            Vector2 aA = transform.position - hook.position;
            playerController.GetComponent<Rigidbody2D>().gravityScale = 2;
            if (aA.normalized.x >= 0.5f && aA.normalized.y <= 0.4f)
            {
                GameManager.instance.SetChildCollidersActive(true);
                isAttach = false;
                isHookActive = false;
                isLineMax = false;
                hook.GetComponent<Hookg>().joint2D.enabled = false;
                hook.transform.parent = null;
                hook.gameObject.SetActive(false);

                playerController.isGrapplingJump = true;
            }
        }
    }


}
