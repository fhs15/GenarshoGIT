using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triangleManager : MonoBehaviour
{
    [SerializeField]
    float CleanupDelaySeconds = 3f;
    [SerializeField]
    float CleanupTimeSeconds = 2f;
    bool CleanupCoroutine = false;
    float time = 0;
    Color grayColor;

    [SerializeField]
    AudioClip destroySound;

    void Start()
    {
        gameObject.AddComponent<AudioSource>().PlayOneShot(destroySound);

        grayColor = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        StartCoroutine(WaitForCleanup());
        Rigidbody2D rb;
       
        foreach (Transform child in transform)
        {
            rb = child.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(Random.Range(-500,500),Random.Range(-500,500)));
        }
    }

    private void FixedUpdate()
    {
        if (CleanupCoroutine)
        {
            time += Time.deltaTime;
            foreach (Transform child in transform)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.Lerp(grayColor, new Color(grayColor.r, grayColor.g, grayColor.b, 0), time/CleanupTimeSeconds);
            }
        }
    }

    protected IEnumerator WaitForCleanup()
    {
        yield return new WaitForSeconds(CleanupDelaySeconds);
        CleanupCoroutine = true;
    }
}
