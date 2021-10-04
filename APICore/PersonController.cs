using System;
using System.Collections.Generic;
using System.Linq;
using APICore.Models;
using Dapper;

namespace APICore
{
    public class PersonRepo: ControllerModule<Person>
    {
        private readonly Queries _query = new("persons");
        
        public override Response<List<Person>> GetAll()
        {
            try
            {
                using var con = Globals.GetConnection();
                var res = con.Query<Person>(_query.Select()).ToList();
                return new Response<List<Person>> { Status = true, Data = res, Message = Message.GetAll[1]};
            }
            catch (Exception ex)
            {
                return new Response<List<Person>> { Status = false, Message = $"{Message.Expection} {ex.Message}"};
            }
        }

        public override Response<Person> Get(Person model)
        {
            try
            {
                using var con = Globals.GetConnection();
                var res = con.QuerySingle<Person>(_query.Select(options: Queries.WhereQuery("Id", model.Id)));
                return new Response<Person> { Status = true, Data = res, Message = Message.Get[1]};
            }
            catch (Exception ex)
            {
                return new Response<Person> { Status = false, Message = $"{Message.Expection} {ex.Message}"};
            }
        }

        public new Response<int> Add(Person model)
        {
            try
            {
                using var con = Globals.GetConnection();
                var res = con.Execute(_query.Insert( new[]
                {
                    "Title",
                    "Description",
                    "ImageUrl"
                }), model);
                return res > 0 ? 
                    new Response<int> { Status = true, Data = res, Message = Message.Add[1] } :
                    new Response<int> { Status = false, Data = res, Message = Message.Add[0] };
            }
            catch (Exception ex)
            {
                return new Response<int> { Status = false, Message = $"{Message.Expection} {ex.Message}"};
            }
        }

        public new Response<int> Update(Person model)
        {
            try
            {
                using var con = Globals.GetConnection();
                var res = con.Execute(_query.Update( new []
                    {
                        "Title", 
                        "Description", 
                        "ImageUrl"
                    }, Queries.WhereQuery("Id", model.Id)), model);
                return res > 0 ? 
                    new Response<int> { Status = true, Data = res, Message = Message.Update[1] } :
                    new Response<int> { Status = false, Data = res, Message = Message.Update[0] };
            }
            catch (Exception ex)
            {
                return new Response<int> { Status = false, Message = $"{Message.Expection} {ex.Message}"};
            }
        }

        public new Response<int> Delete(Person model)
        {
            try
            {
                using var con = Globals.GetConnection();
                var res = con.Execute(_query.Delete( Queries.WhereQuery("Id", model.Id)));
                return res > 0 ? 
                    new Response<int> { Status = true, Data = res, Message = Message.Delete[1] } :
                    new Response<int> { Status = false, Data = res, Message = Message.Delete[0] };
            }
            catch (Exception ex)
            {
                return new Response<int> { Status = false, Message = $"{Message.Expection} {ex.Message}"};
            }
        }
    }
}