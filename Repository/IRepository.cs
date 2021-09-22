using System.Collections.Generic;

namespace Core.Repository
{
    public interface IRepository<in T>
    {
        public Response<List<dynamic>> GetAll();
        public Response<List<dynamic>> Get(T entity);
        public Response<dynamic> Add(T entity);
        public Response<dynamic> Update(T entity);
        public Response<dynamic> Delete(T entity);
    }
}
