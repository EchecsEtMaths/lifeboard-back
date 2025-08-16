namespace Lifeboard.Models
{
    public class AddTransactionDto
    {
        public string Nom { get; set; }
        public DateTime Date { get; set; }
        public string Montant { get; set; }
        public string Categorie { get; set; }
    }
}
