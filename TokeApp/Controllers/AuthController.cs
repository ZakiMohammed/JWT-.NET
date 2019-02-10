using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TokeApp.Models;
using TokeApp.Repository;

namespace TokeApp.Controllers
{
    public class AuthController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Login(User user)
        {
            var repo = new UserRepository();
            var currentUser = repo.GetUser(user.Username);

            if (currentUser == null)
            {
                return NotFound();
            }            
            if (currentUser.Password != user.Password)
            {
                return NotFound();
            }
            return Ok(TokenManager.GenerateToken(currentUser.Username));
        }

        [HttpGet]
        public IHttpActionResult Validate(string token, string username)
        {
            var repo = new UserRepository();
            var user = repo.GetUser(username);
            if (user == null)
            {
                return NotFound();
            }

            var tokenUsername = TokenManager.ValidateToken(token);
            if (username == tokenUsername)
            {
                return Ok("success");
            }
            else
            {
                return BadRequest("token invalid");
            }
        }
    }
}
