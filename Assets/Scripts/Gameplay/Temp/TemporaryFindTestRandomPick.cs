using SkyDragonHunter;
using SkyDragonHunter.test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RandomPickType
{
    Crew,
    Cannon,
    Repairer,
    Spoils,
}

public class TemporaryFindTestRandomPick : MonoBehaviour
{
    [SerializeField] private bool m_IsFreePickMode = false;
    [SerializeField] private RandomPickType m_RandomPickType = RandomPickType.Crew;
    private TestRandomPick testRandomPick;


    void Start()
    {
        testRandomPick = GameObject.Find("TestRandomPick").gameObject.GetComponent<TestRandomPick>();
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        switch (m_RandomPickType)
        {
            case RandomPickType.Crew:
                if (m_IsFreePickMode)
                    gameObject.GetComponent<Button>().onClick.AddListener(testRandomPick.RandomFreePick);
                else
                    gameObject.GetComponent<Button>().onClick.AddListener(testRandomPick.RandomPick);
                break;
            case RandomPickType.Cannon:
                if (m_IsFreePickMode)
                    gameObject.GetComponent<Button>().onClick.AddListener(testRandomPick.RandomFreePickCannon);
                else
                    gameObject.GetComponent<Button>().onClick.AddListener(testRandomPick.RandomPickCannon);
                break;
            case RandomPickType.Repairer:
                if (m_IsFreePickMode)
                    gameObject.GetComponent<Button>().onClick.AddListener(testRandomPick.RandomFreePickRepairer);
                else
                    gameObject.GetComponent<Button>().onClick.AddListener(testRandomPick.RandomPickRepairer);
                break;
            case RandomPickType.Spoils:
                if (m_IsFreePickMode)
                    gameObject.GetComponent<Button>().onClick.AddListener(testRandomPick.RandomFreePickSpoils);
                else
                    gameObject.GetComponent<Button>().onClick.AddListener(testRandomPick.RandomPickSpoils);
                break;
        }
    }

}
