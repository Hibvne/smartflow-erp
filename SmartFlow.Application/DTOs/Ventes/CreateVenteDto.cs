using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.DTOs.Ventes
{
    public class CreateVenteDto
    {
        public int ClientId { get; set; }
        public List<LigneVenteCreateDto> Lignes { get; set; }
    }
}
