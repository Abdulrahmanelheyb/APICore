using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Dapper;

namespace Core.Repository
{
    public class ServiceRepo: IRepository<Service>
    {
        private readonly Queries _query = new("services");
        
        public Response<List<dynamic>> GetAll()
        {
            try
            {
                using var con = Globals.GetConnection();
                var res = con.Query<dynamic>(_query.SelectQuery()).ToList();
                return new Response<List<dynamic>> { Status = true, Data = res, Message = Message.GetAll[1]};
            }
            catch (Exception ex)
            {
                return new Response<List<dynamic>> { Status = false, Message = $"{Message.Expection} {ex.Message}"};
            }
        }

        public Response<List<dynamic>> Get(Service entity)
        {
            try
            {
                using var con = Globals.GetConnection();
                var res = con.Query<dynamic>(_query.SelectQuery(options: Queries.WhereQuery("ID", entity.id))).ToList();
                return new Response<List<dynamic>> { Status = true, Data = res, Message = Message.Get[1]};
            }
            catch (Exception ex)
            {
                return new Response<List<dynamic>> { Status = false, Message = $"{Message.Expection} {ex.Message}"};
            }
        }

        public Response<dynamic> Add(Service entity)
        {
            try
            {
                using var con = Globals.GetConnection();
                var res = con.Execute(_query.InsertQuery( new[]
                {
                    "Title",
                    "Description",
                    "ImageUrl"
                }), entity);
                return res > 0 ? 
                    new Response<dynamic> { Status = true, Data = res, Message = Message.Add[1] } :
                    new Response<dynamic> { Status = false, Data = res, Message = Message.Add[0] };
            }
            catch (Exception ex)
            {
                return new Response<dynamic> { Status = false, Message = $"{Message.Expection} {ex.Message}"};
            }
        }

        public Response<dynamic> Update(Service entity)
        {
            try
            {
                using var con = Globals.GetConnection();
                var res = con.Execute(_query.UpdateQuery( new []
                    {
                        "Title", 
                        "Description", 
                        "ImageUrl"
                    }, Queries.WhereQuery("ID", entity.id)), entity);
                return res > 0 ? 
                    new Response<dynamic> { Status = true, Data = res, Message = Message.Update[1] } :
                    new Response<dynamic> { Status = false, Data = res, Message = Message.Update[0] };
            }
            catch (Exception ex)
            {
                return new Response<dynamic> { Status = false, Message = $"{Message.Expection} {ex.Message}"};
            }
        }

        public Response<dynamic> Delete(Service entity)
        {
            try
            {
                using var con = Globals.GetConnection();
                var res = con.Execute(_query.DeleteQuery( Queries.WhereQuery("ID", entity.id)));
                return res > 0 ? 
                    new Response<dynamic> { Status = true, Data = res, Message = Message.Delete[1] } :
                    new Response<dynamic> { Status = false, Data = res, Message = Message.Delete[0] };
            }
            catch (Exception ex)
            {
                return new Response<dynamic> { Status = false, Message = $"{Message.Expection} {ex.Message}"};
            }
        }
    }
}