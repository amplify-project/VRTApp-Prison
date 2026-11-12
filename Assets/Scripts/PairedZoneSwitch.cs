using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PairedZoneSwitch : MonoBehaviour
{
    public PairedZone[] zones;

    [Tooltip("On: every zone must be occupied (two visitors). Off: the first occupied zone is enough (one visitor).")]
    public bool requireAll = true;
    [Tooltip("Seconds to wait after the condition is met before firing OnAllOccupied, e.g. so the door can be seen closing.")]
    public float delay = 0f;

    public UnityEvent OnAllOccupied;

    bool fired;

    // Wired in the Inspector to the OnChanged event of every zone.
    public void Check()
    {
        if (fired) return;
        if (requireAll)
        {
            foreach (var zone in zones)
            {
                if (!zone.Occupied) return;
            }
        }
        else
        {
            bool any = false;
            foreach (var zone in zones)
            {
                if (zone.Occupied) { any = true; break; }
            }
            if (!any) return;
        }
        fired = true;
        StartCoroutine(FireAfterDelay());
    }

    IEnumerator FireAfterDelay()
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        OnAllOccupied.Invoke();
    }
}
