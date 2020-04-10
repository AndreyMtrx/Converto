using Converto.Data.Interfaces;
using Converto.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Converto.Data.Repositories
{
    public class ConversionRepository : IConversionRepository
    {
        private ApplicationDbContext dbContext;
        public ConversionRepository(ApplicationDbContext context)
        {
            dbContext = context;
        }
        public IQueryable<Conversion> Conversions => dbContext.Conversions;
    }
}
