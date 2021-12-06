// Created By abdulrahman elheyb
// 2021-12-02 8:37 AM

using System.Collections.Generic;
using APICore;

namespace Laboratory
{
    public class UserRepo:BaseRepository, IStandardRepository<SqlQueryBuilder>
    {
        public List<SqlQueryBuilder> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public SqlQueryBuilder Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public SqlQueryBuilder Add(SqlQueryBuilder model)
        {
            throw new System.NotImplementedException();
        }

        public SqlQueryBuilder Update(SqlQueryBuilder model)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}