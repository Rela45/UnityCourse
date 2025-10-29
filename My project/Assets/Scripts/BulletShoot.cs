using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private int _shootForce;
    [SerializeField] private Transform _spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var bullet = Instantiate(_bullet, null);
            bullet.transform.position = _spawnPoint.position;
            bullet.SetActive(true);

            var rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * _shootForce, ForceMode.Impulse); //direziono sempre davanti il proiettile grazie al transform
        }
    }
}
