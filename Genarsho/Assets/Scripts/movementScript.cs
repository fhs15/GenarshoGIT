using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class movementScript : NetworkBehaviour
{
    public Animator animator;
    
    [SerializeField]
    private Vector2 moveVector;
    [SerializeField]
    public float movemementSpeed = 10f;

    [SerializeField]
    protected Rigidbody2D rb = null;

    AudioSource audioSourceStep;
    [SerializeField] AudioClip audioClipStepLeft;
    [SerializeField] AudioClip audioClipStepRight;
    bool step = false;

    private void Start()
    {
        audioSourceStep = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(Vector2 vector)
    {
        moveVector = vector;
    }

    public void OnStopMove(InputValue inputValue)
    {
        if (!IsOwner) return;
        audioSourceStep.Stop();
        moveVector = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if(IsOwner)
        {
            //rb.velocity = moveVector * movemementSpeed;
            rb.AddForce(moveVector * movemementSpeed);

            Transform childSprite = transform.Find("playerSprite");
            if (moveVector.x < 0) childSprite.transform.localScale = new Vector3(-1, 1, 1); else childSprite.transform.localScale = new Vector3(1, 1, 1);

            var currentMovement = rb.velocity.magnitude;
            SetAimatorSpeedServerRpc(currentMovement);
            float clampedSpeed = Mathf.Clamp(currentMovement / 3, 0, 2);
            if (currentMovement > 0.25) animator.speed = clampedSpeed; else animator.speed = 1;

            if (moveVector.x != 0 || moveVector.y != 0) walkingSoundServerRpc(movemementSpeed);

            //rb.velocity = new Vector2 (Mathf.Clamp(rb.velocity.x, -3.2f, 3.2f), Mathf.Clamp(rb.velocity.y, -3.2f, 3.2f));
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 3.2f);
        }
    }

    [ServerRpc]
    void walkingSoundServerRpc(float movementSpeed)
    {
        walkingSoundClientRpc(movementSpeed);
    }

    [ClientRpc]
    void walkingSoundClientRpc(float movementSpeed)
    {
        if (!audioSourceStep.isPlaying)
        {
            audioSourceStep.volume = Random.Range(0.6f, 0.8f);
            audioSourceStep.pitch = (Random.Range(0.3f, 0.6f))+(movementSpeed/30);
            if (step)
            {
                audioSourceStep.PlayOneShot(audioClipStepLeft);
                step = false;
            }
            else
            {
                audioSourceStep.PlayOneShot(audioClipStepRight);
                step = true;
            }
        }
    }

    [ServerRpc]
    private void SetAimatorSpeedServerRpc(float currentMovement)
    {
        animator.SetFloat("Speed", currentMovement);
    }
}
