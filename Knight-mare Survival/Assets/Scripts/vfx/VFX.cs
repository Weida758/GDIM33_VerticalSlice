using UnityEngine;

public class VFX : MonoBehaviour
{
    public enum LifetimeMode { Animation, Timed, Manual }

    public LifetimeMode lifetimeMode = LifetimeMode.Animation;
    public float duration = 1f;

    private Animator animator;
    private float timer;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if (lifetimeMode == LifetimeMode.Timed)
        {
            timer = duration;
        }
        else if (lifetimeMode == LifetimeMode.Animation && animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
            timer = animator.GetCurrentAnimatorStateInfo(0).length;
        }
    }

    void Update()
    {
        if (lifetimeMode == LifetimeMode.Manual) return;

        timer -= Time.deltaTime;
        if (timer <= 0f) Release();
    }

    public void Release()
    {
        VFXPool.Instance.Release(gameObject);
    }
}