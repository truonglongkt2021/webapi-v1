using Newtonsoft.Json;
using System.Collections.Generic;

namespace StreamFile.Core.Models
{
    public class BaseModel<T> : BaseModel
    {
        public BaseModel()
        {
        }

        public BaseModel(T dataModel)
        {
            Data = dataModel;
        }

        [JsonProperty(Order = 5)]
        public T Data { get; set; }
    }

    public class BaseModel
    {
        [JsonProperty(Order = 6, NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty(Order = 7, NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        ///     Will be de-serialize as list property 
        /// </summary>
        [JsonProperty(Order = 8)]
        // [JsonExtensionData]
        public virtual Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
    }
}
