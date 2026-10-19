using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ModosDtos
    {
        public List<double> Valores { get; set; } = new List<double>();
        public string ModoSeleccionado { get; set; } = string.Empty;
    }
}
