using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL_CASTRO.Data;
using PARCIAL_CASTRO.Models;
using PARCIAL_CASTRO.Services;
using System.Threading.Tasks;

namespace PARCIAL_CASTRO.Controllers
{
    [Route("[controller]")]
    public class TransaccionesController : Controller
    {
        private readonly ILogger<TransaccionesController> _logger;
        private readonly ApplicationDbContext _context; // Inyectamos el contexto de la base de datos
        private readonly ConversionService _conversionService; // Inyectamos el servicio de conversión

        public TransaccionesController(ILogger<TransaccionesController> logger, ApplicationDbContext context, ConversionService conversionService)
        {
            _logger = logger;
            _context = context; // Inyectamos el contexto
            _conversionService = conversionService; // Inyectamos el servicio de conversión
        }

        // Acción para mostrar el listado de transacciones
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var transacciones = await _context.DataTransacciones.ToListAsync();
            return View(transacciones); // Pasamos la lista de transacciones a la vista
        }

        // Acción para mostrar el formulario de registro
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // Acción para registrar una nueva transacción
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transacciones transacciones)
        {
            if (ModelState.IsValid)
            {
                // Si la transacción es en USD, calculamos el monto final usando la tasa de cambio
                if (transacciones.TipTrans == "USD")
                {
                     // Convertir de USD a BTC
                    try
                    {
                        transacciones.MontoFin = await _conversionService.ConvertUsdToBtc(transacciones.MontoEnv);
                        transacciones.TasaCam = await _conversionService.GetBtcToUsdRateAsync(); // Guarda la tasa actual
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores al obtener la tasa de conversión
                        ModelState.AddModelError("", "Error al obtener la tasa de conversión: " + ex.Message);
                        return View(transacciones);
                    }
                }
                // Si la transacción es en BTC, utilizamos ConversionService para obtener la tasa de conversión
                else if (transacciones.TipTrans == "BTC")
                {
                    try
                    {
                        var tasaBtc = await _conversionService.GetBtcToUsdRateAsync();
                        
                        transacciones.TasaCam = 1 / tasaBtc; // Esto convierte la tasa de BTC a USD a USD a BTC
                        transacciones.MontoFin = transacciones.MontoEnv * tasaBtc; // Calculamos el monto final en BTC
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores al obtener la tasa de conversión
                        ModelState.AddModelError("", "Error al obtener la tasa de conversión BTC: " + ex.Message);
                        return View(transacciones);
                    }
                }

                // Guardar la transacción en la base de datos
                _context.Add(transacciones);
                await _context.SaveChangesAsync();

                // Redirigir al listado después de guardar
                return RedirectToAction(nameof(Index));
            }

            // En caso de que el modelo no sea válido, regresar la vista con el modelo
            return View(transacciones);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}