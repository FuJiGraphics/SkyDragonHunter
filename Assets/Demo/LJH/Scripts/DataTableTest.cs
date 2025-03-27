using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class DataTableTest : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log($"Started DataTable Test");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log($"Alpha1 Pressed");
                Debug.Log($"{DataTableManager.SampleTable.Get(101).ToString()}");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log($"{DataTableManager.SampleTable.Get(102).ToString()}");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log($"{DataTableManager.SampleTable.Get(103).ToString()}");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log($"{DataTableManager.SampleTable.Get(201).ToString()}");
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Debug.Log($"{DataTableManager.SampleTable.Get(202).ToString()}");
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Debug.Log($"{DataTableManager.SampleTable.Get(203).ToString()}");
            }
        }

    } // Scope by class DataTableTest

} // namespace Root