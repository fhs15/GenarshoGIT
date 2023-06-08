using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField]
    protected Health playerHealth;

    [SerializeField]
    protected Weapon Weapon;

    public CardsController cardsController;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject blackSquare;

    bool RoundRecentlyEnded = false;

    private void Start()
    {
        AbilityManager.Instance.playerManagers.Add(this);
    }

    [ClientRpc]
    public void PlayerDiedClientRpc()
    {
        if (!RoundRecentlyEnded)
        {
            StartCoroutine(RoundEndedWaitForSeconds(5));
            if (IsOwner)
            {
                ScoreController.Instance.RoundOver(OnRoundOverWinner.Lost);
            }
            else
            {
                ScoreController.Instance.RoundOver(OnRoundOverWinner.Won);
            }
        }
    }

    protected IEnumerator RoundEndedWaitForSeconds(float timeInSeconds)
    {
        RoundRecentlyEnded = true;
        yield return new WaitForSeconds(timeInSeconds);
        RoundRecentlyEnded = false;
    }

    public enum OnRoundOverWinner
    {
        Won = 0,
        Lost = 1,
    }

    public void Heal(int amount)
    {
        playerHealth.GetHealedServerRpc(amount);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeWeaponServerRpc(int weaponIndex, float ammoMultiplier)
    {
        ChangeWeaponClientRpc(weaponIndex, ammoMultiplier);
    }

    [ClientRpc]
    protected void ChangeWeaponClientRpc(int weaponIndex, float ammoMultiplier)
    {
        var weaponData = WeaponDataStorage.Instance.GetByIndex(weaponIndex);
        weaponData.AmmoCapacity = Mathf.RoundToInt(weaponData.AmmoCapacity * ammoMultiplier);
        Weapon.ChangeWeapon(weaponData);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DragReduceServerRpc(float amount)
    {
        DragReduceClientRpc(amount);
    }

    [ClientRpc]
    private void DragReduceClientRpc(float amount)
    {
        rb.drag = amount;
    }

    public void BonusHealth(int amount)
    {
        playerHealth.AddMaxHealthServerRpc(amount);
    }

    public void ChangeLuck(int amount)
    {
        cardsController.luck = amount;
    }

    public void ChangeCardHandSize(int amount)
    {
        cardsController.cardCount = amount;
    }

    public void DrawCards()
    {
        cardsController.DrawCards();
    }

    public void BuildRelativeBox(int boxHealth)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePos3D = new Vector3(mousePos.x, mousePos.y, 0f);

        //get a vector direction and clamp the magnitude to limit build range
        Vector3 direction = mousePos3D - gameObject.transform.position;
        float clampedDistance = Mathf.Clamp(direction.magnitude, 0f, 5f);
        Vector3 endPos = gameObject.transform.position + direction.normalized * clampedDistance;

        //Rect rect = new Rect(-12f, 5.45f, 24.5f, 11.45f);
        Rect rect = new Rect() { xMin = -12f, xMax = 12.4f, yMin = 5.45f, yMax = -5.95f };
        if (!rect.Contains(endPos, true))
        {
            throw new ArgumentOutOfRangeException(nameof(endPos), $"Box can't be built outside of arena bounds");
        }

        Quaternion rotation = Weapon.GetComponentInParent<Transform>().rotation;

        BuildRelativeBoxHelperServerRpc(boxHealth, endPos, rotation);
    }

    [ServerRpc(RequireOwnership = false)]
    private void BuildRelativeBoxHelperServerRpc(int boxHealth, Vector3 position, Quaternion rotation)
    {
        GameObject tempObj = Instantiate(blackSquare, position, rotation);
        tempObj.transform.localScale = new Vector3(0.5f, 2.5f, 1);
        tempObj.GetComponent<itemDestructionScript>().health = boxHealth;
        tempObj.GetComponent<NetworkObject>().Spawn();
    }
}

