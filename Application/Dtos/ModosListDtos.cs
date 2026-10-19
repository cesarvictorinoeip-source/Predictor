public class ModosListDtos
{
    public string Modo { get; set; }
    public double ValorPredicho { get; set; }
    public string Tendencia { get; set; }
    public Dictionary<string, string> Detalles { get; set; } = new Dictionary<string, string>();

    
    public List<FilaCalculo> TablaResultados { get; set; } = new List<FilaCalculo>();
}

public class FilaCalculo
{
    public string Indice { get; set; }
    public double Precio { get; set; }
    public string ValorCalculado { get; set; } 
}