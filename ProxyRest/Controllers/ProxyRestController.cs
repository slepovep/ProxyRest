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
					/*if (header.Key.ToString().Contains("CommitMes"))  
					{

						string commitmes = HttpUtility.HtmlEncode(Request.Headers["CommitMes"].ToString()); //обработка русских символов

						Encoding utf8 = Encoding.GetEncoding("UTF-8");
						Encoding win1251 = Encoding.GetEncoding("Windows-1251");

						byte[] utf8Bytes = win1251.GetBytes(commitmes);
						byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);

						commitmes = win1251.GetString(win1251Bytes);

						client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key.ToString(), commitmes);
						headerstr = headerstr + header.Key.ToString() + ": " + win1251Bytes + "\n";
					}
					else
					{
						client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key.ToString(), HttpUtility.HtmlEncode(header.Value)); 
					}*/
					//var utf8bytes = Encoding.UTF8.GetBytes(header.Value);
					//string MessageSignatureValue = Encoding.ASCII.GetString(utf8bytes);
					//client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key.ToString(), MessageSignatureValue);
						var header1 = header.Value.ToString();
						client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key.ToString(), HttpUtility.HtmlEncode(header1));
						headerstr = headerstr + header.Key.ToString() + ": " + HttpUtility.HtmlEncode(header1) + "\n";
				}
				//client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", @"text/html; charset=windows-1251");

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
		public string Encoding1251(string codein)
		{
			string codeout;
			Encoding utf8 = Encoding.GetEncoding("UTF-8");
			Encoding win1251 = Encoding.GetEncoding("Windows-1251");

			Encoding ascii = Encoding.ASCII;
			Encoding unicode = Encoding.Unicode;

			byte[] unicodeBytes = unicode.GetBytes(codein);

			// Perform the conversion from one encoding to the other.
			byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

			char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
			ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
			string asciiString = new string(asciiChars);



			//byte[] win1251Bytes = win1251.GetBytes(codein);
			//byte[] utf8Bytes = Encoding.Convert(utf8, win1251, win1251Bytes);

			//codeout = win1251.GetString(utf8Bytes);
			return asciiString;

		}
		/*
		public string StringToAscii(string codein)
		{
			string codeout = "";
			foreach (char c in codein)
			{
				codeout = codeout + Convert.ToInt32(c);
			}
			return codeout;
		}*/

	}
}
