using UnityEngine;

public class BattleUnitVisual : MonoBehaviour
{
    private BattleUnit unidade;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Inicializar(BattleUnit unidade)
    {
        this.unidade = unidade;
        AtualizarVisual();
    }

    public void AtualizarVisual()
    {
        // Atualizar barra de vida, bloqueio etc.
    }

    public void AnimarAtaque()
    {
        animator.SetTrigger("Attack");
    }

    public void AnimarDano()
    {
        animator.SetTrigger("Hit");
    }
}