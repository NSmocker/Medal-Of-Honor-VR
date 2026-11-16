using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Сумка для патронів. На об'єкті має бути BoxCollider з Is Trigger.
// Об'єкти з компонентом AmmoBagFlag реєструються, коли заходять у тригерну зону,
// і магнітяться у перший вільний слот саме в момент відпускання з руки.
public class AmmoBag : MonoBehaviour
{
    [Tooltip("Слоти сумки. Об'єкт магнітиться до першого слоту без дочірніх об'єктів")]
    public List<Transform> slots;

    [Tooltip("Викликається, коли об'єкт успішно покладено у слот")]
    public UnityEvent onAmmoBagged;

    // Об'єкт увійшов у тригерну зону сумки (може бути ще в руці гравця)
    void OnTriggerEnter(Collider other)
    {
        var ammoBagFlag = other.GetComponentInParent<AmmoBagFlag>();
        if (ammoBagFlag == null) return;

        ammoBagFlag.bagInZone = this;
    }

    // Об'єкт покинув тригерну зону сумки
    void OnTriggerExit(Collider other)
    {
        var ammoBagFlag = other.GetComponentInParent<AmmoBagFlag>();
        if (ammoBagFlag == null) return;

        if (ammoBagFlag.bagInZone == this)
            ammoBagFlag.bagInZone = null;
    }

    // Магнітить об'єкт у перший вільний слот. Повертає true, якщо поклали.
    public bool TryStoreItem(Transform item)
    {
        var flag = item.GetComponent<AmmoBagFlag>();
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
            onAmmoBagged?.Invoke();
            return true;
        }

        return false;
    }
}
