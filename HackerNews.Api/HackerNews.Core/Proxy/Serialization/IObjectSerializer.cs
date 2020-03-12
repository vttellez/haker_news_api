using System.Text;

namespace HackerNews.Core.Proxy.Serialization
{
    public interface IObjectSerializer
    {
        /// <summary>
        /// Gets or sets content type 
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Gets or sets encoding
        /// </summary>
        Encoding Encoding { get; }

        string Serialize<TObjectType>(TObjectType o);

        TObjectType Deserialize<TObjectType>(string serializedObject);
    }
}
