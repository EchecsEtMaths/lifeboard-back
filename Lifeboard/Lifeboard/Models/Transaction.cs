namespace Lifeboard.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime DateTransac { get; set; }
        public decimal Montant { get; set; }
        public string CategorieNom { get; set; }
        public bool Commun { get; set; }
    }
}
