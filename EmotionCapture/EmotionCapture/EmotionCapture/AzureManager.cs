using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace EmotionCapture
{
    class AzureManager
    {
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<EmotionCaptureModel> EmotionCaptureTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://emotioncapture.azurewebsites.net");
            this.EmotionCaptureTable = this.client.GetTable<EmotionCaptureModel>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task<List<EmotionCaptureModel>> GetEmotionInformation()
        {
            return await this.EmotionCaptureTable.ToListAsync();
        }

        public async Task AddEmotion(EmotionCaptureModel emotionCaptureModel)
        {
            await this.EmotionCaptureTable.InsertAsync(emotionCaptureModel);
        }

        public async Task DeleteHistory(EmotionCaptureModel emotionCaptureModel)
        {
            await this.EmotionCaptureTable.DeleteAsync(emotionCaptureModel);
        }
    }
}
