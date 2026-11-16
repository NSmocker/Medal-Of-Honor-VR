using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Сумка для зброї. На об'єкті має бути BoxCollider з Is Trigger.
// Зброя з компонентом WeaponBagFlag магнітиться у вільний слот
// саме в момент відпускання з руки.
public class WeaponBag : MonoBehaviour
{
    [Tooltip("Слоти сумки. Зброя магнітиться до першого слоту без дочірніх об'єктів")]
    public List<Transform> slots;

    [Tooltip("Викликається, коли зброю успішно покладено у слот")]
    public UnityEvent onWeaponBagged;

    // Зброя увійшла у тригерну зону сумки
    void OnTriggerEnter(Collider other)
    {
        var weaponFlag = other.GetComponentInParent<WeaponBagFlag>();
        if (weaponFlag == null) return;

        weaponFlag.bagInZone = this;
    }

    // Зброя покинула тригерну зону сумки
    void OnTriggerExit(Collider other)
    {
        var weaponFlag = other.GetComponentInParent<WeaponBagFlag>();
        if (weaponFlag == null) return;

        if (weaponFlag.bagInZone == this)
            weaponFlag.bagInZone = null;
    }

    // Магнітить зброю у перший вільний слот. Повертає true, якщо поклали.
    public bool TryStoreItem(Transform item)
    {
        var flag = item.GetComponent<WeaponBagFlag>();
        var rb = item.GetComponent<Rigidbody>();
        if (rb == null) rb = item.GetComponentInChildren<Rigidbody>();
        if (rb == null) return false;

        foreach (Transform slot in slots)
        {
            if (slot == null || slot.childCount > 0) continue;

            item.SetParent(slot);
            item.localPosition = Vector3.zero;
            item.localRotation = Quaternion.identity;
            rb.isKinematic = true;
            if (flag != null) flag.FlagBagged();
            onWeaponBagged?.Invoke();
            return true;
        }

        return false;
    }
}
