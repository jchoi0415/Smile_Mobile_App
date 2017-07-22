using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionCapture
{
    public class EmotionCaptureModel
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "Happy")]
        public Boolean Happy { get; set; }

        [JsonProperty(PropertyName = "Neutral")]
        public Boolean Neutral { get; set; }

        [JsonProperty(PropertyName = "Other")]
        public Boolean Other { get; set; }
    }
}
