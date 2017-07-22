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

        [JsonProperty(PropertyName = "Anger")]
        public float Anger { get; set; }

        [JsonProperty(PropertyName = "Contempt")]
        public float Contempt { get; set; }

        [JsonProperty(PropertyName = "Disgust")]
        public float Disgust { get; set; }

        [JsonProperty(PropertyName = "Fear")]
        public float Fear { get; set; }

        [JsonProperty(PropertyName = "Happiness")]
        public float Happiness { get; set; }

        [JsonProperty(PropertyName = "Neutral")]
        public float Neutral { get; set; }

        [JsonProperty(PropertyName = "Sadness")]
        public float Sadness { get; set; }

        [JsonProperty(PropertyName = "Surprise")]
        public float Surprise { get; set; }
    }
}
