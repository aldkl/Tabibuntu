using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCoin : MonoBehaviour
{
    [SerializeField]
    private int coinName;
    [SerializeField]
    private string CoinText;

    private void Start()
    {
        int coinStatus = PlayerPrefs.GetInt(coinName.ToString(), 0);

        // 코인이 먹힌 상태(1)라면 코인을 비활성화
        if (coinStatus == 1)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.GetSpecialCoin(coinName, CoinText);
            gameObject.SetActive(false);
        }
    }
}
