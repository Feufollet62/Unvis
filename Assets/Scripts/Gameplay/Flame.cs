using UnityEngine;
using Random = UnityEngine.Random;

public class Flame : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime = .5f;
    [SerializeField] private float lifeTimeVar = .1f;
    [SerializeField] private float inaccuracy = 5f;
    
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask wallMask;

    private float timer;

    private void Start()
    {
        lifeTime += Random.Range(-lifeTimeVar, lifeTimeVar);

        // Spread
        Vector3 inaccuracyAngles = new Vector3( Random.Range(-inaccuracy, inaccuracy),
                                                Random.Range(-inaccuracy, inaccuracy),
                                                Random.Range(-inaccuracy, inaccuracy));

        transform.Rotate(inaccuracyAngles);
    }
    
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        
        if (timer > lifeTime || Physics.Raycast(transform.position, transform.forward, speed, wallMask))
        {
            Destroy(gameObject);
        }
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, speed, enemyMask))
        {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            
            // Si il fuit alors il est invincible
            if (enemy.state != ItState.Flee)
            {
                enemy.Hit();
            } 
            
            Destroy(gameObject);
        }
        
        else
        {
            transform.position += transform.forward * speed * .1f;
        }
    }
}
