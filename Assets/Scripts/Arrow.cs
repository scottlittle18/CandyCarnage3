﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField][Tooltip("The arrow's movement speed")]
    private int speed;

    [SerializeField][Tooltip("The amount of time the arrow can exist for before automatically being destroyed")]
    private int despawnTime = 1;

    private float timePassed = 0;

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        Despawn();
    }

    /// <summary>
    /// Moves the arrow in the direction it is facing
    /// </summary>
    private void MoveForward()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    /// <summary>
    /// Destroys the arrow if it has gone too long without hitting anything
    /// </summary>
    private void Despawn()
    {
        timePassed += Time.deltaTime;

        if (timePassed > despawnTime)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Destroys the arrow when it hits something
    /// </summary>
    /// <param name="other">the something that was hit</param>
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}