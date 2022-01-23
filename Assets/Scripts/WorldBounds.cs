using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBounds : MonoBehaviour
{
    [SerializeField] private GameObject startingPoint;
    private GameObject player;
    CharacterController2D controller2D;
    
    private void Awake()
    {
        player = GameObject.Find("Player");
        controller2D = player.GetComponent<CharacterController2D>();
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           controller2D.StartCoroutine(controller2D.RespawnChar());
        }
    }
}
