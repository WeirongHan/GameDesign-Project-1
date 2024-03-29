﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region movement_variables
    public float movespeed;
    #endregion

    #region physics_components
    Rigidbody2D enemyRB;
    #endregion

    #region targeting_variables
    public Transform player;
    #endregion

    #region attack_variables
    public float explosionDamage;
    public float explosionRadius;
    public GameObject explosionObj;
    #endregion

    #region health_variables
    public float maxHealth;
    float currHealth;
    #endregion

    #region Unity_functions
    //Runs once on creation
    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
    }

    //Runs every frame
    private void Update()
    {
        //Check to see if we know where player is
        if (player == null)
        {
            return;
        }

        Move();
    }

    #endregion

    #region movement_functions
    //Move directly at player
    private void Move()
    {
        //Calculate the movement vecrtor. Player pos - Enemy pos = Direction of player relative to enemy
        Vector2 direction = player.position - transform.position;

        enemyRB.velocity = direction.normalized * movespeed;
    }
    #endregion

    #region attack_functions
    //Raycasts box for player and causes damage, spawns explosion prefab
    private void Explode()
    {
        //Call audio manger for explosion sound
        FindObjectOfType<AudioManager>().Play("Explosion");
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                //cause damage
                hit.transform.GetComponent<PlayerController>().TakeDamage(explosionDamage);
                Debug.Log("Hit Player with explosion");

                //Spawn Explosion prefab in game
                Instantiate(explosionObj, transform.position, transform.rotation);
                Destroy(this.gameObject);

            }


        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Explode();
        }

    }


    #endregion

    #region health_functions
    //Enemy takes damage based on 'value' param
    public void TakeDamage(float value)
    {

        //Call audio manger for explosion sound
        FindObjectOfType<AudioManager>().Play("BatHurt");
        //Decrement Health
        currHealth -= value;
        Debug.Log("Health is now" + currHealth.ToString());

        //Check for death
        if (currHealth <= 0)
        {
            Die();
        }

        //Destroys enemy object
        void Die()
        {
            //Destroy game object 
            Destroy(this.gameObject);
        }
    }
    #endregion
}
