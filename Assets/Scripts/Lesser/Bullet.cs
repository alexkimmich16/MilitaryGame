using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.GetComponent<Skeleton>())
        {
            col.transform.GetComponent<Skeleton>().AddDamage(Damage);
        }
        Destroy(gameObject);
    }
}
