using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField]
    protected Rigidbody2D rigidBody2D;

    protected BulletDataSO _bulletData;

    int RicochetCount = 0;

    Vector3 Velocity;
    Vector3 initVelocity = new Vector3(0.1f, 0.1f, 0.1f);

    public BulletDataSO BulletData
    {
        get => _bulletData;
        set
        {
            _bulletData = value;
            rigidBody2D = GetComponent<Rigidbody2D>();
            rigidBody2D.drag = BulletData.Friction;
        }
    }

    public void init()
    {
        RicochetCount = 0;
        float scale = BulletData.Size;
        transform.localScale = new Vector3(0.2f * scale, 0.1f * scale, 1f * scale);
        rigidBody2D.velocity = transform.right * _bulletData.BulletSpeed;
        initVelocity = rigidBody2D.velocity;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (IsServer)
        {
            switch (col.gameObject.tag)
            {
                case "Wall":
                    itemDestructionScript wall = col.gameObject.GetComponent<itemDestructionScript>();
                    wall.TakeDamageClientRpc(_bulletData.Damage, itemDestructionScript.DamageType.Bullet);

                    if (RicochetCount < _bulletData.MaxRicochets)
                    {
                        var speed = initVelocity.magnitude;
                        var direction = Vector3.Reflect(Velocity.normalized, col.contacts[0].normal);
                        rigidBody2D.velocity = direction * speed;
                        RicochetCount++;
                    }
                    else
                    {
                        gameObject.GetComponent<NetworkObject>().Despawn(false);
                        gameObject.SetActive(false);
                    }
                    break;
                case "Player":
                    Health player = col.gameObject.GetComponent<Health>();
                    player.TakeDamageClientRpc(_bulletData.Damage, Health.DamageType.Bullet);
                    gameObject.GetComponent<NetworkObject>().Despawn(false);
                    gameObject.SetActive(false);
                    break;
                case "Bullet":
                    break;
                case "Borderwall":
                    BorderwallRicochet borderwall = col.gameObject.GetComponent<BorderwallRicochet>();
                    borderwall.TakeDamageClientRpc(BorderwallRicochet.DamageType.Bullet);

                    if (RicochetCount < _bulletData.MaxRicochets)
                    {
                        var speed = initVelocity.magnitude;
                        var direction = Vector3.Reflect(Velocity.normalized, col.contacts[0].normal);
                        rigidBody2D.velocity = direction * speed;
                        RicochetCount++;
                    }
                    else
                    {
                        gameObject.GetComponent<NetworkObject>().Despawn(false);
                        gameObject.SetActive(false);
                    }
                    break;
                default:
                    Debug.LogWarning("Bullet hit unexpected object");
                    gameObject.GetComponent<NetworkObject>().Despawn(false);
                    gameObject.SetActive(false);
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            Velocity = rigidBody2D.velocity;

            if (rigidBody2D != null && _bulletData != null)
            {
                transform.right = rigidBody2D.velocity.normalized;
                if (Velocity.magnitude < initVelocity.magnitude * 0.99)
                {
                    rigidBody2D.velocity = transform.right * initVelocity.magnitude;
                }
            }
        }
    }
}
