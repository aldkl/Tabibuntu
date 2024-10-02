using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookg : MonoBehaviour
{

    GrapplingHook grappling;
    public DistanceJoint2D joint2D;
    // Start is called before the first frame update
    void Start()
    {
        grappling = GameObject.Find("Player").GetComponent<GrapplingHook>();
        joint2D = GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Giwa") 
        {
            grappling.isAttach = true;
            joint2D.enabled = true;
            transform.parent = collision.transform;
        }
    }
}
