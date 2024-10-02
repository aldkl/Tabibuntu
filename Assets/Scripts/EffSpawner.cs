using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffSpawner : MonoBehaviour
{

    public GameObject CoinEffectOrigin;
    
    Queue<GameObject> coinEffects = new Queue<GameObject>();
    public AudioClip coinEffectClip2;
    public Transform CoinPa;

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            coinEffects.Enqueue(CreateEffect(new Vector2(-100, -100), 0));
        }
    }
    public GameObject CreateEffect(Vector2 createVec2, int SoundType)
    {
        GameObject newObj = Instantiate(CoinEffectOrigin);
        newObj.gameObject.SetActive(true);
        newObj.transform.SetParent(CoinPa);
        newObj.transform.position = createVec2;
        if(SoundType == 2)
        {
            newObj.GetComponent<AudioSource>().clip = coinEffectClip2;
        }
        newObj.GetComponent<AudioSource>().Play();
        return newObj;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
