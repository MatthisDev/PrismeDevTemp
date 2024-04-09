using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quetetest : ScriptableObject
{
    private List<Quetetest> quetenecessaires;
    public Quetetest quetesuivante;
    public List<queteobjective> Queteobjectives;
    public int ID;
}

public class queteobjective
{
    public quetetype Quetetype;
    public int number;
}
public enum quetetype
{
    killmonster,
    recolterressoucres,
}