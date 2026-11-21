
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebuffTooltipUI : MonoBehaviour
{
    [SerializeField] private RectTransform tooltipPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private Vector2 offset =
        new Vector2(15f, 15f);

    private Canvas canvas;
    private RectTransform canvasRect;

    private bool visivel;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>().rootCanvas;
        canvasRect = canvas.GetComponent<RectTransform>();

        tooltipPanel.gameObject.SetActive(false);
    }

   
    public void Mostrar(
        string titulo,
        string descricao,
        RectTransform icone)
    {
        titleText.text = titulo;
        descriptionText.text = descricao;

        tooltipPanel.gameObject.SetActive(true);

        // Posiciona uma única vez
        AtualizarPosicao(icone);
    }

    private void AtualizarPosicao(RectTransform icone)
    {
        Camera cameraUI = canvas.renderMode ==
                          RenderMode.ScreenSpaceOverlay
            ? null
            : canvas.worldCamera;

        // Obtém o centro do ícone na tela
        Vector3 posicaoTela = RectTransformUtility
            .WorldToScreenPoint(cameraUI, icone.position);

        // Converte para coordenadas locais do Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            posicaoTela,
            cameraUI,
            out Vector2 posicaoLocal
        );

        // Posiciona o Tooltip à direita do ícone
        Vector2 posicaoFinal = posicaoLocal + new Vector2(45f, 0f);

        Vector2 minimo = canvasRect.rect.min;
        Vector2 maximo = canvasRect.rect.max;
        Vector2 tamanho = tooltipPanel.rect.size;

        posicaoFinal.x = Mathf.Clamp(
            posicaoFinal.x,
            minimo.x,
            maximo.x - tamanho.x
        );

        posicaoFinal.y = Mathf.Clamp(
            posicaoFinal.y,
            minimo.y,
            maximo.y - tamanho.y
        );

        tooltipPanel.anchoredPosition = posicaoFinal;
    }


    

    public void Esconder()
    {
        visivel = false;
        tooltipPanel.gameObject.SetActive(false);
    }

    
 

}
