using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private float maxZPos;
    [SerializeField] private float minZPos;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        float zPos = Mathf.Clamp(player.position.z, minZPos, maxZPos);
        transform.position = new Vector3(0,0,zPos);
    }
}
