using SkyDragonHunter.Gameplay;
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
        stats.maxHP = HP;
        stats.currentHP = stats.maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
