using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backroundMusicCombat : MonoBehaviour
{
    //gets sounds and clip
    public AudioSource backroundSource;
    public AudioClip backroundClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!backroundSource.isPlaying)
        {
            backroundSource.PlayOneShot(backroundClip);
        }
    }
}
