using Newtonsoft.Json;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;

namespace SkyDragonHunter.SaveLoad
{
    public class BigNumConverter : JsonConverter<BigNum>
    {
        public override BigNum ReadJson(JsonReader reader, Type objectType, BigNum existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var numString = Convert.ToString(reader.Value);
            if (string.IsNullOrEmpty(numString))
                return new BigNum(0);

            return new BigNum(numString);
        }

        public override void WriteJson(JsonWriter writer, BigNum value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }

    public class ItemDataConverter : JsonConverter<ItemData>
    {
        public override ItemData ReadJson(JsonReader reader, Type objectType, ItemData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var id = Convert.ToInt32(reader.Value);
            return DataTableMgr.ItemTable.Get(id);
        }
        public override void WriteJson(JsonWriter writer, ItemData value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ID);
        }
    }

    public class CrewTableDataConverter : JsonConverter<CrewTableData>
    {
        public override CrewTableData ReadJson(JsonReader reader, Type objectType, CrewTableData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var id = Convert.ToInt32(reader.Value);
            return DataTableMgr.CrewTable.Get(id);
        }

        public override void WriteJson(JsonWriter writer, CrewTableData value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ID);
        }
    }
} // namespace Root