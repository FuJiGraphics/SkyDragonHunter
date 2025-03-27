using SkyDragonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables 
{
    public class SampleDataTable : DataTable
    {
        public class Data
        {
            public ID ID { get; set; }
            public string Name { get; set; }
            public int Value { get; set; }
        }

        private Dictionary<ID, Data> m_dict = new Dictionary<ID, Data>();

        public override void LoadFromText(string text)
        {
            var list = LoadCSV<Data>(text);
            m_dict.Clear();

            foreach (var item in list)
            {
                if(!m_dict.ContainsKey(item.ID))
                {
                    m_dict.Add(item.ID, item);
                }
                else
                {
                    Debug.LogError($"Key {item.ID} exists already");
                }
            }
        }
    } // Scope by class SampleDataTable

} // namespace Root