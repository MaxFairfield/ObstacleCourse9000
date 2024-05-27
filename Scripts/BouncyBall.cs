using System.Collections;
using UnityEngine;

public class BouncyBall : MonoBehaviour
{
    public float bounceForce = 10.0f;
    public float bounceDuration = 1f;

    private bool debounce = false;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Coroutine to make the ball play through bounce animation
    IEnumerator Bounce()
    {
        debounce = true;

        audioSource.time = 0.3f;
        audioSource.Play();

        Vector3 originalScale = transform.localScale;
        Vector3 originalPosition = transform.position;

        // Squish (Low and Flat)
        yield return StartCoroutine(
            AnimateBounce(
                originalScale,
                originalPosition,
                new Vector3(originalScale.x + 1f, originalScale.y - 1f, originalScale.z + 1f),
                new Vector3(transform.position.x, originalPosition.y - 0.5f, transform.position.z),
                bounceDuration/3
            )
        );

        // Expand (Tall and Thin)
        yield return StartCoroutine(
            AnimateBounce(
                transform.localScale,
                transform.localPosition,
                new Vector3(originalScale.x - 2f, originalScale.y + 1f, originalScale.z - 2f),
                new Vector3(transform.position.x, originalPosition.y + 0.5f, transform.position.z),
                bounceDuration/3
            )
        );

        // Return to normal using original scale and position
        yield return StartCoroutine(
            AnimateBounce(
                transform.localScale,
                transform.localPosition,
                originalScale,
                originalPosition,
                bounceDuration/3
            )
        );

        transform.localScale = originalScale;
        transform.position = originalPosition;

        debounce = false;
    }

    // Coroutine to animate the bounce of the ball according to params, independently from main thread
    IEnumerator AnimateBounce(Vector3 originalScale, Vector3 originalPosition, Vector3 newSize, Vector3 newPosition, float duration)
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

    private void OnTriggerStay(Collider other)
    {
        if (!debounce && other.transform.position.y > transform.position.y)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //rb.velocity = new Vector3(rb.velocity.x, bounceForce, rb.velocity.y);
                Vector3 direction = (other.transform.position - transform.position).normalized;
                rb.velocity = (direction * bounceForce) + new Vector3(0, 0, Random.Range(-1f, 1f));
                StartCoroutine(Bounce());
            }
        }
    }
}
