using CsvHelper;
using Org.BouncyCastle.Tls;
using SkyDragonHunter.Managers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SkyDragonHunter.Tables {

    public abstract class DataTable
    {
        // 필드 (Fields)
        public static readonly string FormatPath = "Tables/{0}";

        // Public 메서드
        public static List<T> LoadCSV<T>(string csvFile)
        {
            using (var reader = new StringReader(csvFile))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csvReader.GetRecords<T>().ToList();
            }
        }

        public void Load(string fileName)
        {
            var path = string.Format(FormatPath, fileName);
            var textAsset = Resources.Load<TextAsset>(path);
            LoadFromText(textAsset.text);
        }

        public abstract void LoadFromText(string text);
    } // Scope by class DataTable

} // namespace Root