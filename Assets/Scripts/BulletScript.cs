using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private int damage;
    [SerializeField] private Stats stat;
    [SerializeField] private float knockbackForce = 5f;
    private void Awake()
    {
        damage = stat.Damage();       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 difference = (collision.transform.position - transform.position).normalized;
        Vector2 force = difference * knockbackForce;
        
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        if (collision.gameObject)
        {
            Destroy(gameObject);
        }
    }
}
