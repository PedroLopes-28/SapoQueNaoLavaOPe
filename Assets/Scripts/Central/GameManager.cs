
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Gman { get; private set; }
        public static RunData Run { get; private set; }

        [SerializeField] public CardDatabase deckBase;

        public List<CardData> deckJogador;

        private void Awake()
        {
            if (Gman != null && Gman != this)
            {
                Destroy(gameObject);
                return;
            }

            Gman = this;

            DontDestroyOnLoad(gameObject);

            Run = new RunData();

            deckJogador = new List<CardData>(
                deckBase.buscaTodas()
            );
        }

        public void NovaRun()
        {
            Run = new RunData();

            deckJogador = new List<CardData>(
                deckBase.buscaTodas()
            );

            SceneManager.LoadScene("Map", LoadSceneMode.Single);
        }

        public void EntraCombate(string idNo)
        {
            Run.noAtual = idNo;

            SceneManager.LoadScene("Battle", LoadSceneMode.Single);
        }

        public void EntraEvento(string idEvento)
        {
            Run.noAtual = idEvento;

            SceneManager.LoadScene("Event", LoadSceneMode.Single);
        }

        public void EntraLoja(string idLoja)
        {
            Run.noAtual = idLoja;

            SceneManager.LoadScene("Loja", LoadSceneMode.Single);
        }

        public void FinalizaNo(string idNo, int HpRestante, int goldNovo)
        {
            if (!Run.nosConcluidos.Contains(idNo))
            {
                Run.nosConcluidos.Add(idNo);
            }

            Run.HP = HpRestante;
            Run.gold = goldNovo;

            bool todasConcluidas = true;

            for (int i = 1; i <= 5; i++)
            {
                string id = "no_" + i;

                if (!Run.nosConcluidos.Contains(id))
                {
                    todasConcluidas = false;
                    break;
                }
            }

            if (todasConcluidas)
            {
                SceneManager.LoadScene("Victory", LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene("Map", LoadSceneMode.Single);
            }
        }

        public void ReceberDano(int dano)
        {
            Run.HP = Mathf.Max(0, Run.HP - dano);
            
        }

        public int GastarOuro(int qtd)
        {
            if (Run.gold - qtd < 0)
            {
                return 1;
            }

            Run.gold = Mathf.Max(0, Run.gold - qtd);

            return 0;
        }
    }
}
