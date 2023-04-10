using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField]
    private float health = 100f;

    [SerializeField]
    private float dieDelay = .01f;

    public void takeDamage(float amount)
    {
        health -= amount;

        if (IsOutOfHealth())
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject, dieDelay);
    }

    private bool IsOutOfHealth()
    {
        return health <= 0f;
    }
}
