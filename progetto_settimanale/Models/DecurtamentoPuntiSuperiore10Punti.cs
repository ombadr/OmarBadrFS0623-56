﻿namespace progetto_settimanale.Models
{
    public class DecurtamentoPuntiSuperiore10Punti
    {
        public int idanagrafica { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public DateTime DataViolazione { get; set; }
        public decimal Importo { get; set; }
        public int DecurtamentoPunti { get; set; }
    }
}
