using UnityEngine;

public class StartPercorso : MonoBehaviour
{
void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerMover>();
        if(player != null)
        {
            if (!GameManager.Instance.IsTimerStarted())
            {
                GameManager.Instance.StartTimer();
            }
        }
    }
}
