using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float _timeToDie = 5f;
    void Start()
    {
        Destroy(gameObject,_timeToDie);
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        print("colliso con li plane");
        Destroy(gameObject);
    }
}
