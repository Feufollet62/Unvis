using Player;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] private int fuelAmount = 60;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().ChangeFuel(fuelAmount);
            Destroy(gameObject);
        }
    }
}
