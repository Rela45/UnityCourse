using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float speed = 0.1f;
    private Vector3 movement = Vector3.zero;
    [SerializeField] private float _rotateSpeed = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void FixedUpdate()
    {
        if(movement != Vector3.zero)  _rb.linearVelocity = new Vector3(movement.x * speed, _rb.linearVelocity.y, movement.z * speed); //funzione del movimento fatta in fixedupdate sempre perchè è qui che si gestisce la fisica
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            movement = transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement = -transform.forward;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0f,-_rotateSpeed, 0f));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0f, _rotateSpeed, 0f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        speed = speed * 2;
    }

}
