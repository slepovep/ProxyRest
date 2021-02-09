using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Text;


//using Microsoft.AspNetCore.


namespace ProxyRest.Controllers
{
    [ApiController]
    [Route("Redirect")]
	public class ProxyRestController : ControllerBase
	{
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
		public static async Task<string> Post(/*HttpContent content*/)
		{
			var httpRequestMessage = new HttpRequestMessage();
			HttpClient client = new HttpClient();
			HttpContent content = new StringContent("pJsonContent", Encoding.UTF8, "application/json");
			httpRequestMessage.Content = content;

			//var content = "new FormUrlEncodedContent(values)";

			var response = await client.PostAsync("http://www.example.com/recepticle.aspx", content);

			var responseString = await response.Content.ReadAsStringAsync();

			/*var req = context.HttpContext.Request;
			var request = context.HttpContext.Request;
			var stream = new StreamReader(request.Body);
			var body = stream.ReadToEnd();

			HttpContext.Request.

			HttpContent requestContent = Request.Content;
			string jsonContent = requestContent.ReadAsStringAsync().Result;

			//находим входящий GET-запрос
			string query = this.Request.QueryString.ToString();
			//Task<string> content;
			HttpContent content;

			var httpContent = this.Request.GetTypeContent;
			content = this.Request.Query.

			if (string.IsNullOrEmpty(query))
			{
				return "Не введен адреса запроса";
			}

			//отсекаем первый ?
			string address_url = query;
			HttpClient client = new HttpClient();

			try
			{
				//HttpResponseMessage
				HttpResponseMessage result = client.PostAsync("url1", content);
				//await client.PostAsync.(this.Request, content); 
				return result.ToString();
			}
			catch
			{
				return "Ошибка запроса:" + "\n" + address_url;
			};*/

			//HttpClient client = new HttpClient();
			//string query = this.Request.QueryString.ToString();

			//HttpContent content = new StringContent("this.Request.QueryString.ToString()");
			//HttpResponseMessage response = await client.PostAsync("url", content).ConfigureAwait(false);
			//return await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			//string result = await this.Request.Content.ReadAsStringAsync();
			//return responseString;

			return await client.SendAsync(httpRequestMessage).ToString();



		}
	}
}
