using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;
using System;
using System.Web;

namespace ProxyRest.Controllers
{
    [ApiController]
    [Route("Redirect")]
	public class ProxyRestController : ControllerBase
	{

		private readonly IConfiguration Configuration;

		public ProxyRestController(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		[HttpGet]
		public string Get()
		{
			//находим входящий GET-запрос
			string query = this.Request.QueryString.ToString();
			Task<string> content;

			if (query == null | query.Length == 1)
			{
				return "Не введен адреса запроса";
			}

			//отсекаем первый ?
			string address_url = query.Substring(1);
			HttpClient client = new HttpClient();

			try
			{
				content = client.GetStringAsync(address_url);
				return content.Result;
			}
			catch
			{
				return "Ошибка запроса:" + "\n" + address_url;
			};

		}

		[HttpPost]
		public async Task<string> Post()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			string body;
			string headerstr = "";
			HttpClient client = new HttpClient();
			var httpRequestMessage = new HttpRequestMessage();
			var Urllocalgit = Configuration["UrlLocalGit"];
			
			try
			{
				using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
				{
					body = await reader.ReadToEndAsync();
				}
			}
			catch
			{
				return "Ошибка считывания тела запроса";
			}

			try
			{ 
				foreach (var header in Request.Headers)
				{
					string headervalue;
					if ( header.Key.ToString().Contains("CommitMes") | header.Key.ToString().Contains("Directory") )
					{
					   headervalue = Base64Encode(header.Value.ToString());
					}
					else
					{
						headervalue = header.Value;
					}
					client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key.ToString(), headervalue);
					headerstr = headerstr + header.Key.ToString() + ": " + headervalue + "\n";
				}
				client.DefaultRequestHeaders.TryAddWithoutValidation("HeaderBase64", "1");  //добавляем параметр указывающий на кодировку заголовка
				HttpContent content = new StringContent(body, Encoding.UTF8);
				httpRequestMessage.Content = content;

				var response = await client.PostAsync(Urllocalgit, content);
				var responseString = await response.Content.ReadAsStringAsync();
				return responseString;
			}
			catch (Exception ex)
			{
				return "Ошибка обработки запроса: " + ex.Message  + "\n"+ headerstr;
			}

		}

		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

	}
}
