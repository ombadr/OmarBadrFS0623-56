using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using progetto_settimanale.Models;


namespace progetto_settimanale.Controllers
{
    public class AnagraficaController : Controller
    {
        private readonly SqlConnection _connection;

        public AnagraficaController(SqlConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Index()
        {
            var anagrafiche = new List<Anagrafica>();
            _connection.Open();
            string query = "SELECT * FROM ANAGRAFICA";
            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        anagrafiche.Add(new Anagrafica()
                        {
                            idanagrafica = reader.GetInt32(0),
                            Cognome = reader.GetString(1),
                            Nome = reader.GetString(2),
                            Indirizzo = reader.GetString(3),
                            Città = reader.GetString(4),
                            CAP = reader.GetString(5),
                            Cod_Fisc = reader.GetString(6),
                        });
                    }
                }
            }
            _connection.Close();
            return View(anagrafiche);
        }

        public IActionResult Aggiungi()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Aggiungi(Anagrafica anagrafica)
        {
            if (ModelState.IsValid)
            {
                _connection.Open();
                var query = "INSERT INTO ANAGRAFICA (Cognome, Nome, Indirizzo, Città, CAP, Cod_Fisc) VALUES (@Cognome, @Nome, @Indirizzo, @Città, @CAP, @Cod_Fisc)";
                using (var cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@Cognome", anagrafica.Cognome);
                    cmd.Parameters.AddWithValue("@Nome", anagrafica.Nome);
                    cmd.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo);
                    cmd.Parameters.AddWithValue("@Città", anagrafica.Città);
                    cmd.Parameters.AddWithValue("@CAP", anagrafica.CAP);
                    cmd.Parameters.AddWithValue("@Cod_Fisc", anagrafica.Cod_Fisc);
                    cmd.ExecuteNonQuery();
                }
                _connection.Close();

                return RedirectToAction("Index");
            }
            return View(anagrafica);
        }
    }
}
