using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appear : NetworkCollider
{
    public GameObject button;
    public GameObject sound;
    public GameObject nun;



    // Start is called before the first frame update
    public override void Trigger()
    {
        StartCoroutine(EnableAfterTime());
    }

    IEnumerator EnableAfterTime()
    {
        yield return new WaitForSeconds(360.0f);
        button.SetActive(true);
        sound.SetActive(true);
        nun.SetActive(true);


    }
}

