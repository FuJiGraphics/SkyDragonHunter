using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad
{
    public class Vector3Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(Vector3);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var v = (Vector3)value;
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(v.x);
            writer.WritePropertyName("y");
            writer.WriteValue(v.y);
            writer.WritePropertyName("z");
            writer.WriteValue(v.z);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            return new Vector3(
                (float)jo["x"],
                (float)jo["y"],
                (float)jo["z"]
            );
        }
    }

    public class BigNumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(BigNum);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((BigNum)value).ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var s = reader.Value?.ToString() ?? "0";
            return new BigNum(s);
        }
    }

    //public class BigNumConverter : JsonConverter<BigNum>
    //{
    //    public override BigNum ReadJson(JsonReader reader, Type objectType, BigNum existingValue, bool hasExistingValue, JsonSerializer serializer)
    //    {
    //        var numString = Convert.ToString(reader.Value);
    //        if (string.IsNullOrEmpty(numString))
    //            return new BigNum(0);
    //
    //        return new BigNum(numString);
    //    }
    //
    //    public override void WriteJson(JsonWriter writer, BigNum value, JsonSerializer serializer)
    //    {
    //        writer.WriteValue(value.ToString());
    //    }
    //}
    public class ItemTableDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(ItemData);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int id = Convert.ToInt32(reader.Value);
            
            return DataTableMgr.ItemTable.Get((ItemType)id);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var item = (ItemData)value;
            writer.WriteValue((int)item.Type);  // ID 대신 Type enum 값(또는 item.ID) 사용
        }
    }

    //public class ItemTableDataConverter : JsonConverter<ItemData>
    //{
    //    public override ItemData ReadJson(JsonReader reader, Type objectType, ItemData existingValue, bool hasExistingValue, JsonSerializer serializer)
    //    {
    //        var id = Convert.ToInt32(reader.Value);
    //        return DataTableMgr.ItemTable.Get(id);
    //    }
    //    public override void WriteJson(JsonWriter writer, ItemData value, JsonSerializer serializer)
    //    {
    //        writer.WriteValue(value.ID);
    //    }
    //}

    public class CrewTableDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(CrewTableData);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int id = Convert.ToInt32(reader.Value);
            return DataTableMgr.CrewTable.Get(id);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var crew = (CrewTableData)value;
            writer.WriteValue(crew.ID);
        }
    }

    //public class CrewTableDataConverter : JsonConverter<CrewTableData>
    //{
    //    public override CrewTableData ReadJson(JsonReader reader, Type objectType, CrewTableData existingValue, bool hasExistingValue, JsonSerializer serializer)
    //    {
    //        var id = Convert.ToInt32(reader.Value);
    //        return DataTableMgr.CrewTable.Get(id);
    //    }
    //
    //    public override void WriteJson(JsonWriter writer, CrewTableData value, JsonSerializer serializer)
    //    {
    //        writer.WriteValue(value.ID);
    //    }
    //}
} // namespace Root