using Lifeboard.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace Lifeboard.Services
{
    public class CategoriesService
    {
        private readonly string _connectionString;
        public CategoriesService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("FinancesDb")!;
        }

        public async Task<List<Categorie>> GetCategories()
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT c.id, c.nom " +
                "FROM categorie c " +
                "ORDER BY nom ASC", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            var results = new List<Categorie>();
            while (await reader.ReadAsync())
            {
                var categorie = new Categorie
                {
                    Id = reader.GetInt32("id"),
                    Nom = reader.GetString("nom")
                };
                results.Add(categorie);
            }
            return results;
        }
    }
}
