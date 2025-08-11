using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Background : MonoBehaviour
{
    [SerializeField] float speed = 1;

    List<Transform> backgrounds = new List<Transform>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in transform)
        {
            backgrounds.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in backgrounds)
        {
            child.Translate(0, -speed * Time.deltaTime, 0);
        }
    }
}
