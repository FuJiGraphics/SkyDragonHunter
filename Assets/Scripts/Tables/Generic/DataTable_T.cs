using CsvHelper;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SkyDragonHunter.Tables.Generic {

    public class IntArrayConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, CsvHelper.Configuration.MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
                return new int[0];
            try
            {
                return text.Split('/')
                           .Select(s => int.Parse(s, CultureInfo.InvariantCulture))
                           .ToArray();
            }
            catch (Exception ex)
            {
                throw new TypeConverterException(this, memberMapData, text, row.Context, $"Failed to convert '{text}' to int[].", ex);
            }
        }
    }

    public abstract class DataTableData
    {
        public int ID { get; set; }
    }

    public abstract class DataTable<T> : DataTable where T : DataTableData
    {
        // 필드 (Fields)
        private Dictionary<int, T> m_dict = new Dictionary<int, T>();
        
        public T First => m_dict.First().Value;

        // Public 메서드
        [Obsolete("LoadCSV<T> Method is unavailable in DataTable<T>, please use non-generic LoadCSV instead", true)]
        new public static List<U> LoadCSV<U>(string csvFile)
        {
            return null;
        }

        public static List<T> LoadCSV(string csvFile)
        {
            using (var reader = new StringReader(csvFile))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Context.TypeConverterCache.AddConverter<int[]>(new IntArrayConverter());
                return csvReader.GetRecords<T>().ToList();
            }
        }

        public virtual T Get(int ID)
        {
            if(!m_dict.ContainsKey(ID))
            {
                Debug.Log($"Key {ID} does not exist in Table");
                return null;
            }

            return m_dict[ID];                        
        }

        public override void LoadFromText(string text)
        {
            var list = LoadCSV(text);
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
    } // Scope by class DataTable_T

} // namespace Root