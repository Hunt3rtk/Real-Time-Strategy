using UnityEngine;
using UnityEngine.AI;

public class Soldier : Unit
{
    private void Start()
    {
        unitName = "Soldier";
        health = 100f;
        speed = 5f;
        attackPower = 10f;
        defensePower = 5f;
        range = 2f;
    }
}
