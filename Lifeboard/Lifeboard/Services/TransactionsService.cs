using Lifeboard.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;

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

        public async Task<TotalCourant> GetTotalCourant()
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT COALESCE(SUM(montant), 0) as total FROM finances.transactions WHERE compte_id = 1", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            var result = new TotalCourant();
            while (await reader.ReadAsync())
            {
                result = new TotalCourant
                {
                    Total = reader.GetDecimal("total")
                };
            }
            return result;
        }

        internal async Task AddTransaction(AddTransactionDto transaction)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmdMax = new MySqlCommand("SELECT COALESCE(MAX(id), 0)+1 FROM Transactions", conn);
            int newId = Convert.ToInt32(cmdMax.ExecuteScalar());

            var cmd = new MySqlCommand($"INSERT INTO Transactions (id, nom,date_transac,montant,categorie_id,compte_id) " +
                "VALUES (@newId, @nom, @dateTransac, @montant, (SELECT Id FROM Categorie WHERE Nom = @categorieId), 1)", conn);
            cmd.Parameters.AddWithValue("@newId", newId);
            cmd.Parameters.AddWithValue("@nom", transaction.Nom);
            cmd.Parameters.AddWithValue("@dateTransac", transaction.Date);
            cmd.Parameters.AddWithValue("@montant", decimal.Parse(transaction.Montant.Replace("€", "").Trim(), new CultureInfo("fr-FR")));
            cmd.Parameters.AddWithValue("@categorieId", transaction.Categorie);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Transaction>> GetTransactionsForUser(string user)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT t.id, t.nom, t.date_transac, t.montant, c.nom as categorie " +
                "FROM transactions t " +
                "INNER JOIN categorie c ON t.categorie_id = c.id " +
                "WHERE compte_Id = 1 AND t.user = @user " +
                "ORDER BY date_transac DESC", conn);
            cmd.Parameters.AddWithValue("@user", user);

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

        public async Task<TotalCourant> GetTotalCourantForUser(string user)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand("SELECT COALESCE(SUM(montant), 0) as total FROM finances.transactions WHERE compte_id = 1 AND user = @user", conn);
            cmd.Parameters.AddWithValue("@user", user);

            using var reader = await cmd.ExecuteReaderAsync();

            var result = new TotalCourant();
            while (await reader.ReadAsync())
            {
                result = new TotalCourant
                {
                    Total = reader.GetDecimal("total")
                };
            }
            return result;
        }

        internal async Task AddTransactionForUser(string user, AddTransactionDto transaction)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmdMax = new MySqlCommand("SELECT COALESCE(MAX(id), 0)+1 FROM Transactions", conn);
            int newId = Convert.ToInt32(cmdMax.ExecuteScalar());

            var cmd = new MySqlCommand($"INSERT INTO Transactions (id, nom,date_transac,montant,categorie_id,compte_id, user) " +
                "VALUES (@newId, @nom, @dateTransac, @montant, (SELECT Id FROM Categorie WHERE Nom = @categorieId), 1, @user)", conn);
            cmd.Parameters.AddWithValue("@newId", newId);
            cmd.Parameters.AddWithValue("@nom", transaction.Nom);
            cmd.Parameters.AddWithValue("@dateTransac", transaction.Date);
            cmd.Parameters.AddWithValue("@montant", decimal.Parse(transaction.Montant.Replace("€", "").Trim(), new CultureInfo("fr-FR")));
            cmd.Parameters.AddWithValue("@categorieId", transaction.Categorie);
            cmd.Parameters.AddWithValue("@user", user);

            await cmd.ExecuteNonQueryAsync();
        }

        internal async Task DeleteTransaction(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand($"DELETE FROM Transactions WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
