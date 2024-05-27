using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puncher : MonoBehaviour
{
    private bool doPunch = false;
    private Vector3 originalPosition;

    public AudioSource audioSource;

    IEnumerator PunchCycle()
    {
        while (true)
        {
            transform.position = originalPosition + new Vector3(0, 0, -5);

            doPunch = true;

            StartCoroutine(
                Animate(
                    transform.localScale,
                    originalPosition + new Vector3(0, 0, -5),
                    transform.localScale,
                    originalPosition,
                    0.25f
                )
            );

            yield return new WaitForSeconds(0.1f);

            doPunch = false;

            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }
    void Start()
    {
        originalPosition = transform.position;

        StartCoroutine(PunchCycle());
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (doPunch && collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            StartCoroutine(pc.Stun());
            audioSource.time = 0.2f;
            audioSource.Play();
        }
    }

    IEnumerator Animate(Vector3 originalScale, Vector3 originalPosition, Vector3 newSize, Vector3 newPosition, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, newSize, elapsedTime / duration);
            transform.position = Vector3.Lerp(originalPosition, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
