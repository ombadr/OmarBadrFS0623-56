namespace progetto_settimanale.Models
{
    public class ViolazioniImportoMaggioreDi400
    {
        public int idverbale { get; set; }
        public DateTime DataViolazione { get; set; }
        public decimal Importo { get; set; }
        public int DecurtamentoPunti { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string TipoViolazione { get; set; }
    }
}
