using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.DTOs.Categories
{
    public class CreateCategorieDto
    {
        public string Nom { get; set; }
        public string Description { get; set; }
        public string Prefixe { get; set; }
    }
}
