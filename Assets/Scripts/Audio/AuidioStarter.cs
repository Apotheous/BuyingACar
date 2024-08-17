using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuidioStarter : MonoBehaviour
{
    AudioListener audioListener;
    void Start()
    {
        audioListener = gameObject.GetComponent<AudioListener>();

        StartCoroutine(AuidoListenerOpener());

    }
    private IEnumerator AuidoListenerOpener()
    {
        yield return new WaitForSeconds(0.5f);

        AudioList();
    }

    private void AudioList()
    {
        if (audioListener != null)
        {
            audioListener.enabled = true;
        }
    }
}
