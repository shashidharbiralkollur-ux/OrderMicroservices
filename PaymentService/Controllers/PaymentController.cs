using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;
using PaymentService.Repository;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentRepository _repo;

        public PaymentController(PaymentRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("pay")]
        public IActionResult Pay(PaymentRequest req)
        {
            _repo.SavePayment(req);
            return Ok(new { message = "Payment successful" });
        }
    }
}