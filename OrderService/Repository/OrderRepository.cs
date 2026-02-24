using Microsoft.Data.SqlClient;
using OrderService.Models;

namespace OrderService.Repository
{
    public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int CreateOrder(Order order)
        {
            using SqlConnection con = new(_connectionString);
            con.Open();

            string query = @"
                INSERT INTO Orders (ProductName, Quantity, Amount, Status)
                OUTPUT INSERTED.Id
                VALUES (@ProductName, @Quantity, @Amount, @Status)";

            using SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@ProductName", order.ProductName);
            cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
            cmd.Parameters.AddWithValue("@Amount", order.Amount);
            cmd.Parameters.AddWithValue("@Status", order.Status);

            return (int)cmd.ExecuteScalar();
        }

        public Order? GetOrderById(int id)
        {
            using SqlConnection con = new(_connectionString);
            con.Open();

            string query = "SELECT * FROM Orders WHERE Id=@Id";
            using SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Order
            {
                Id = (int)r["Id"],
                ProductName = r["ProductName"].ToString()!,
                Quantity = (int)r["Quantity"],
                Amount = (decimal)r["Amount"],
                Status = r["Status"].ToString()
            };
        }

        public void UpdateStatus(int id, string status)
        {
            using SqlConnection con = new(_connectionString);
            con.Open();

            string query = "UPDATE Orders SET Status=@Status WHERE Id=@Id";
            using SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}