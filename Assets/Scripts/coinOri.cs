using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class coinOri : MonoBehaviour
{

    public int CoinPrice;
    EffSpawner effSpawner;
    private void Start()
    {
        effSpawner = GameObject.Find("CoinEffSpawner").GetComponent<EffSpawner>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.Updatescore(CoinPrice);
            effSpawner.CreateEffect(transform.position, CoinPrice / 5);
            this.gameObject.SetActive(false);
        }
    }
}
