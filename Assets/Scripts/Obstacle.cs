using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int scoreValue = 10;
    public int hazardDamage = 20;
    public int scorableDamage = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (gameObject.CompareTag("Scorable"))
            {
                AddScoreToPlayer();
            }
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (gameObject.CompareTag("Hazard"))
                {
                    player.TakeDamage(hazardDamage);
                }
                else if (gameObject.CompareTag("Scorable"))
                {
                    player.TakeDamage(scorableDamage);
                    AddScoreToPlayer(); 
                }
            }
            Destroy(gameObject);
        }
    }

    void AddScoreToPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            PlayerController player = playerObj.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddScore(scoreValue);
            }
        }
    }
}