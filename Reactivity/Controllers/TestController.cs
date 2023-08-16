// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Linq;
// using System.Net.Http;
// using System.Text;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json;

// namespace Reactivity.Controllers
// {
//     [Route("api/[controller]")]
//     public class TestController : ControllerBase
//     {
//          private readonly HttpClient _httpClient = new HttpClient();

//     [HttpGet]
//     public async Task<ActionResult> MyAction()
//     {
//         var data = new { nic = "567654324V",
//                         user = "nusky" };

//         var json = JsonConvert.SerializeObject(data);
//         var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im51c2t5IiwibmJmIjoxNjgzMTc5NTc0LCJleHAiOjE2ODMxNzk2MDQsImlhdCI6MTY4MzE3OTU3NH0.C9Qpy8fi7FQfiPzxowsbedXGfIuqSxxLL_rBTtafqzoz0N18ZDzaI3cLo0wFSJxJYJT5UC5uOh2wV7KbGSFsAA";

//         var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:13001/api/vishwaportal/GetCustomerInfoByNIC");
//         request.Headers.Add("Authorization", "Bearer " + token);
//         request.Content = new StringContent(json, Encoding.UTF8, "application/json");

//         var response = await _httpClient.SendAsync(request);
//         Console.WriteLine(response);
//         var responseContent = await response.Content.ReadAsStringAsync();

//         Console.WriteLine("resonse - " + responseContent);
//         return Ok(responseContent);
//     }
//     }
// }