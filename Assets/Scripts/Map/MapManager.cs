
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class MapManager : MonoBehaviour
{
    [Header("Referência do mapa")]
    public Tilemap pathTilemap;

    [Header("Personagem")]
    public Sprite sapoSprite;

    [Header("Movimento do sapo")]
    public float duracaoPulo = 0.3f;
    public float alturaPulo = 0.8f;

    private List<Vector3> posicoesFases = new List<Vector3>();

    private Transform sapo;
    private int faseAtual = 0;
    private bool pulando = false;

    void Start()
    {
        if (pathTilemap == null)
        {
            Debug.LogError("Path_Tilemap não foi configurado!");
            return;
        }

        if (sapoSprite == null)
        {
            Debug.LogError("Sprite IDLE não foi configurado!");
            return;
        }

        if (GameManager.Gman == null || GameManager.Run == null)
        {
            Debug.LogError("GameManager ou Run não encontrados!");
            return;
        }

        EncontrarFases();

        if (posicoesFases.Count < 6)
        {
            Debug.LogError("O mapa precisa ter pelo menos 6 vitórias-régias!");
            return;
        }

        EncontrarProximaFase();

        CriarSapo();
    }

    void EncontrarFases()
    {
        posicoesFases.Clear();

        BoundsInt limites = pathTilemap.cellBounds;

        foreach (Vector3Int celula in limites.allPositionsWithin)
        {
            if (pathTilemap.HasTile(celula))
            {
                Vector3 posicao = pathTilemap.GetCellCenterWorld(celula);
                posicoesFases.Add(posicao);
            }
        }

        posicoesFases.Sort((a, b) =>
        {
            if (a.x != b.x)
                return a.x.CompareTo(b.x);

            return a.y.CompareTo(b.y);
        });

        Debug.Log("Vitórias-régias encontradas: " + posicoesFases.Count);
        Debug.Log("Fases disponíveis: " + (posicoesFases.Count - 1));
    }

    void EncontrarProximaFase()
    {
        List<string> concluidas = GameManager.Run.nosConcluidos;

        Debug.Log("Fases concluídas: " + string.Join(", ", concluidas));

        faseAtual = 0;

        for (int i = 1; i < posicoesFases.Count; i++)
        {
            string id = "no_" + i;

            if (concluidas.Contains(id))
            {
                faseAtual = i;
            }
            else
            {
                break;
            }
        }

        Debug.Log("Posição inicial do sapo: " + faseAtual);
    }

    void CriarSapo()
    {
        GameObject objetoSapo = new GameObject("SapoMapa");

        objetoSapo.transform.position =
            posicoesFases[faseAtual] + Vector3.up * 0.2f;

        SpriteRenderer renderer =
            objetoSapo.AddComponent<SpriteRenderer>();

        renderer.sprite = sapoSprite;
        renderer.sortingOrder = 10;
        renderer.transform.localScale = new Vector3(0.3f, 0.3f, 1f);

        sapo = objetoSapo.transform;
    }

    void Update()
    {
        if (pulando || sapo == null)
            return;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            Mover(-1);
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            Mover(1);
        }
    }

    void Mover(int direcao)
    {
        int destino = faseAtual + direcao;

        if (destino < 0 || destino >= posicoesFases.Count)
            return;

        faseAtual = destino;

        StartCoroutine(
            PularAte(posicoesFases[faseAtual] + Vector3.up * 0.2f)
        );
    }

    IEnumerator PularAte(Vector3 destino)
    {
        pulando = true;

        Vector3 inicio = sapo.position;
        float tempo = 0;

        while (tempo < duracaoPulo)
        {
            tempo += Time.deltaTime;

            float progresso = Mathf.Clamp01(tempo / duracaoPulo);

            Vector3 posicao = Vector3.Lerp(inicio, destino, progresso);

            posicao.y += Mathf.Sin(progresso * Mathf.PI) * alturaPulo;

            sapo.position = posicao;

            yield return null;
        }

        sapo.position = destino;

        pulando = false;

        EntrarNaFase();
    }

    void EntrarNaFase()
    {
        if (faseAtual == 0)
            return;

        string id = "no_" + faseAtual;

        // Impede entrar novamente em fases concluídas.
        if (GameManager.Run.nosConcluidos.Contains(id))
            return;

        Debug.Log("Entrando na fase: " + id);

        // A terceira fase é um evento.
        if (faseAtual == 3)
        {
            GameManager.Gman.EntraEvento(id);
        }
        else
        {
            GameManager.Gman.EntraCombate(id);
        }
    }
}
