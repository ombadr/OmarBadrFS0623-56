namespace progetto_settimanale.Models
{
    public class PuntiDecurtatiPerTrasgressore
    {
        public int idanagrafica { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public int TotalePuntiDecurtati { get; set; }
    }
}
