using System.Collections.Generic;
using Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DefeatUI : MonoBehaviour
{
    [SerializeField] GameObject defeatCanvas;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defeatCanvas.SetActive(false);
    }

    public void MostraDefeat()
    {
        defeatCanvas.SetActive(true);
    }

    public void escondeDefeat()
    {
        defeatCanvas.SetActive(false);
    }

    public void RetornaMenu()
    {
        GameManager.Gman.NovaRun();
    }
}
