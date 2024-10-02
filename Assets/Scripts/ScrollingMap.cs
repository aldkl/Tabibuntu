using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class ScrollingMap : MonoBehaviour
{
    public float speed = 7;
    public bool isScroll;
    // Start is called before the first frame update
    void Start()
    {
        isScroll = true;
    }
    private void FixedUpdate()
    {
        if(isScroll) 
        { 
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
}
