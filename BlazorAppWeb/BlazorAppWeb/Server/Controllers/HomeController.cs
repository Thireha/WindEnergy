using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BlazorAppWeb.Client.Pages.Index;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BlazorAppWeb.Server.Controllers
{
	[AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
	{
		private readonly HttpClient _httpClient;

		public HomeController()
		{
			_httpClient = new HttpClient();
		}

		[HttpGet]
		[Route("Getendpoint1DataAsync")]
		public async Task<ActionResult<ResponseModel>> Getendpoint1DataAsync()
		{
			ResponseModel data = new ResponseModel();
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var endpoint = new Uri("https://search.worldbank.org/api/v2/wds?format=json&display_title=wind%20energy&rows=100");
				response = await _httpClient.GetAsync(endpoint);

				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					data = JsonConvert.DeserializeObject<ResponseModel>(json);

                    return Ok(data);
				}
				else
				{
					return BadRequest($"Failed to fetch data: {response.ReasonPhrase}");
				}
			}
			catch (Exception ex)
			{
				data.ErrorMessage = $"Internal Server Error: {ex.Message}";
				return StatusCode(Int32.Parse((response.StatusCode).ToString()),data);;
			}
		}

	}
}
