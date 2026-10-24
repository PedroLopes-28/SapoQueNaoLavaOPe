using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnitVisual : MonoBehaviour
{
    private BattleUnit unidade;
    private Animator animator;
    
    [SerializeField] private Slider barraVida;
    [SerializeField] private TMP_Text hpText;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Inicializar(BattleUnit unidade)
    {
        this.unidade = unidade;
        AtualizarVida();
    }
    

    public void AnimarAtaque()
    {
        animator.SetTrigger("Attack");
    }

    public void AnimarDano()
    {
        animator.SetTrigger("Hit");
    }
    
    public void AtualizarVida()
    {
        if (unidade == null)
            return;

        barraVida.maxValue = unidade.vidaMaxima;
        barraVida.SetValueWithoutNotify(unidade.vidaAtual);

        hpText.text =
            $"{unidade.vidaAtual}/{unidade.vidaMaxima}";
    }
}
