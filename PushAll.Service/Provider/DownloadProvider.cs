using System.Net.Http;
using System.Threading.Tasks;

namespace PushAll.Service.Provider
{
	class DownloadProvider : IDownloadProvider
	{
		public async Task<string> DownloadPage(string url)
		{
			using (var client = new HttpClient())
			{
				return await client.GetStringAsync(url);
			}
		}
	}
}
