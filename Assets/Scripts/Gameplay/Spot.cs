using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Light))]
public class Spot : MonoBehaviour
{
    private float maxIntensity;
    [SerializeField] private float minIntensity = 0.2f;
    
    [SerializeField] [Range(0,.99f)] private float normalFlickerAmount = .315f;
    [SerializeField] [Range(0,.99f)] private float itCloseFlickerAmount = .5f;
    
    [SerializeField] [Range(0,  1f)] private float flickerThreshold = .3f;
    [SerializeField] [Range(0,  1f)] private float flickerInterval = .03f;

    [SerializeField] private float itMaxDistance = 5f;
    
    private float timer;
    private bool on = true;
    
    private Light thisLight;
    private Transform enemy;

    // Add random chance to go out
    
    private void Start()
    {
        thisLight = GetComponent<Light>();
        maxIntensity = thisLight.intensity;

        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (!(timer > flickerInterval)) return;

        float distance = Vector3.Distance(transform.position + Vector3.down * 2, enemy.position);
        float flicker = Mathf.Lerp(itCloseFlickerAmount, normalFlickerAmount, distance / itMaxDistance);
        
        if (on)
        {
            thisLight.intensity = maxIntensity;
            
            float rng = Random.Range(0f, flicker);

            if (rng > flickerThreshold)
            {
                on = false;
                thisLight.intensity = minIntensity;
            }
        }
        else
        {
            on = true;
            thisLight.intensity = maxIntensity;
        }

        timer = 0;
    }
}