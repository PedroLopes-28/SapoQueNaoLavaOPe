using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "Events/EventData")]
public class EventData : ScriptableObject
{

    public int id;
    public string titulo;
    public string história;
    [SerializeField] public List<OptionData> opcoes;

}
