using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform subject;

    Vector2 startPos;
    float startZ;

    Vector2 travel => (Vector2)cam.transform.position - startPos;

    float distanceFromSubject => transform.position.z - subject.position.z;

    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));

    float parallaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;

    private void Start()
    {
        startPos = transform.position;
        startZ = transform.position.z;
    }
    private void Update()
    {
        Vector2 newPos = startPos + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, cam.transform.position.y - 6.5f, startZ);
    }
}
