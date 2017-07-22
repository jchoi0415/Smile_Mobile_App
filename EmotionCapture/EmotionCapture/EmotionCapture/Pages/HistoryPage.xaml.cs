using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmotionCapture
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryPage : ContentPage
	{
        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;

        public HistoryPage ()
		{
			InitializeComponent ();
		}

        async void GetEmotions(object sender, System.EventArgs e)
        {
            List<EmotionCaptureModel> EmotionInformation = await AzureManager.AzureManagerInstance.GetEmotionInformation();
            //EmotionInformation.Reverse();
            HotDogList.ItemsSource = EmotionInformation;
        }
    }
}