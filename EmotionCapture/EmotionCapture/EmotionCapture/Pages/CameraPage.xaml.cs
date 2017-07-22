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
            currentEmotion.Text = "Setting Client DONE";
            byte[] byteData = GetImageAsByteArray(file);
            currentEmotion.Text = "Image to Byte Array";
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                currentEmotion.Text = "Setting MediaTypeHeader";
                response = await client.PostAsync(url, content);
                currentEmotion.Text = "CALLED FOR REQUEST";
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    //FaceObject responseModel = JsonConvert.DeserializeObject<FaceObject>(responseString);

                    //this.currentEmotion.Text = (responseModel.scores.happiness.ToString());
                    currentEmotion.Text = "You here son";
                }
                else
                {
                    currentEmotion.Text = "You Dun GOOFED";
                }
                file.Dispose();
            }
        }
    }
}