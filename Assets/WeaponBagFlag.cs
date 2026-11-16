using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// Вішается на зброю, яку можна класти в WeaponBag.
// Магнітить зброю у вільний слот сумки саме в момент відпускання з руки.
public class WeaponBagFlag : MonoBehaviour
{
    [Tooltip("Зброя зараз лежить у слоті сумки")]
    public bool isInBag = false;

    [Tooltip("Сумка, у тригерній зоні якої зараз знаходиться об'єкт (виставляється автоматично скриптом WeaponBag)")]
    [HideInInspector] public WeaponBag bagInZone;

    [Tooltip("Не використовується, залишено для сумісності зі старою сценою")]
    public float notGrabbedTime = 0.5f;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponentInParent<XRGrabInteractable>();

        // Якщо зброя вже лежить у сумці на момент старту гри (парент у ієрархії сумки),
        // позначаємо її як покладену і робимо kinematic, щоб вона не впала із сумки
        if (GetComponentInParent<WeaponBag>() != null)
        {
            isInBag = true;
            var startRb = GetComponent<Rigidbody>();
            if (startRb == null) startRb = GetComponentInChildren<Rigidbody>();
            if (startRb != null) startRb.isKinematic = true;
        }

        if (grabInteractable == null) return;

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
        // Якщо зброя все ще в руці (двухручний хват і відпустили лише одну руку) - нічого не робимо
        if (grabInteractable == null || grabInteractable.isSelected) return;

        // XRI після grab зі слота відновлює старий парент і isKinematic.
        // У зброї немає скрипта Magazine, який би це скидав, тому робимо це тут:
        // повертаємо зброю у "вільний" стан, щоб вона могла вільно падати
        transform.SetParent(null);
        var rb = GetComponent<Rigidbody>();
        if (rb == null) rb = GetComponentInChildren<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        // Якщо в момент відпускання зброя була у тригерній зоні сумки - магнітимо у вільний слот.
        // Якщо поза зоною або вільних слотів немає - зброя просто падає.
        if (isInBag || bagInZone == null) return;

        if (bagInZone.TryStoreItem(transform))
            grabInteractable.throwOnDetach = false; // зброя тепер kinematic, кидок не потрібен
    }

    public void FlagBagged() => isInBag = true;

    public void FlagGrabbed()
    {
        isInBag = false;
        bagInZone = null;
    }
}
