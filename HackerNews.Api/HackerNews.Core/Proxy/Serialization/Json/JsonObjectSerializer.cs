using System.Text;
using Newtonsoft.Json;

namespace HackerNews.Core.Proxy.Serialization.Json
{
    public class JsonObjectSerializer : IObjectSerializer
    {
        #region Private Members
        private const string contentType = "application/json";

        private static readonly Encoding encoding = Encoding.UTF8;

        private readonly JsonSerializerSettings _serializerSettings;
        #endregion

        #region Constructor
        public JsonObjectSerializer(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings;
        }
        #endregion


        public virtual string ContentType => contentType;

        public virtual Encoding Encoding => encoding;

        public virtual string Serialize<TObjectType>(TObjectType o)
        {
            return JsonConvert.SerializeObject(o, _serializerSettings);
        }

        public virtual TObjectType Deserialize<TObjectType>(string serializedObject)
        {
            return JsonConvert.DeserializeObject<TObjectType>(serializedObject, _serializerSettings);
        }
    }
}
