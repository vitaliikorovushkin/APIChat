using ChatClientMvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChatClientMvc.Controllers
{
    public class ChatController : Controller
    { 
        
        public async Task<IActionResult> Index()
        {
            ChatViewModel viewModel = new ChatViewModel();
            string user = HttpContext.Session.GetString("CurrentUser");
            string id = user == "" ? "All" : user;

            using (HttpClient client = new HttpClient())
            {                
                var response = await client.GetAsync("https://localhost:44385/api/chat/index/"+ id);
                var data = await response.Content.ReadAsStringAsync();
                viewModel = JsonConvert.DeserializeObject<ChatViewModel>(data);               
                response.Dispose();
            }

            return View(viewModel);
        }

        //GET: Chat/Register
        public IActionResult Register()
        {
            return View();
        }

        //POST: Chat/Register
        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> RegisterAsync([FromForm]User user)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44385/api/chat/register", content);
                var data = await response.Content.ReadAsStringAsync();
                var userReceived = JsonConvert.DeserializeObject<IEnumerable<User>>(data);
                response.Dispose();
            }
            return RedirectToAction("Login");
        }

        //GET: Chat/Login
        public IActionResult Login()
        {
            return View();
        }

        //POST: Chat/Login
        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginAsync([FromForm] User user)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:44385/api/chat/login", content);
                var data = await response.Content.ReadAsStringAsync();
                var userReceived = JsonConvert.DeserializeObject<User>(data);
                HttpContext.Session.SetString("CurrentUser", userReceived.Name);
                //HttpContext.Session.SetString("CurrentUserId", userReceived.Id.ToString());
                response.Dispose();
            }
            return RedirectToAction("Index");
        }

        //POST: Chat/Logout
        [HttpPost]
        [ActionName("Logout")]
        public async Task<IActionResult> LogoutAsync([FromForm] User user)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PutAsync("https://localhost:44385/api/chat/logout/", content);
                response.Dispose();
            }
            HttpContext.Session.SetString("CurrentUser", string.Empty);
            //HttpContext.Session.SetString("CurrentUserId", string.Empty);

            return RedirectToAction("Index");
        }

        //POST: Chat/Send
        [HttpPost]
        [ActionName("Send")]
        public async Task<IActionResult> SendAsync([FromForm] ChatMessage msg)
        {
            msg.From = HttpContext.Session.GetString("CurrentUser");
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(msg), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44385/api/message/send", content);
                var data = await response.Content.ReadAsStringAsync();
                //var msgReceived = JsonConvert.DeserializeObject<IEnumerable<ChatMessage>>(data);               
               
                response.Dispose();
            }
            return RedirectToAction("Index");
        }
    }
}
