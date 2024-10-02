using System.Collections;
using UnityEngine;

public class MonsterRandomAni : MonoBehaviour
{
    private Animator myAnimator;
    private float Curtime = 0; // 애니메이션 변경 간격 (초)
    private float animationInterval = 10f; // 애니메이션 변경 간격 (초)
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