using CDR_Worship.Models;
using CDR_Worship.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CDR_Worship.Controllers
{
    [Route("[controller]")]
    public class SmsController : Controller
    {
        private readonly ILogger<SmsController> _logger;
        private readonly ISmsService _smsService;

        public SmsController(ILogger<SmsController> logger, ISmsService smsService)
        {
            _logger = logger;
            _smsService = smsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("send")]
public IActionResult SendSms(Sms model)
{
    if (ModelState.IsValid)
    {
        if (!string.IsNullOrEmpty(model.Message))
        {
            try
            {
                // Usar el servicio para enviar el mensaje a todos los destinatarios
                _smsService.SendSms(model.Message);
                _logger.LogInformation("SMS sent to all configured recipients.");
                
                // Enviar mensaje de éxito a la vista usando TempData
                TempData["SuccessMessage"] = "El SMS fue enviado exitosamente a todos los destinatarios.";
                return RedirectToAction("Index"); // Redirige a la vista Index o actual
            }
            catch (Exception ex)
            {
                // Enviar mensaje de error a la vista usando TempData
                _logger.LogError($"Error al enviar SMS: {ex.Message}");
                TempData["ErrorMessage"] = "No se pudo enviar el SMS. Intenta nuevamente.";
                return RedirectToAction("Index"); // Redirige a la vista actual
            }
        }

        ModelState.AddModelError("Message", "El mensaje no puede estar vacío.");
    }

    return View("Index", model); // Si hay errores, regresa la vista con el modelo
}
    }
}
