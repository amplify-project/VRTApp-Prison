using System.Collections;
using UnityEngine;

// Hides the furnishings of a cell at scene load, then reveals them step by step when Begin() is called.
// Walls, floor, ceiling and door are simply left out of the steps, so they stay visible throughout.
// Call Begin() from a NetworkTrigger's OnTrigger UnityEvent in the Inspector, so it starts for both visitors.
// Both cells live in every visitor's scene, so put the matching object of Cell1 and Cell2 in the same step.
public class CellBuildUp : MonoBehaviour
{
    [System.Serializable]
    public class Step
    {
        [Tooltip("Objects revealed together, e.g. the bed of Cell1 and the bed of Cell2.")]
        public GameObject[] objects;
        [Tooltip("Seconds to wait before this step. Added to the default interval.")]
        public float extraDelay;
    }

    [Tooltip("Revealed in this order. Objects not listed here are never touched.")]
    public Step[] steps;
    [Tooltip("Seconds between steps.")]
    public float interval = 1.5f;
    [Tooltip("Seconds to wait after Begin() before the first step.")]
    public float startDelay = 1f;
    public enum Effect { Grow, Fade }

    [Tooltip("Grow scales the object up from nothing. Fade makes it gradually visible (URP Lit/Simple Lit/Unlit materials only, others pop in).")]
    public Effect effect = Effect.Fade;
    [Tooltip("Seconds the effect takes. 0 makes objects pop in at once.")]
    public float growDuration = 0.8f;
    public AnimationCurve growCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    bool started;

    // Awake, not Start: the objects must be hidden before the first frame is rendered.
    void Awake()
    {
        foreach (var step in steps)
        {
            foreach (var obj in step.objects)
            {
                if (obj != null) obj.SetActive(false);
            }
        }
    }

    public void Begin()
    {
        if (started) return;
        started = true;
        StartCoroutine(Build());
    }

    IEnumerator Build()
    {
        yield return new WaitForSeconds(startDelay);
        foreach (var step in steps)
        {
            yield return new WaitForSeconds(interval + step.extraDelay);
            foreach (var obj in step.objects)
            {
                if (obj != null) StartCoroutine(Reveal(obj));
            }
        }
    }

    IEnumerator Reveal(GameObject obj)
    {
        if (effect == Effect.Fade && growDuration > 0f)
        {
            yield return Fade(obj);
            yield break;
        }
        Transform t = obj.transform;
        Vector3 fullScale = t.localScale;
        if (growDuration > 0f) t.localScale = Vector3.zero;
        obj.SetActive(true);
        float elapsed = 0f;
        while (elapsed < growDuration)
        {
            elapsed += Time.deltaTime;
            t.localScale = fullScale * growCurve.Evaluate(Mathf.Clamp01(elapsed / growDuration));
            yield return null;
        }
        t.localScale = fullScale;
    }

    // Swaps each material for a transparent copy while fading, then restores the originals.
    IEnumerator Fade(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>(true);
        var originals = new Material[renderers.Length][];
        var fading = new System.Collections.Generic.List<Material>();
        for (int i = 0; i < renderers.Length; i++)
        {
            originals[i] = renderers[i].sharedMaterials;
            var copies = new Material[originals[i].Length];
            for (int j = 0; j < copies.Length; j++)
            {
                var m = originals[i][j];
                if (m == null || !m.HasProperty("_BaseColor") || !m.HasProperty("_Surface"))
                {
                    copies[j] = m; // unsupported shader: stays as is and pops in
                    continue;
                }
                var copy = new Material(m);
                copy.SetFloat("_Surface", 1f);
                copy.SetFloat("_Blend", 0f);
                copy.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                copy.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                copy.SetFloat("_ZWrite", 1f);
                copy.SetOverrideTag("RenderType", "Transparent");
                copy.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                copy.DisableKeyword("_ALPHATEST_ON");
                copy.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                var c = copy.GetColor("_BaseColor");
                c.a = 0f;
                copy.SetColor("_BaseColor", c);
                copies[j] = copy;
                fading.Add(copy);
            }
            renderers[i].sharedMaterials = copies;
        }
        obj.SetActive(true);

        float elapsed = 0f;
        while (elapsed < growDuration)
        {
            elapsed += Time.deltaTime;
            float a = growCurve.Evaluate(Mathf.Clamp01(elapsed / growDuration));
            foreach (var m in fading)
            {
                var c = m.GetColor("_BaseColor");
                c.a = a;
                m.SetColor("_BaseColor", c);
            }
            yield return null;
        }

        for (int i = 0; i < renderers.Length; i++) renderers[i].sharedMaterials = originals[i];
        foreach (var m in fading) Destroy(m);
    }
}
