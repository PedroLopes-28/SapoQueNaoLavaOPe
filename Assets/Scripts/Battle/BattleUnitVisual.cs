using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnitVisual : MonoBehaviour
{
    private BattleUnit unidade;
    private Animator animator;
    
    [SerializeField] private Slider barraVida;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text shieldCount;
    [SerializeField] private Transform debuffContainer;
    [SerializeField] private DebuffIconUI debuffIconPrefab;
    
    [SerializeField] private TMP_Text damageText;

    [SerializeField] private float duracaoFlash = 0.25f;
    [SerializeField] private float duracaoTexto = 0.8f;
    [SerializeField] private float distanciaTexto = 60f;

    private Coroutine coroutineFlash;
    private Coroutine coroutineTexto;

    private Color corBase;
    private Vector2 posicaoInicialTexto;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float duracaoEfeito = 0.5f;
    private Coroutine coroutineCor;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Inicializar(BattleUnit unidade)
    {
        
        // Remove uma inscrição anterior, se existir
        if (this.unidade != null)
            this.unidade.OnDanoRecebido -= ReceberFeedbackDano;
        
        this.unidade = unidade;


        this.unidade.OnDanoRecebido += ReceberFeedbackDano;
        
        corBase = spriteRenderer.color;

        posicaoInicialTexto = damageText.rectTransform.anchoredPosition;

        damageText.gameObject.SetActive(false);
        AtualizarVida();
    }
    
    private void OnDestroy()
    {
        if (unidade != null)
            unidade.OnDanoRecebido -= ReceberFeedbackDano;
    }
    
    
    private void ReceberFeedbackDano(int dano)
    {
        // Reinicia as animações anteriores
        if (coroutineFlash != null)
            StopCoroutine(coroutineFlash);

        if (coroutineTexto != null)
            StopCoroutine(coroutineTexto);

        coroutineFlash = StartCoroutine(PiscarPreto());
        coroutineTexto = StartCoroutine(MostrarDano(dano));
    }

    
    
    private IEnumerator PiscarPreto()
    {
        float tempo = 0f;

        while (tempo < duracaoFlash)
        {
            tempo += Time.deltaTime;

            float progresso = Mathf.Clamp01(
                tempo / duracaoFlash
            );

            // 0 -> 1 -> 0
            float intensidade = Mathf.Sin(
                progresso * Mathf.PI
            );

            spriteRenderer.color = Color.Lerp(
                corBase,
                Color.black,
                intensidade
            );

            yield return null;
        }

        spriteRenderer.color = corBase;
        coroutineFlash = null;
    }

    
    
    private IEnumerator MostrarDano(int dano)
    {
        damageText.gameObject.SetActive(true);

        damageText.text = dano.ToString();

        // Dano zero fica azul; demais danos ficam vermelhos
        Color corTexto = dano == 0
            ? new Color(0.6f, 0.8f, 1f)
            : new Color(1f, 0.35f, 0.35f);

        damageText.color = corTexto;

        RectTransform rect = damageText.rectTransform;

        float tempo = 0f;

        while (tempo < duracaoTexto)
        {
            tempo += Time.deltaTime;

            float progresso = Mathf.Clamp01(
                tempo / duracaoTexto
            );

            // Move o número para cima
            rect.anchoredPosition =
                posicaoInicialTexto +
                Vector2.up * distanciaTexto * progresso;

            // Diminui a opacidade gradualmente
            Color corAtual = corTexto;
            corAtual.a = 1f - progresso;

            damageText.color = corAtual;

            yield return null;
        }

        rect.anchoredPosition = posicaoInicialTexto;
        damageText.gameObject.SetActive(false);

        coroutineTexto = null;
    }




    public void AnimarEfeito(Color cor)
    {
        animator.SetTrigger("Attack");

        // Interrompe o efeito anterior
        if (coroutineCor != null)
        {
            StopCoroutine(coroutineCor);
        }

        coroutineCor = StartCoroutine(EfeitoCor(cor));
    }

    private IEnumerator EfeitoCor(Color cor)
    {
        Color corOriginal = Color.white;

        float tempo = 0f;

        // Transição: cor original -> cor do efeito
        while (tempo < duracaoEfeito / 2f)
        {
            tempo += Time.deltaTime;

            float progresso = Mathf.Clamp01(
                tempo / (duracaoEfeito / 2f)
            );

            spriteRenderer.color = Color.Lerp(
                corOriginal,
                cor,
                progresso
            );

            yield return null;
        }

        tempo = 0f;

        // Transição: cor do efeito -> cor original
        while (tempo < duracaoEfeito / 2f)
        {
            tempo += Time.deltaTime;

            float progresso = Mathf.Clamp01(
                tempo / (duracaoEfeito / 2f)
            );

            spriteRenderer.color = Color.Lerp(
                cor,
                corOriginal,
                progresso
            );

            yield return null;
        }

        // Garante que a cor volte ao normal
        spriteRenderer.color = corOriginal;

        coroutineCor = null;
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
        shieldCount.text = unidade.bloqueio.ToString();
    }
    
    
    public void AtualizarDebuffs()
    {
        // Remove os ícones antigos
        foreach (Transform filho in debuffContainer)
        {
            Destroy(filho.gameObject);
        }

        // Cria um ícone para cada debuff ativo
        foreach (DebuffEffect ativo in unidade.debuffs)
        {
            if (ativo == null || ativo.duracao <= 0)
                continue;

            DebuffIconUI icone = Instantiate(
                debuffIconPrefab,
                debuffContainer
            );

            icone.Inicializar(
                ativo.icone,
                ativo.duracao
            );
        }
    }

}
