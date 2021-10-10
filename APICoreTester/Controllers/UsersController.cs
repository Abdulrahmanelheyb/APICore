using System;
using System.Collections.Generic;
using System.Linq;
using APICore;
using APICore.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace APICoreTester.Controllers
{
    [Route("api/users")]
    public class UsersController
    {
        private readonly Configurations _configurations;
        public UsersController(Configurations config)
        {
            _configurations = config;
            _configurations.CreateDatabaseConnection();
        }
        
        private readonly Queries _query = new("users");


        [HttpPost("login")]
        public Response<Person> Login(Person model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Surname))
                {                    
                    return new Response<Person> { Status = true, Data = model, Message = Message.GetMessage(MessageTypes.Login, true) };
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return new Response<Person> { Status = false, Message = Message.GetMessage(MessageTypes.Exception)};
            }
        }

        [HttpPost("getall")]
        public Response<List<Person>> GetAll()
        {
            try
            {
                using var con = _configurations.CreateDatabaseConnection();
                var res = con.Query<Person>(_query.Select()).ToList();
                return new Response<List<Person>> { Status = true, Data = res, Message = Message.GetMessage(MessageTypes.GetAll, true)};
            }
            catch (Exception ex)
            {
                return new Response<List<Person>> { Status = false, Message = $"{Message.GetMessage(MessageTypes.GetAll, true)} {ex.Message}"};
            }
        }

        [HttpPost("get")]
        public Response<Person> Get(Person model)
        {
            try
            {
                using var con = _configurations.CreateDatabaseConnection();
                var res = con.QuerySingle<Person>(_query.Select(options: Queries.WhereQuery("Id", model.Id)));
                return new Response<Person> { Status = true, Data = res, Message = Message.GetMessage(MessageTypes.Get, true)};
            }
            catch (Exception ex)
            {
                return new Response<Person> { Status = false, Message = $"{Message.GetMessage(MessageTypes.Get)} {ex.Message}"};
            }
        }

        [HttpPost("add")]
        public Response<int> Add(Person model)
        {
            try
            {
                using var con = _configurations.CreateDatabaseConnection();
                var res = con.Execute(_query.Insert( new[]
                {
                    "Title",
                    "Description",
                    "ImageUrl"
                }), model);
                return res > 0 ? 
                    new Response<int> { Status = true, Data = res, Message = Message.GetMessage(MessageTypes.Add, true) } :
                    new Response<int> { Status = false, Data = res, Message = Message.GetMessage(MessageTypes.Add) };
            }
            catch (Exception)
            {
                return new Response<int> { Status = false, Message = Message.GetMessage(MessageTypes.Exception)};
            }
        }

        [HttpPost("update")]
        public Response<int> Update(Person model)
        {
            try
            {
                using var con = _configurations.CreateDatabaseConnection();
                var res = con.Execute(_query.Update( new []
                    {
                        "Title", 
                        "Description", 
                        "ImageUrl"
                    }, Queries.WhereQuery("Id", model.Id)), model);
                return res > 0 ? 
                    new Response<int> { Status = true, Data = res, Message = Message.GetMessage(MessageTypes.Update, true) } :
                    new Response<int> { Status = false, Data = res, Message = Message.GetMessage(MessageTypes.Update) };
            }
            catch (Exception)
            {
                return new Response<int> { Status = false, Message = Message.GetMessage(MessageTypes.Exception)};
            }
        }

        [HttpPost("delete")]
        public Response<int> Delete(Person model)
        {
            try
            {
                using var con = _configurations.CreateDatabaseConnection();
                var res = con.Execute(_query.Delete( Queries.WhereQuery("Id", model.Id)));
                return res > 0 ? 
                    new Response<int> { Status = true, Data = res, Message = Message.GetMessage(MessageTypes.Delete, true) } :
                    new Response<int> { Status = false, Data = res, Message = Message.GetMessage(MessageTypes.Delete) };
            }
            catch (Exception)
            {
                return new Response<int> { Status = false, Message = Message.GetMessage(MessageTypes.Exception)};
            }
        }
    }
}