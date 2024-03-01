using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using progetto_settimanale.Models;

namespace progetto_settimanale.Controllers
{
    public class ViolazioneController : Controller
    {
        private readonly SqlConnection _connection;

        public ViolazioneController(SqlConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Index()
        {
            var violazioni = new List<Violazione>();
            _connection.Open();
            string query = "SELECT * FROM TIPO_VIOLAZIONE";
            using(SqlCommand cmd = new SqlCommand(query, _connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        violazioni.Add(new Violazione()
                        {
                            idviolazione = reader.GetInt32(0),
                            descrizione = reader.GetString(1),
                        });
                    }
                }
            }
            _connection.Close();
                return View(violazioni);
        }

        public IActionResult Aggiungi()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Aggiungi(Violazione violazione)
        {
            if (ModelState.IsValid)
            {
                _connection.Open();
                var query = "INSERT INTO TIPO_VIOLAZIONE (descrizione) VALUES (@descrizione)";
                using (var cmd = new SqlCommand(query, _connection)) {
                    cmd.Parameters.AddWithValue("@descrizione", violazione.descrizione);
                    cmd.ExecuteNonQuery();
                }
                _connection.Close();

                return RedirectToAction("Index");
            }

            return View(violazione);
        }
    }
}
