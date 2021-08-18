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

        //закрытый конструктор предотвращает создание классов
        private Singleton()
        {
        }

        //тут мы получим экземпляр класса
        public static Singleton GetInstance
        {
            get { return lazy.Value; }
        }

        public void GetSingletonThreadId() => Console.WriteLine($"Thread singleton - {Thread.CurrentThread.GetHashCode()}");

        //3 задание
        public async Task GetDoggyPickUrl()
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync("https://dog.ceo/api/breeds/image/random");
            var responseUrl = JsonConvert.DeserializeObject<DoggyModel>(await response.Content.ReadAsStringAsync()).Message;
            var imgUrl = await client.GetAsync(responseUrl);
            Regex regex = new Regex(@"https://images.dog.ceo/breeds/");
            string removeUrl = regex.Replace(responseUrl, "");
            string[] doggyInfo = Regex.Split(removeUrl, "/");
            
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + doggyInfo[0];

            response.EnsureSuccessStatusCode();
            await using var ms = await imgUrl.Content.ReadAsStreamAsync();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            await using var fs = File.Create(path + "\\" + doggyInfo[1]);
            ms.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(fs);
        }
        //Думаю по большей комментарии избыточны и в обычном проекте я не стал бы оставлять такое кол-во комментариев, т.к. код должен быть самодокументируемым, но поскольку это тестовое задание
    }
}
