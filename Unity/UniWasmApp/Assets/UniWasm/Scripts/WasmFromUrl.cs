using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace UniWasm
{
    public class WasmFromUrl : WasmBehaviour
    {
        [SerializeField]
        private string url;

        private void Awake()
        {
            if (!string.IsNullOrEmpty(url))
            {
                _ = LoadWasmFromUrl(url);
            }
        }

        public async Task LoadWasmFromUrl(string url)
        {
            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync(url);
            if (result.IsSuccessStatusCode)
            {
                var stream = await result.Content.ReadAsStreamAsync();
                LoadWasm(stream);
            }
        }
    }
}
