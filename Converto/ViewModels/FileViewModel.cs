using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Converto.ViewModels
{
    public class FileViewModel
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string ConversionGuid { get; set; }
    }
}
