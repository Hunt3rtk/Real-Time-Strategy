using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class Tank : Unit
{

    private void Start()
    {
        unitName = "Tank";
        health = 200f;
        speed = 3f;
        attackPower = 20f;
        defensePower = 10f;
        cooldown = 1f;
        range = 7f;
    }

}

