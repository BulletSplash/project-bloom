using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBoxScript : MonoBehaviour
{
    private CharacterController2D controller2D;
    private GameObject player;
    void Awake()
    {
        player = GameObject.Find("Player");

        controller2D = player.GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Bullet")
        {
            StartCoroutine(controller2D.OnHitHeatlth());
        }
    }
}
