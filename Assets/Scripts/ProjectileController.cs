using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the Ground
        if (collision.gameObject)
        {
            Debug.Log("Projectile collided with... something");
            Destroy(gameObject);
        }
    }
}
