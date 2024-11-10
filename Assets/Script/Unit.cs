using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private float unitHealt;
    public float unitMaxHealt;

    public HealthTracker healthTracker;
    // Start is called before the first frame update
    public void Init()
    {
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);

        unitHealt = unitMaxHealt;
        updateHealtUi();
    }

    private void updateHealtUi()
    {
        healthTracker.UpdateSliderValue(unitHealt, unitMaxHealt);
        if (unitHealt <= 0)
        {
            GameManager.instance.OnUnitKilled(gameObject);
           Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
    }

    internal void TakeDmage(int damageToInflict)
    {
        unitHealt -= damageToInflict;
        updateHealtUi() ;
    }
}
