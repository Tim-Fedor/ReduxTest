using Modules.PlayerTapController;
using UnityEngine;

public abstract class ABoosterBehaviour : MonoBehaviour
{
    protected abstract void UseBooster(IPlayer player);
    public abstract void SetupValues(float floatValue, int intValue);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UseBooster(other.gameObject.GetComponent<PlayerController>());
        }
    }
}
