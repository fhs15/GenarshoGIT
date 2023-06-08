using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class itemDestructionScript : NetworkBehaviour
{
    [SerializeField]
    public int health = 5;

    [SerializeField]
    GameObject destroyedCube;

    [SerializeField]
    private AudioClip hitSoundBullet;

    [SerializeField]
    private AudioClip hitSoundMelee;

    private AudioSource src;

    [SerializeField]
    private SpriteRenderer sprite;

    private bool DamageTaken = false;

    private float time = 0f;

    private float colorFadeDuration = 0.2f;

    private void Start()
    {
        src = gameObject.GetComponent<AudioSource>();
    }

    [ClientRpc]
    public void TakeDamageClientRpc(int amount, DamageType type)
    {
        if (IsServer)
        {
            health -= amount;

            if (health <= 0)
            {
                GameObject tempObj = Instantiate(destroyedCube, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0), Quaternion.identity);
                tempObj.transform.localScale = new Vector3(gameObject.transform.lossyScale.x, gameObject.transform.lossyScale.y, 1);
                tempObj.transform.rotation = gameObject.transform.rotation;
                tempObj.GetComponent<NetworkObject>().Spawn();

                gameObject.GetComponent<NetworkObject>().Despawn();
                gameObject.SetActive(false);
            }
        }

        if (health > 0)
        {
            StartCoroutine(TakeDamageCoroutine());
            switch (type)
            {
                case DamageType.Bullet:
                    src.PlayOneShot(hitSoundBullet);
                    break;
                case DamageType.Melee:
                    src.PlayOneShot(hitSoundMelee);
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (DamageTaken)
        {
            time += Time.deltaTime;
            sprite.color = Color.Lerp(new Color(0.5f,0.5f,0.5f,1f), Color.white, time / colorFadeDuration);
        } 
        else
        {
            sprite.color = Color.white;
        }
    }

    protected IEnumerator TakeDamageCoroutine()
    {
        time = 0f;
        DamageTaken = true;
        yield return new WaitForSeconds(colorFadeDuration);
        DamageTaken = false;
    }

    public enum DamageType
    {
        Bullet = 0,
        Melee = 1,
    }
}
