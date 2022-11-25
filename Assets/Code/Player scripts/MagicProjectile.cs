using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RemixGame
{
    public class MagicProjectile : MonoBehaviour
    {
        [SerializeField] private float Speed = 4f;
        [SerializeField] private float projectileDespawnTime = 0.5f;
        [SerializeField] private Magicblock magicblock;
        [SerializeField] private string levelWallsParentTag = "Walls";
        [SerializeField] private string levelPlatformsParentTag = "Platforms";

        private Rigidbody2D projectileRb;
        private GameObject player;
        private bool playerFacingRight;
        private bool timerPassed = false;

        // Initializing the projectileRb with the gameobjects rigidbody
        // also used to check if the player character is facing left or right
        // which is used to determine the direction of movement for the
        // projectile
        private void Awake()
        {
            projectileRb = gameObject.GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                Debug.LogError("Object with the tag 'Player' was not found for the projectile.");
            }

            if (player.GetComponent<Character>().FacingRight)
            {
                playerFacingRight = true;
            }
            else if (!player.GetComponent<Character>().FacingRight)
            {
                playerFacingRight = false;
            }

            StartCoroutine(DespawnTimer());
        }

        // A magicblock is spawned and the projectile is destroyed if the timercoroutine has passed.
        private void Update()
        {
            if (timerPassed)
            {
                SpawnMagicblock();
            }
        }

        // This is used for the movement of the projectile
        // The if statement determines the direction of movement for the projectile
        private void FixedUpdate()
        {
            if (playerFacingRight)
            {
                projectileRb.velocity = transform.right * Speed;
            }
            else if (!playerFacingRight)
            {
                projectileRb.velocity = -transform.right * Speed;
            }
        }

        // This is used to destroy the projectile if it collides with something
        // that it can collide with.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(levelWallsParentTag) || collision.gameObject.CompareTag(levelPlatformsParentTag))
            {
                SpawnMagicblock();
            }

            Destroy(gameObject);
        }

        // Method to spawn the magicblock(s)
        private void SpawnMagicblock()
        {
            Instantiate(magicblock, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }

        // Simple despawn timer coroutine to track determine if the projectile has been instantiated
        // for enough time.
        IEnumerator DespawnTimer()
        {
            if (!timerPassed)
            {
                yield return new WaitForSeconds(projectileDespawnTime);
                timerPassed = true;
            }
        }
    }
}
