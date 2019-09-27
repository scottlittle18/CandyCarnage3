﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //Damage is the same for all enemies; different enemy types will need to be children of this class
    [SerializeField][Tooltip("The damage for all enemies using this script")]
    private static int damage = 20;
    public static int enemyDamage
    {
        get { return damage; }
    }

    [SerializeField][Tooltip("The enemy's max health")]
    private int maxHealth = 200;
    private int health;

    [SerializeField][Tooltip("The force at which the enemy is knocked away when hitting the player (needs to be high)")]
    private float force = 2000;
    private Rigidbody myRigidbody;

    private NavMeshAgent myNavMeshAgent;
    private GameObject[] players;
    private GameObject closestPlayer;
    private float distance;
    private bool isDead = false; //even though the enemy is destoryed on death, this is needed to stop AttackPlayer()

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        players = GameObject.FindGameObjectsWithTag("Player");
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfDead();
        FindClosestPlayer();
        AttackPlayer();

        foreach(GameObject player in players)
            Debug.Log(player);
    }


    /// <summary>
    /// Destroys itself if it doesn't have any health left
    /// </summary>
    private void CheckIfDead()
    {
        if (health <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Takes damage when hit by an attack.    (WILL NEED TO EDIT THIS WHEN THERE ARE MULTIPLE ATTACK TYPES)
    /// </summary>
    /// <param name="other">The trigger that was hit</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            health -= 20;
            Debug.Log(health);
        }
    }

    /// <summary>
    /// Pushes the enemy away from the player
    /// adapted from this post https://answers.unity.com/questions/1100879/push-object-in-opposite-direction-of-collision.html
    /// </summary>
    /// <param name="collision">Whatever the enemy collides with</param>
    private void OnCollisionStay(Collision collision)
    {
        // If the object we hit is the enemy
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player hit");
            // Calculate Angle Between the collision point and the player
            Vector3 direction = collision.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            direction = -direction.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            myRigidbody.AddForce(direction * force);
        }
    }

    /// <summary>
    /// Uses the enemy's navMesh to move to the closest player's position
    /// </summary>
    private void AttackPlayer()
    {
        if (!isDead) //stops it from trying to move the same frame as death
            myNavMeshAgent.SetDestination(closestPlayer.transform.position);
    }

    /// <summary>
    /// Returns the player that is closest
    /// adapted from https://answers.unity.com/questions/264568/find-objects-and-go-there-with-nav-mesh-agent.html
    /// </summary>
    /// <returns></returns>
    private void FindClosestPlayer()
    {
        GameObject closest = null;
        distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject player in players)
        {
            Vector3 difference = player.transform.position - position;
            float targetDistance = difference.sqrMagnitude;
            if (targetDistance < distance) //distance starts at infinity so this will always work at least once
            {
                closest = player;
                distance = targetDistance;
            }
        }
        closestPlayer = closest;
    }
}