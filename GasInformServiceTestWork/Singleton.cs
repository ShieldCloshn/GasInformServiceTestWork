using GasInformServiceTestWork.Managers;
using GasInformServiceTestWork.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GasInformServiceTestWork
{
    //Ленивая реализация подразумевает создание экземпляра только по необходимости
    public class Singleton
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly Lazy<Singleton> lazy = new Lazy<Singleton>(() => new Singleton());

        private static readonly string removedSymb = "/";

        //закрытый конструктор предотвращает создание экземпляров класса
        private Singleton()
        {
        }

        //Экземпляр класса
        public static Singleton GetInstance
        {
            get { return lazy.Value; }
        }

        public int GetSingletonThreadId() => Thread.CurrentThread.GetHashCode(); 

        //3 задание
        public async Task GetDoggyPickUrl()
        {
            var test =  Thread.CurrentThread;
            var dogApiSettings = GetDogAppiSettings();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(dogApiSettings.MediaTypeSettings));
            var response = await client.GetAsync(dogApiSettings.DogApiLink);
            var responseUrl = JsonConvert.DeserializeObject<DogApiResponseModel>(await response.Content.ReadAsStringAsync()).Message;

            if (response.IsSuccessStatusCode)
                await CreateDoggyFile(responseUrl);
            else
                Console.WriteLine("Ошибка при попытке получить ответ от DogApi");
        }

        //Получение данных из settings.json
        private DogAppiSettingsModel GetDogAppiSettings()
        {
            var configuration = ConfigurationManager.Build();
            var dogApiUrl = configuration["dogApiConfig:DogApiLink"];
            var dogApiMediaType = configuration["dogApiConfig:MediaTypeSettings"];
            var remodedLinkPart = configuration["dogApiConfig:RemovedLinkPart"];

            return new DogAppiSettingsModel(dogApiUrl, dogApiMediaType, remodedLinkPart);
        }

        //разбитие результата ответа с Api на необходимые составные части.
        private DoggyInfoModel ParseUrl(string url)
        {
            var dogApiSettings = GetDogAppiSettings();
            var regex = new Regex(dogApiSettings.RemovedLinkPart);
            var removeUrl = regex.Replace(url, "");
            var dogInfoSplit = Regex.Split(removeUrl, removedSymb);
            var doggyInfo = new DoggyInfoModel(dogInfoSplit[0], dogInfoSplit[1]);
            return doggyInfo;
        }

        private string GetPatch(string lastPartOfPath, string fileName)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path = Path.Combine(path, lastPartOfPath);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return Path.Combine(path, fileName);
        }

        private async Task CreateDoggyFile(string imgUrl)
        {
            var doggyInfo = ParseUrl(imgUrl);
            var path = GetPatch(doggyInfo.Breed, doggyInfo.FileName);

            var response = await client.GetAsync(imgUrl);
            await using var steam = await response.Content.ReadAsStreamAsync();
            await using var fs = File.Create(path);
            steam.Seek(0, SeekOrigin.Begin);
            steam.CopyTo(fs);
            Console.WriteLine($"Файл сохранён по пути {path}");
        }


    }
}
