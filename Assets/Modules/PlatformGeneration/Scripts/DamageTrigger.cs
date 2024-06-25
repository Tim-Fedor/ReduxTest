using Modules.PlayerTapController;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private bool _isIgnoreIDDQD;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<IPlayer>();
            player.ApplyHealthChange(player.Lives - _damage, _isIgnoreIDDQD);
        }
    }
}
