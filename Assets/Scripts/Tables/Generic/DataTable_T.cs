using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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

    public class BigNumConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty (text)) 
                return new BigNum[0];
            return new BigNum(text);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            var target = value as BigNum?;
            if (target == null)
            {
                Debug.LogError($"target cannot be converted to BigNum returning 0");
                return "0";
            }

            return target.ToString();
        }
    }

    public class BigNumArrayConverTer : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
                return Array.Empty<BigNum>();

            var splitedTextArray = text.Split('/');
            BigNum[] bigNumArr = new BigNum[splitedTextArray.Length];


            for (int i = 0; i < splitedTextArray.Length; ++i)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var c in splitedTextArray[i])
                {
                    if (char.IsDigit(c))
                    {
                        sb.Append(c);
                    }
                }
                splitedTextArray[i] = sb.ToString().TrimStart('0');
                if (string.IsNullOrEmpty(splitedTextArray[i]))
                {
                    splitedTextArray[i] = "0";
                }
                bigNumArr[i] = new BigNum(splitedTextArray[i]);
            }

            return bigNumArr;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (!(value is BigNum[] bigNumArr))
            {
                return "0";
            }

            return string.Join("/", bigNumArr.Select(bigNum => bigNum.ToString()));
        }
    }

    public abstract class DataTableData
    {
        public int ID { get; set; }
    }

    public abstract class DataTable<T> : DataTable where T : DataTableData
    {
        // 필드 (Fields)
        protected Dictionary<int, T> m_dict = new Dictionary<int, T>();
        
        public T First => m_dict.First().Value;
        public Dictionary<int, T>.KeyCollection Keys => m_dict.Keys;
        public Dictionary<int, T>.ValueCollection Values => m_dict.Values;
        public List<T> ToList() => m_dict.Values.ToList();
        public T[] ToArray() => new SortedDictionary<int, T>(m_dict).Values.ToArray();
        public int Count => m_dict.Count;

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
                csvReader.Context.TypeConverterCache.AddConverter<BigNum>(new BigNumConverter());
                csvReader.Context.TypeConverterCache.AddConverter<BigNum[]>(new BigNumArrayConverTer());
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