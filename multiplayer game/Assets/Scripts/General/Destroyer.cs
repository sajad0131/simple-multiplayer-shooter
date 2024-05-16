using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAudioSource;
    public float generalTimeToDestroy = 2f;
    private bool destroying;
    void Update()
    {
        if (isAudioSource && gameObject.GetComponent<AudioSource>().isPlaying && !destroying)
        {
            StartCoroutine(Destroy(time()));
        }
        if (!isAudioSource && !destroying)
        {
            StartCoroutine(Destroy(generalTimeToDestroy));
        }
        
    }

    private float time()
    {
        if (isAudioSource)
        {
            
            return gameObject.GetComponent<AudioSource>().clip.length;
        }
        else
        {
            return generalTimeToDestroy;
        }
    }

    IEnumerator Destroy(float t)
    {
        destroying = true;
        yield return new WaitForSeconds(t);
        DestroyImmediate(this.gameObject);
    }


}
