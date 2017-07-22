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
            List<EmotionCaptureModel> info = await AzureManager.AzureManagerInstance.GetEmotionInformation();
            //info.Reverse();
            EmotionList.ItemsSource = info;
        }

        async void ClearHistory(object sender, System.EventArgs e)
        {
            var answer = await DisplayAlert("WARNING", "Are you sure you want to clear your history?", "Yes", "No");
            if(answer == true)
            {
                List<EmotionCaptureModel> info = await AzureManager.AzureManagerInstance.GetEmotionInformation();
                foreach (EmotionCaptureModel element in info)
                {
                    await AzureManager.AzureManagerInstance.DeleteHistory(element);
                }
                List<EmotionCaptureModel> newInfo = await AzureManager.AzureManagerInstance.GetEmotionInformation();
                EmotionList.ItemsSource = newInfo;
            }
        }
    }
}