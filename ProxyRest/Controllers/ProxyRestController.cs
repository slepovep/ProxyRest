using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

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
		public string Post()
		{
			//находим входящий GET-запрос
			string query = this.Request.QueryString.ToString();
			//Task<string> content;
			HttpContent content;
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
			};

		}



	}
}
