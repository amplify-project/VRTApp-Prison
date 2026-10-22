using UnityEngine;
using UnityEngine.Events;
using VRT.Pilots.Common;

public class PairedZone : MonoBehaviour
{
    public NetworkTrigger enterTrigger;
    public UnityEvent OnChanged;

    public bool Occupied { get; private set; }

    bool sent;

    private void OnTriggerEnter(Collider other)
    {
        if (sent) return;
        sent = true;
        enterTrigger.Trigger();
    }

    // Wired in the Inspector to the OnTrigger event of enterTrigger.
    public void SetOccupied()
    {
        Occupied = true;
        OnChanged.Invoke();
    }
}
