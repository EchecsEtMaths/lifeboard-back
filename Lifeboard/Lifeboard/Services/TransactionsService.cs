using Lifeboard.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace Lifeboard.Services
{
    public class TransactionsService
    {
        private readonly string _connectionString;
        public TransactionsService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("FinancesDb")!;
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            
            var cmd = new MySqlCommand("SELECT t.id, t.nom, t.date_transac, t.montant, c.nom as categorie " +
                "FROM transactions t " +
                "INNER JOIN categorie c ON t.categorie_id = c.id " +
                "WHERE compte_Id = 1 " +
                "ORDER BY date_transac DESC", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            var results = new List<Transaction>();
            while (await reader.ReadAsync())
            {
                var transaction = new Transaction
                {
                    Id = reader.GetInt32("id"),
                    Nom = reader.GetString("nom"),
                    DateTransac = reader.GetDateTime("date_transac"),
                    Montant = reader.GetDecimal("montant"),
                    CategorieNom = reader.GetString("categorie")
                };
                results.Add(transaction);
            }
            return results;
        }
    }
}
