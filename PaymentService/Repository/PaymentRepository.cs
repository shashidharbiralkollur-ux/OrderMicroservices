using Microsoft.Data.SqlClient;
using PaymentService.Models;

namespace PaymentService.Repository
{
    public class PaymentRepository
    {
        private readonly string _cs;

        public PaymentRepository(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection");
        }

        public void SavePayment(PaymentRequest req)
        {
            using SqlConnection con = new(_cs);
            con.Open();

            var q = @"
            INSERT INTO Payments (OrderId, Amount, PaymentStatus)
            VALUES (@OrderId, @Amount, 'SUCCESS')";

            using SqlCommand cmd = new(q, con);
            cmd.Parameters.AddWithValue("@OrderId", req.Id);
            cmd.Parameters.AddWithValue("@Amount", req.Amount);

            cmd.ExecuteNonQuery();
        }
    }
}