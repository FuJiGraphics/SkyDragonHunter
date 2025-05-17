using SkyDragonHunter;
using SkyDragonHunter.test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporaryFindTestRandomPick2 : MonoBehaviour
{
    [SerializeField] private RandomPickType m_RandomPickType = RandomPickType.Crew;
    private TestRandomPick testRandomPick;

    void Start()
    {
        testRandomPick = GameObject.Find("TestRandomPick").gameObject.GetComponent<TestRandomPick>();
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        switch (m_RandomPickType)
        {
            case RandomPickType.Crew:
                gameObject.GetComponent<Button>().onClick.AddListener(testRandomPick.RandomTenPick);
                break;
            case RandomPickType.Cannon:
                gameObject.GetComponent<Button>().onClick.AddListener(testRandomPick.RandomTenPickCannon);
                break;
            case RandomPickType.Repairer:
                gameObject.GetComponent<Button>().onClick.AddListener(testRandomPick.RandomTenPickRepairer);
                break;
            case RandomPickType.Spoils:
                gameObject.GetComponent<Button>().onClick.AddListener(testRandomPick.RandomTenPickSpoils);
                break;
        }
    }

}
