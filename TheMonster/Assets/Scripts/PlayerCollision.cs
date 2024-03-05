using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public LayerMask enemyLayer;
    [SerializeField] GameObject deathPanel;
    public bool playerHasDied;

    private void Start()
    {
        playerHasDied = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((enemyLayer & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("Player collided with enemy!");
            deathPanel.SetActive(true);
            playerHasDied = true;
            Time.timeScale = 0f;
        }
    }
}
