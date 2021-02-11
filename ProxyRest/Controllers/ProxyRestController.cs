﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;

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
			var httpRequestMessage = new HttpRequestMessage();
			HttpClient client = new HttpClient();
			string body;
			var Urllocalgit = Configuration["UrlLocalGit"];

			using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
			{
				body = await reader.ReadToEndAsync();
			}


			foreach (var header in Request.Headers)
			{
				if (!header.Key.ToString().Contains("Content"))
				{
					client.DefaultRequestHeaders.Add(header.Key.ToString(), header.Value.ToString());
				}
			}

			HttpContent content = new StringContent(body, Encoding.UTF8);
			httpRequestMessage.Content = content;

			var response = await client.PostAsync(Urllocalgit, content);
			var responseString = await response.Content.ReadAsStringAsync();

			return responseString; 

		}
	}
}
