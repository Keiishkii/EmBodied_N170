using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DataCollection
{
    public class DeserializeConvertor_DataCollectionEvent : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            string type = (string)jObject["type"];

            DataCollectionEvent_Interface dataCollectionEvent = null;
            switch (type)
            {
                case nameof(DataCollectionEvent_RecordMarker):
                {
                    dataCollectionEvent = new DataCollectionEvent_RecordMarker();
                } break;
            }
            
            serializer.Populate(jObject.CreateReader(), dataCollectionEvent);
            return dataCollectionEvent;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataCollectionEvent_Interface).IsAssignableFrom(objectType);
        }
    }
}
