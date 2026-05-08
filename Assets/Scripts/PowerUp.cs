using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int healAmount = 20;
    public float boostDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (gameObject.CompareTag("HealthItem"))
                {
                    player.Heal(healAmount);
                    Debug.Log("เก็บยา: " + healAmount);
                }
                else if (gameObject.CompareTag("SpeedItem"))
                {
                    player.ActivateSpeedBoost(boostDuration);
                    Debug.Log("เก็บไนตรัส");
                }
            }

            Destroy(gameObject);
        }
    }
}