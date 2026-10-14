using System;
using System.Collections.Generic;
using Unity.VisualScripting;


//armazena todos os dados da run no momento
[Serializable]
public class RunData
{
    public int HP = 100;
    public int gold = 50;
    public int noAtual = 0;
    
    
    public List<string> deck= new List<string>(); //cartas
    public List<string> nosConcluidos= new List<string>();// nos que já foram feitos
    public List<string> legendary= new List<string>(); // cartas de custo 0, pode se escolher uma a cada turno para jogar
    public List<string> blessings = new List<string>(); //efeitos passivos permanentes

}