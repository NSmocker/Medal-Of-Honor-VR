using UnityEngine;
using MikeNspired.XRIStarterKit;

/// <summary>
/// Sets an Animator bool (default "Dead") the first time this object takes damage.
/// Implements IDamageable so bullets (SimpleCollisionDamage) detect it the same way
/// they detect the Zombie's HitBox.
/// </summary>
public class HeadHitDeath : MonoBehaviour, IDamageable
{
    [Tooltip("Animator bool parameter set to true on the first hit.")]
    [SerializeField] private string deadParameter = "Dead";

    private Animator animator;
    private bool isDead;

    private void Awake()
    {
        // The Animator normally sits on the character root; search up first, then from the root down.
        animator = GetComponentInParent<Animator>();
        if (animator == null)
        {
            Transform root = transform;
            while (root.parent != null) root = root.parent;
            animator = root.GetComponentInChildren<Animator>(true);
        }
    }

    public void TakeDamage(float damage, GameObject damager)
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
            animator.SetBool(deadParameter, true);
    }
}
