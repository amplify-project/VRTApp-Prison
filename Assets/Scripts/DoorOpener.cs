using System.Collections;
using UnityEngine;

// Swings a door open or closed around a hinge. Call Open() / Close() / Toggle() from a
// NetworkTrigger's OnTrigger UnityEvent in the Inspector, so it runs for both visitors.
public class DoorOpener : MonoBehaviour
{
    [Tooltip("The door leaf that swings. Defaults to this GameObject.")]
    public Transform door;
    [Tooltip("Empty GameObject placed at the hinge. The door rotates around its position.")]
    public Transform hinge;
    [Tooltip("Axis to rotate around, in world space. Up for a normal door.")]
    public Vector3 axis = Vector3.up;
    [Tooltip("Total swing in degrees when open. Negative swings the other way.")]
    public float angle = 90f;
    public float duration = 2f;
    public AnimationCurve easing = AnimationCurve.EaseInOut(0, 0, 1, 1);

    float currentAngle;
    bool isOpen;
    Coroutine swing;

    void Reset()
    {
        door = transform;
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
        SwingTo(angle);
    }

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
        SwingTo(0f);
    }

    public void Toggle()
    {
        if (isOpen) Close();
        else Open();
    }

    void SwingTo(float target)
    {
        if (swing != null) StopCoroutine(swing);
        swing = StartCoroutine(Swing(target));
    }

    IEnumerator Swing(float target)
    {
        Transform leaf = door != null ? door : transform;
        Vector3 pivot = hinge != null ? hinge.position : leaf.position;
        Vector3 worldAxis = axis.normalized;
        float start = currentAngle;
        // A swing interrupted halfway takes proportionally less time.
        float time = Mathf.Max(0.01f, duration * Mathf.Abs(target - start) / Mathf.Max(0.01f, Mathf.Abs(angle)));
        float t = 0f;
        while (t < 1f)
        {
            t = Mathf.Min(1f, t + Time.deltaTime / time);
            float next = Mathf.Lerp(start, target, easing.Evaluate(t));
            leaf.RotateAround(pivot, worldAxis, next - currentAngle);
            currentAngle = next;
            yield return null;
        }
        swing = null;
    }
}
