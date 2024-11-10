using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int healt;
    internal void TakeDmage(int damageInInflict)
    {
        healt -= damageInInflict;
    }

   
}
