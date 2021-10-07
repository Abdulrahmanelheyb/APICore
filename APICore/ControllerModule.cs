using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace APICore
{
    public abstract class ControllerModule<T>: ControllerBase
    {
        [HttpPost("getall")]
        public virtual Response<List<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpPost("get")]
        public virtual Response<T> Get()
        {
            throw new NotImplementedException();
        }

        [HttpPost("add")]
        public virtual Response<T> Add(T model)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("update")]
        public virtual Response<T> Update(T model)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("delete")]
        public virtual Response<T> Delete(T model)
        {
            throw new NotImplementedException();
        }
    }
}