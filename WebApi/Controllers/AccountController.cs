using DAL.DataFiles;
using DAL.DataFiles.OrdersRepository;
using DAL.DataFiles.UsersRepository;
using DAL.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IConfiguration config;
       

        public AccountController(IUsersRepository usersRepository, IOrdersRepository ordersRepository, IConfiguration conf)
        {
            config = conf;
            _usersRepository = usersRepository;
            _ordersRepository = ordersRepository;
          
        }

       // Get all users
        [HttpGet]
        [Route("[Action]")]
        [Authorize(Roles ="Manager")]
        public async Task<IActionResult> GetAllUsers()
        {
            var UsersList = await _usersRepository.GetAllUsers();
            List<UserDetailsModel> ReturnUsersList = new List<UserDetailsModel>();
            foreach (var user in UsersList)
            {
                UserDetailsModel u = new UserDetailsModel();
                u.Gender = user.Gender;
                u.FullName = user.FullName;
                u.UserID = user.UserID;
                u.Email = user.Email;              
                u.Roles = user.Roles;
                u.Picture = user.Picture;
                u.BirthDate = user.BirthDate;
                u.Gender = user.Gender;
                u.Tz = user.Tz;
                u.UserName = user.UserName;
                ReturnUsersList.Add(u);

            }
            return Ok(ReturnUsersList);
        }

        // Get User by ID
        [HttpGet]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _usersRepository.GetUserById(id);
            if(user != null)
            {
              
                UserDetailsModel u = new UserDetailsModel();
                u.Gender = user.Gender;
                u.FullName = user.FullName;
                u.UserID = user.UserID;
                u.Email = user.Email;
                u.Roles = user.Roles;
                u.Picture = user.Picture;
                u.BirthDate = user.BirthDate;
                u.Gender = user.Gender;
                u.Tz = user.Tz;
                u.UserName = user.UserName;
           

                return Ok(u);
            }
            else
            {
                return NotFound();
            }
        }

        // Register method
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromBody] User newUser)
        {

            var result = await _usersRepository.Register(newUser);
            if (result == "success")
            {
                return Created("",newUser);
            }
            else
            {
                return BadRequest(result);
            }
            
        }

        //Login Method
        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = await _usersRepository.GetUserByUsername(login.UserName);

            if (user !=null && user.Password==login.Password) 
            {
                try
                {
                    var securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["jwt:SecretKey"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    var claims = new[] 
                    {
                    new Claim(JwtRegisteredClaimNames.Sub,"User Private Info"),
                    new Claim(ClaimTypes.Name,user.UserID.ToString()),
                    new Claim(ClaimTypes.Role,user.Roles.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    };
                    var token = new JwtSecurityToken(issuer: config["jwt:Issuer"],
                        audience: config["jwt:Audience"], claims: claims,
                        expires: DateTime.Now.AddMinutes(90), signingCredentials: credentials);
                    string tok = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { 
                    Token=tok,
                    UserDetails = new
                    {
                        FullName=user.FullName,
                        Email=user.Email,
                        Picture=user.Picture
                    }
                    });

                }

                catch (Exception ex)
                {
                    return BadRequest(ex.InnerException.ToString());
                }

            }
            else
            {
                return Unauthorized(); 
            }
          
        }

        // Update User 
        [HttpPut]
        [Route("[Action]")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User UserToUpdate)
        {
            var ExistingUser = await _usersRepository.GetUserById(UserToUpdate.UserID);
            if (ExistingUser != null)
            {
                var result = await _usersRepository.UpdateUser(ExistingUser, UserToUpdate);
                if (result == "success")
                {
                    return Ok(ExistingUser);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE User
        [HttpDelete]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteUser(int id)
        {

            var user = await _usersRepository.GetUserById(id);
            if (user != null)
            {
                var OrdersForUser =await  _usersRepository.GetOrdersByUserId(id);
                if(OrdersForUser.Count >0)
                {
                    return BadRequest("Cant remove user,there are orders for this user");
                }
                else
                {
                    var result = await _usersRepository.DeleteUser(user);
                    if (result == "success")
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                   
                }              
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("[Action]")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("wwwroot", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = "http://localhost:31946/Images/" + fileName;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // GET orders by userID
        [HttpGet]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Manager,Worker,Customer")]
        public async Task<IActionResult> GetOrdersById(int id)
        {
            var orders = await _usersRepository.GetOrdersByUserId(id);
            if (orders != null)
            {
                return Ok(orders);
            }
            else
            {
                return NotFound();
            }
        }
    }

}
