using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using progetto_settimanale.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace progetto_settimanale.Controllers
{
    public class VerbaleController : Controller
    {
        private readonly SqlConnection _connection;

        public VerbaleController(SqlConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Index()
        {
            var verbali = new List<Verbale>();
            _connection.Open();
            string query = "SELECT * FROM VERBALE";
            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        verbali.Add(new Verbale()
                        {
                            idverbale = reader.GetInt32(0),
                            DataViolazione = reader.GetDateTime(1),
                            IndirizzoViolazione = reader.GetString(2),
                            Nominativo_Agente = reader.GetString(3),
                            DataTrascrizioneVerbale = reader.GetDateTime(4),
                            Importo = reader.GetDecimal(5),
                            DecurtamentoPunti = reader.GetInt32(6),
                            idanagrafica = reader.GetInt32(7),
                            idviolazione = reader.GetInt32(8),

                        });
                    }
                }
            }
            _connection.Close();
            return View(verbali);
        }

        public IActionResult Aggiungi()
        {
            var anagrafiche = new List<Anagrafica>();
            var violazione = new List<Violazione>();
            _connection.Open();

            using (var cmd = new SqlCommand("SELECT idanagrafica, Cognome, Nome FROM ANAGRAFICA", _connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        anagrafiche.Add(new Anagrafica
                        {
                            idanagrafica = reader.GetInt32(0),
                            Cognome = reader.GetString(1),
                            Nome = reader.GetString(2),
                        });
                    }
                }
            }


            using (var cmd = new SqlCommand("SELECT idviolazione, descrizione FROM TIPO_VIOLAZIONE", _connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        violazione.Add(new Violazione
                        {
                            idviolazione = reader.GetInt32(0),
                            descrizione = reader.GetString(1),
                        });
                    }
                }
            }

            _connection.Close();

            var anagraficheSelectList = anagrafiche.Select(a => new SelectListItem
            {
                Value = a.idanagrafica.ToString(),
                Text = $"ID {a.idanagrafica} - {a.Cognome} {a.Nome}"
            });

            ViewBag.Anagrafiche = new SelectList(anagraficheSelectList, "Value", "Text");
            ViewBag.Violazioni = new SelectList(violazione, "idviolazione", "descrizione");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Aggiungi(Verbale verbale)
        {
            if (ModelState.IsValid)
            {
                _connection.Open();
                var query = "INSERT INTO VERBALE (DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, idanagrafica, idviolazione) VALUES (@DataViolazione, @IndirizzoViolazione, @Nominativo_Agente, @DataTrascrizioneVerbale,@Importo, @DecurtamentoPunti, @idanagrafica, @idviolazione)";

                using (var cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                    cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione);
                    cmd.Parameters.AddWithValue("@Nominativo_Agente", verbale.Nominativo_Agente);
                    cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale);
                    cmd.Parameters.AddWithValue("@Importo", verbale.Importo);
                    cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti);
                    cmd.Parameters.AddWithValue("@idanagrafica", verbale.idanagrafica);
                    cmd.Parameters.AddWithValue("@idviolazione", verbale.idviolazione);

                    cmd.ExecuteNonQuery();
                }

                _connection.Close();
                return RedirectToAction("Index");

            }
            return View(verbale);
        }
    }
}
