using System;
using Core;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Options/GanharOuro")]
public class GanharOuro : OptionData
{
    [SerializeField] private int ouro;
    public override void execute(GameManager manager)
    {
      //função de ganhar ouro
    }
}