using System;
using UnityEngine;

public class Screen : MonoBehaviour
{
    [SerializeField] private Light screenLight;
    [SerializeField] private Color BSODColor;

    [SerializeField] private MeshRenderer meshRenderer;
    
    [SerializeField] private Material matBliss;
    [SerializeField] private Material matBSOD;

    [SerializeField] private Transform detectionPoint;

    private Transform it;
    
    private Color initialColor;
    private float initialIntensity;

    private void Start()
    {
        it = FindObjectOfType<Enemy>().transform;
        
        initialColor = screenLight.color;
        initialIntensity = screenLight.intensity;
    }

    private void Update()
    {
        if (Vector3.Distance(it.position, detectionPoint.position) < 3f)
        {
            meshRenderer.sharedMaterial = matBSOD;
            screenLight.color = BSODColor;
            screenLight.intensity = 5;
        }
        else
        {
            meshRenderer.sharedMaterial = matBliss;
            screenLight.color = initialColor;
            screenLight.intensity = initialIntensity;
        }
    }
}