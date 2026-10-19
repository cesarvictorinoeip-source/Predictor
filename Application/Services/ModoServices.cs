using Aplicacion.ViewModels;
using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class ModoServices
    {
        public ModoListViewModel modoList { get; set; }

        public ModoServices()
        {
            modoList = new ModoListViewModel
            {
                Modos = new List<NewModoView>()
            };
        }

        public ModosListDtos CalcularDetallado(ModosDtos dto)
        {
            var resultado = new ModosListDtos { Modo = dto.ModoSeleccionado };

            if (dto.Valores == null || dto.Valores.Count < 2) return resultado;

            double ultimoValor = dto.Valores.Last();

            switch (dto.ModoSeleccionado)
            {
                case "Media Móvil Simple (SMA) Crossover":
                    double smaCorta = dto.Valores.TakeLast(5).Average();
                    double smaLarga = dto.Valores.Average();
                    resultado.ValorPredicho = smaCorta;
                    resultado.Tendencia = smaCorta > smaLarga ? "alcista" : "bajista";

                    resultado.Detalles.Add("SMA corta (5p)", smaCorta.ToString("N2"));
                    resultado.Detalles.Add("SMA larga (20p)", smaLarga.ToString("N2"));
                    resultado.Detalles.Add("Diferencial", (smaCorta - smaLarga).ToString("N2"));

                    // Generar tabla para SMA
                    for (int i = 0; i < dto.Valores.Count; i++)
                    {
                        resultado.TablaResultados.Add(new FilaCalculo
                        {
                            Indice = $"t={i}",
                            Precio = dto.Valores[i],
                            ValorCalculado = i >= 4 ? dto.Valores.Skip(i - 4).Take(5).Average().ToString("N2") : "n/a"
                        });
                    }
                    break;

                case "Regresión Lineal":
                    var (pendiente, interseccion) = ObtenerComponentesRegresion(dto.Valores);
                    resultado.ValorPredicho = (pendiente * dto.Valores.Count) + interseccion;
                    resultado.Tendencia = resultado.ValorPredicho > ultimoValor ? "alcista" : "bajista";

                   
                    resultado.Detalles.Add("Pendiente", pendiente.ToString("F4"));
                    resultado.Detalles.Add("Intersección", interseccion.ToString("N2"));
                    resultado.Detalles.Add("Puntos analizados", dto.Valores.Count.ToString());

                    // Generar tabla para Regresión (Valores de la recta)
                    for (int i = 0; i < dto.Valores.Count; i++)
                    {
                        double valorRecta = (pendiente * i) + interseccion;
                        resultado.TablaResultados.Add(new FilaCalculo
                        {
                            Indice = $"t={i}",
                            Precio = dto.Valores[i],
                            ValorCalculado = valorRecta.ToString("N2")
                        });
                    }
                    break;

                case "Momentum (ROC)":
                    double roc = CalcularMomentum(dto.Valores);
                    resultado.ValorPredicho = ultimoValor * (1 + (roc / 100));
                    resultado.Tendencia = roc > 0 ? "alcista" : "bajista";

                    resultado.Detalles.Add("ROC (%)", roc.ToString("N2") + "%");
                    resultado.Detalles.Add("Variación Abs.", (resultado.ValorPredicho - ultimoValor).ToString("N2"));
                    resultado.Detalles.Add("Volatilidad", Math.Abs(roc) > 10 ? "Alta" : "Normal");

                    // Generar tabla de ROC paso a paso
                    for (int i = 0; i < dto.Valores.Count; i++)
                    {
                        string calculoROC = "n/a";
                        if (i >= 5) // ROC de 5 periodos como en la referencia
                        {
                            double inicial = dto.Valores[i - 5];
                            double r = inicial == 0 ? 0 : ((dto.Valores[i] - inicial) / inicial) * 100;
                            calculoROC = r.ToString("N2") + "%";
                        }

                        resultado.TablaResultados.Add(new FilaCalculo
                        {
                            Indice = $"t={i}",
                            Precio = dto.Valores[i],
                            ValorCalculado = calculoROC
                        });
                    }
                    break;

                default:
                    resultado.ValorPredicho = 0;
                    resultado.Tendencia = "neutral";
                    break;
            }

            return resultado;
        }

        private (double pendiente, double interseccion) ObtenerComponentesRegresion(List<double> valores)
        {
            int n = valores.Count;
            double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
            for (int i = 0; i < n; i++)
            {
                sumX += i;
                sumY += valores[i];
                sumXY += i * valores[i];
                sumX2 += i * i;
            }
            double pendiente = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double interseccion = (sumY - pendiente * sumX) / n;
            return (pendiente, interseccion);
        }

        private double CalcularMomentum(List<double> valores)
        {
            double actual = valores.Last();
            double inicial = valores.First();
            return inicial == 0 ? 0 : ((actual - inicial) / inicial) * 100;
        }
    }
}