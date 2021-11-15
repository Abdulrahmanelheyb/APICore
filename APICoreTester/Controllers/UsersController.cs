using System.Collections.Generic;
using APICore;
using APICore.Security;
using APICoreTester.Models;
using APICoreTester.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICoreTester.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController
    {
        private readonly UserRepo _repo;

        public UsersController()
        {
            _repo = new UserRepo();
        }

        [HttpPost("login")]   
        public Response<User> Login(User model)
        {
            return _repo.Login(model);
        }

        [HttpPost("getall")]
        public Response<List<User>> GetAll()
        {
            return _repo.GetAll();
        }

        [HttpPost("get")]
        public Response<User> Get(User model)
        {
            return _repo.Get(model);
        }

        [HttpPost("add")]
        public Response<dynamic> Add(User model)
        {
            return _repo.Add(model);
        }

        [HttpPost("update")]
        public Response<dynamic> Update(User model)
        {
            return _repo.Update(model);
        }

        [HttpPost("delete")]
        public Response<dynamic> Delete(User model)
        {
            return _repo.Delete(model);
        }
    }
}