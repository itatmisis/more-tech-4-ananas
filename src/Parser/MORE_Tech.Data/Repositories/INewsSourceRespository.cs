using MORE_Tech.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORE_Tech.Data.Repositories
{
    public interface INewsSourceRespository
    {
        IQueryable<NewsSource> GetAllActive();

        IQueryable<NewsSource> Get(int count, int offset);
    }
}
