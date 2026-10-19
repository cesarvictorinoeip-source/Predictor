using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Application.Services;
using Application.Dtos;
using Microsoft.AspNetCore.Http; 

namespace Predictor.Controllers
{
    public class PredictorController : Controller
    {
        private readonly ModoServices _modoServices;

        public PredictorController(ModoServices modoServices)
        {
            _modoServices = modoServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult CalcularPrediccion(List<double> valores)
        {
            if (valores == null || valores.Count == 0)
            {
                return RedirectToAction("Index");
            }

            
            string modoSession = HttpContext.Session.GetString("ModoElegido") ?? "Regresión Lineal";

          
            var dtoEntrada = new ModosDtos
            {
                Valores = valores,
                ModoSeleccionado = modoSession
            };

            
            
            ModosListDtos resultadoDetallado = _modoServices.CalcularDetallado(dtoEntrada);

         
            
            ViewBag.ResultadoDetallado = resultadoDetallado;
            ViewBag.ModoUsado = modoSession;

            return View("Index");
        }

        public IActionResult Modos()
        {
            List<PredictorView> listaDeModos = new()
            {
                new PredictorView { Modo = "Media Móvil Simple (SMA) Crossover", Descripcion = "Analiza el cruce de medias." },
                new PredictorView { Modo = "Regresión Lineal", Descripcion = "Predice valores basados en la pendiente." },
                new PredictorView { Modo = "Momentum (ROC)", Descripcion = "Mide la velocidad de cambio." }
            };

            return View(listaDeModos);
        }

        [HttpPost]
        public IActionResult GuardarModo(string modoSeleccionado)
        {
            if (!string.IsNullOrEmpty(modoSeleccionado))
            {
                
                HttpContext.Session.SetString("ModoElegido", modoSeleccionado);

                
                TempData["Mensaje"] = "Algoritmo configurado: " + modoSeleccionado;

                
                return RedirectToAction("Index");
            }

            
            return RedirectToAction("Modos");
        }
    }
}