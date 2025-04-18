using SkyDragonHunter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporaryFindWaveController2 : MonoBehaviour
{
    private TestWaveController waveController;

    void Start()
    {
        waveController = GameObject.Find("WaveController").gameObject.GetComponent<TestWaveController>();
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        gameObject.GetComponent<Button>().onClick.AddListener(waveController.OnOffInfiniteMod);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
