using UnityEngine;
using UnityEngine.UIElements;

public class Portal : MonoBehaviour
{
    public float doubledSpeed = 2f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            print("oggetto con rigidbody entrato nel portale");
             rb.linearVelocity += rb.linearVelocity.normalized * doubledSpeed;
        }
    }
}
