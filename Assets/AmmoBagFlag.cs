using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// Вішается на об'єкт, який можна класти в AmmoBag (наприклад Garand Mags).
// Стежить за тим, чи об'єкт зараз у тригерній зоні сумки, і магнітить
// його у вільний слот саме в момент, коли гравець відпускає об'єкт з руки.
public class AmmoBagFlag : MonoBehaviour
{
    [Tooltip("Об'єкт зараз лежить у слоті сумки")]
    public bool isInBag = false;

    [Tooltip("Сумка, у тригерній зоні якої зараз знаходиться об'єкт (виставляється автоматично скриптом AmmoBag)")]
    [HideInInspector] public AmmoBag bagInZone;

    [Tooltip("Не використовується, залишено для сумісності зі старою сценою")]
    public float notGrabbedTime = 0.5f;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponentInParent<XRGrabInteractable>();

        // Якщо об'єкт уже лежить у сумці на момент старту гри (парент у ієрархії сумки),
        // позначаємо його як покладений і робимо kinematic, щоб він не впав із сумки
        if (GetComponentInParent<AmmoBag>() != null)
        {
            isInBag = true;
            var startRb = GetComponent<Rigidbody>();
            if (startRb == null) startRb = GetComponentInChildren<Rigidbody>();
            if (startRb != null) startRb.isKinematic = true;
        }

        if (grabInteractable == null) return;

        // Підписуємось у Start, щоб гарантовано бути після інших слухачів (наприклад Magazine),
        // які встигають віджати об'єкт (unparent, isKinematic = false) до нашої перевірки
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isInBag = false;
        if (grabInteractable != null)
            grabInteractable.throwOnDetach = true; // відновлюємо можливість кидка
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        // Якщо об'єкт все ще в руці (наприклад, двохручний хват і відпустили лише одну руку) - нічого не робимо
        if (grabInteractable == null || grabInteractable.isSelected) return;

        // XRI після grab зі слота відновлює старий парент і isKinematic,
        // тому повертаємо об'єкт у "вільний" стан, щоб він міг вільно падати
        transform.SetParent(null);
        var rb = GetComponent<Rigidbody>();
        if (rb == null) rb = GetComponentInChildren<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        // Якщо в момент відпускання об'єкт був у тригерній зоні сумки - магнітимо у вільний слот.
        // Якщо поза зоною або вільних слотів немає - об'єкт просто падає.
        if (isInBag || bagInZone == null) return;

        if (bagInZone.TryStoreItem(transform))
            grabInteractable.throwOnDetach = false; // об'єкт тепер kinematic, кидок не потрібен
    }

    // Залишено для сумісності з іншими скриптами
    public void FlagBagged() => isInBag = true;

    public void FlagGrabbed()
    {
        isInBag = false;
        bagInZone = null;
    }
}
