using UnityEngine;
using UnityEngine.Events;

public class PairedZoneSwitch : MonoBehaviour
{
    public PairedZone[] zones;
    public UnityEvent OnAllOccupied;

    bool fired;

    // Wired in the Inspector to the OnChanged event of every zone.
    public void Check()
    {
        if (fired) return;
        foreach (var zone in zones)
        {
            if (!zone.Occupied) return;
        }
        fired = true;
        OnAllOccupied.Invoke();
    }
}
