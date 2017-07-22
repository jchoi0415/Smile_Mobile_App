using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmotionCapture
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CameraPage : ContentPage
	{
		public CameraPage ()
		{
			InitializeComponent();
		}

        async void TakePicture(object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            picture.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });

            currentEmotion.Text = "Analysing Facial Emotion...";
            await EmotionAnalyser(file);
        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task EmotionAnalyser(MediaFile file)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "664536650bae4da6889cd586eea2a941"); //5c14c67a270c4d0cae8a2e016745a23a
            string url = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize";
            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    DeserializeJSONAsync(responseString);
                }
                else
                {
                    currentEmotion.Text = "Something is wrong!";
                }
                file.Dispose();
            }
        }

        public async void DeserializeJSONAsync(string responseString)
        {
            var emotions = JsonConvert.DeserializeObject<EmotionModel[]>(responseString);
            var scores = emotions[0].scores;
            var highestScore = scores.Values.OrderByDescending(score => score).First();
            //probably a more elegant way to do this.
            var highestEmotion = scores.Keys.First(key => scores[key] == highestScore);
            if (highestEmotion == "happiness")
            {
                EmotionCaptureModel emotion = new EmotionCaptureModel()
                {
                    Happy = true,
                    Neutral = false,
                    Other = false
                };
                await AzureManager.AzureManagerInstance.AddEmotion(emotion);

                currentEmotion.Text = "YOU ARE SMILING, GOOD JOB!";
            }
            else if (highestEmotion == "neutral")
            {
                EmotionCaptureModel emotion = new EmotionCaptureModel()
                {
                    Happy = false,
                    Neutral = true,
                    Other = false
                };
                await AzureManager.AzureManagerInstance.AddEmotion(emotion);

                currentEmotion.Text = "TOO NEUTRAL, SMILE!";
            }
            else
            {
                EmotionCaptureModel emotion = new EmotionCaptureModel()
                {
                    Happy = false,
                    Neutral = false,
                    Other = true
                };
                await AzureManager.AzureManagerInstance.AddEmotion(emotion);

                currentEmotion.Text = "ARE YOU EVEN TRYING, MATE?";
            }
        }
    }
}