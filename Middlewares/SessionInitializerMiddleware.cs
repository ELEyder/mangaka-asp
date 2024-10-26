using Mangaka.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mangaka.Middlewares
{
    public class SessionInitializerMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionInitializerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Session.GetString("userDataJson") == null)
            {
                User defaultSessionData = new User
                {
                    ID = 0,
                    Name = "Anónimo",
                    Email = "Anónimo@gmail.com",
                    Password = "1234",
                    DateCreate = DateTime.Now,
                    Status = true
                };

                string userDataJson = JsonSerializer.Serialize(defaultSessionData);
                context.Session.SetString("userDataJson", userDataJson);
            }

            await _next(context);
        }
    }
}
