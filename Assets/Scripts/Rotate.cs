using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    
    void Update()
    {
        transform.Rotate(0,speed * Time.deltaTime,0);
    }
}
