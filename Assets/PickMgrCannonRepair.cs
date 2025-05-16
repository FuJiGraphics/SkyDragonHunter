using SkyDragonHunter.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickMgrCannonRepair : MonoBehaviour
{
    [SerializeField] private Button cannonOnePickButton;
    [SerializeField] private Button cannonTenPickButton;
    [SerializeField] private Button repairOnePickButton;
    [SerializeField] private Button repairTenPickButton;

    public Transform crewAndMoonStone;

    private void Start()
    {
        AddListeners();
    }

    private void AddListeners()
    {
        cannonOnePickButton.onClick.AddListener(OnClickCannonOnePick);
        cannonTenPickButton.onClick.AddListener(OnClickCannonTenPick);
        repairOnePickButton.onClick.AddListener(OnClickReapirOnePick);
        repairTenPickButton.onClick.AddListener(OnClickReapirTenPick);
    }

    private void OnClickCannonOnePick()
    {
        

    }

    private void OnClickCannonTenPick()
    {

    }

    private void OnClickReapirOnePick()
    {

    }
    private void OnClickReapirTenPick()
    {

    }
}
