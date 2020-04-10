using Converto.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Converto.Data.Interfaces
{
    public interface IConversionRepository
    {
        public IQueryable<Conversion> Conversions { get; }
    }
}
