using System.Collections;
using UnityEngine;

public class MonsterRandomAni : MonoBehaviour
{
    private Animator myAnimator;
    private float Curtime = 0; // �ִϸ��̼� ���� ���� (��)
    private float animationInterval = 10f; // �ִϸ��̼� ���� ���� (��)
    private string[] animations = { "MonsterSide", "MonsterCrying" };

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        Curtime = 0;
    }

    private void Update()
    {

        Curtime += Time.deltaTime;
        if (Curtime > animationInterval)
        {
            Curtime = 0;
            string randomAnimation = animations[Random.Range(0, animations.Length)];
            myAnimator.Play(randomAnimation);
        }

    }
}