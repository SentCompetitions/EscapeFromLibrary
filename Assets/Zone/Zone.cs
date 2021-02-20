using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Zone: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        OnZoneEnter(other);
    }

    public virtual void OnZoneEnter(Collider other) {}
}