using System.Data;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    [SerializeField] private int _bulletToSpawn = 3;
    [SerializeField] private GameObject _bulletPrefab;
    void Start()
    {
        for(int i = 0; i < _bulletToSpawn; i++)
        {
            Instantiate(_bulletPrefab, Vector3.zero, Quaternion.identity);
        }
    }

}
