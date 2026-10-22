using System.Collections;
using UnityEngine;
using VRT.Pilots.Common;

public class TriggerAfterSceneLoad : NetworkTrigger
{
    public float delay = 60.0f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        base.Trigger(); // with MasterOnlyTrigger = true, non-masters ignore this
    }
}
