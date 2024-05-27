using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public AudioSource audioSource;

    void OnCollisionStay(Collision collision)
    {
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

        if (pc)
        {
            pc.StartCoroutine(pc.Stun());

            audioSource.time = 0.2f;
            audioSource.Play();
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
