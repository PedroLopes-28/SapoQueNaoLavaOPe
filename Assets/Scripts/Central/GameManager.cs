using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
public class GameManager : MonoBehaviour
{
    public static GameManager Gman { get; private set; }
    public static RunData Run { get; private set; }


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
    }

    public void NovaRun()
    {
         Run = new RunData();

         SceneManager.LoadScene("Map");
    }
    
    public void EntraCombate(string idNo)
    {
        Run.noAtual = idNo;

        SceneManager.LoadScene("Battle");
    }

    public void EntraEvento(string idEvento)
    {
        Run.noAtual = idEvento;
        SceneManager.LoadScene("Event");
    }

    public void EntraLoja(string idLoja)
    {
        Run.noAtual = idLoja;  
        SceneManager.LoadScene("Loja");
    }

    public void FinalizaNo(string idNo,int HpRestante, int goldNovo)
    {
        Run.nosConcluidos.Append(idNo);
        SceneManager.LoadScene("Map");
        
    }

    public void ReceberDano(int dano)
    {
        Mathf.Max(
            0,
            Run.HP- dano
        );

        if (Run.HP<1)
        {
            //GameOver
        }
    }
    
    public int GastarOuro(int qtd)
    {

        if (Run.gold - qtd < 0)
        {
            return 1;
        }

        Mathf.Max(
            0,
            Run.gold-qtd
        );
        return 0;
    }

}

}
