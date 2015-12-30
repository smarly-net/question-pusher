using System.Threading.Tasks;

namespace PushAll.Service.Provider
{
	interface IDownloadProvider
	{
		Task<string> DownloadPage(string url);
	}
}
