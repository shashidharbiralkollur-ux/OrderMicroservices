using Microsoft.Data.SqlClient;
using InventoryService.Models;

namespace InventoryService.Repository
{
    public class InventoryRepository
    {
        private readonly string _connectionString;

        public InventoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool ReserveStock(InventoryRequest request)
        {
            using SqlConnection con = new(_connectionString);

            string query = @"
                UPDATE Inventory
                SET AvailableQuantity = AvailableQuantity - @Qty
                WHERE ProductName = @ProductName
                  AND AvailableQuantity >= @Qty";

            SqlCommand cmd = new(query, con);
            cmd.Parameters.AddWithValue("@Qty", request.Quantity);
            cmd.Parameters.AddWithValue("@ProductName", request.ProductName);

            con.Open();
            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
