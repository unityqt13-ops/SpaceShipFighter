using UnityEngine;

public class ExplosionAutoDestroy : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator == null) return;

        AnimatorStateInfo state =
            animator.GetCurrentAnimatorStateInfo(0);

        if (state.normalizedTime >= 1f &&
            !animator.IsInTransition(0))
        {
            Destroy(gameObject);
        }
    }
}