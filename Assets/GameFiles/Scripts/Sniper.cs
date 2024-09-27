using UnityEngine;
using UnityEngine.AI;

public class Sniper : Unit
{
    private void Start()
    {
        unitName = "Sniper";
        health = 80f;
        speed = 6f;
        attackPower = 15f;
        defensePower = 3f;
        range = 15f;
    }
}