using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionCapture
{
    public class EmotionModel
    {
        public FaceRectangle faceRectangle { get; set; }
        public IDictionary<string, double> scores { get; set; }
    }

    public class FaceRectangle
    {
        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
