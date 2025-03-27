using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusModifyTest : MonoBehaviour
{
    // TEMPORARY SCRIPT FOR TEST ONLY, CAN BE DELETED FREELY ANYTIME WITHOUT PERMISSION
    [SerializeField] private double HP = 50;

    void Start()
    {
        var stats = GetComponent<CharacterStatus>();
        var bars = GetComponentsInChildren<UIHealthBar>();
        foreach (var bar in bars)
        {
            if (bar.name == "UIShieldBar")
            {
                bar.maxHealth = 100;
                bar.ResetHP();
            }
            else if (bar.name == "UIHealthBar")
            {
                bar.maxHealth = HP;
                bar.ResetHP();
            }
        }
        stats.maxHP = HP;
        stats.currentHP = stats.maxHP;
    }

    // Update is called once per frame
}
