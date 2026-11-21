using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebuffIconUI : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] private Image imagem;
    [SerializeField] private TMP_Text duracao;

    private DebuffEffect debuff;
    private DebuffTooltipUI tooltip;

    public void Inicializar(
        DebuffEffect efeito,
        DebuffTooltipUI tooltipUI)
    {
        debuff = efeito;
        tooltip = tooltipUI;

        imagem.sprite = efeito.icone;
        imagem.preserveAspect = true;

        duracao.text = efeito.duracao.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (debuff != null && tooltip != null)
        {
            tooltip.Mostrar(
                debuff.nome,
                debuff.descricao,
                GetComponent<RectTransform>()
            );
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.Esconder();
    }

    private void OnDisable()
    {
        if (tooltip != null)
            tooltip.Esconder();
    }
}