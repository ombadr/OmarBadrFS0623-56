using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using progetto_settimanale.Models;
namespace progetto_settimanale.Controllers
{
    public class ReportController : Controller
    {
        private readonly SqlConnection _connection;

        public ReportController(SqlConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TotaleVerbaliPerTrasgressore()
        {
            var listaTotale = new List<TotaleVerbaliPerTrasgressore>();
            try
            {
                _connection.Open();
                string query = @"
                   SELECT a.idanagrafica, a.Cognome, a.Nome, COUNT(v.idverbale) AS TotaleVerbali
                   FROM ANAGRAFICA a
                   JOIN VERBALE v ON a.idanagrafica = v.idanagrafica
                    GROUP BY a.idanagrafica, a.Cognome, a.Nome
                        ";
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaTotale.Add(new TotaleVerbaliPerTrasgressore
                            {
                                idanagrafica = reader.GetInt32(0),
                                Cognome = reader.GetString(1),
                                Nome = reader.GetString(2),
                                TotaleVerbali = reader.GetInt32(3)
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return View(listaTotale);
        }

        public IActionResult PuntiDecurtatiPerTrasgressore()
        {
            var listaTotale = new List<PuntiDecurtatiPerTrasgressore>();

            try
            {
                _connection.Open();
                string query = @"
                SELECT a.idanagrafica,a.Cognome, a.Nome, SUM(v.DecurtamentoPunti) as TotalePuntiDecurtati
                FROM Anagrafica a
                JOIN VERBALE v ON a.idanagrafica = v.idanagrafica
                GROUP BY a.idanagrafica, a.Cognome, a.Nome
                        ";
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaTotale.Add(new PuntiDecurtatiPerTrasgressore
                            {
                                idanagrafica = reader.GetInt32(0),
                                Cognome = reader.GetString(1),
                                Nome = reader.GetString(2),
                                TotalePuntiDecurtati = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                            });
                        }
                    }
                }

            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            } finally { _connection.Close(); }
            
            return View(listaTotale);
        }

        public IActionResult DecurtamentoPuntiSuperiore10Punti()
        {
            var listaTotale = new List<DecurtamentoPuntiSuperiore10Punti>();
            try
            {
                _connection.Open();
                string query = @"
                    SELECT a.idanagrafica, a.Cognome, a.Nome, v.DataViolazione, v.Importo, v.DecurtamentoPunti
                    FROM ANAGRAFICA a
                    JOIN VERBALE v ON a.idanagrafica = v.idanagrafica
                    WHERE v.DecurtamentoPunti > 10
                        ";
                
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            listaTotale.Add(new DecurtamentoPuntiSuperiore10Punti
                            {
                                idanagrafica = reader.GetInt32(0),
                                Cognome = reader.GetString(1),
                                Nome = reader.GetString(2),
                                DataViolazione = reader.GetDateTime(3),
                                Importo = reader.GetDecimal(4),
                                DecurtamentoPunti = reader.GetInt32(5)
                            });
                        }
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            } finally
            {
                _connection.Close();
            }
            return View(listaTotale);
        }

        public IActionResult ViolazioniImportoMaggioreDi400()
        {
            var listaTotale = new List<ViolazioniImportoMaggioreDi400>();
            try
            {
                _connection.Open();
                string query = @"
                     SELECT v.idverbale, v.DataViolazione, v.Importo, v.DecurtamentoPunti, a.Cognome, a.Nome, tv.descrizione AS TipoViolazione
                     FROM VERBALE v
                     JOIN ANAGRAFICA a ON v.idanagrafica = a.idanagrafica
                     JOIN TIPO_VIOLAZIONE tv ON v.idviolazione = tv.idviolazione
                     WHERE v.Importo > 400
                    ";
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaTotale.Add(new ViolazioniImportoMaggioreDi400
                            {
                                idverbale = reader.GetInt32(0),
                                DataViolazione = reader.GetDateTime(1),
                                Importo = reader.GetDecimal(2),
                                DecurtamentoPunti = reader.GetInt32(3),
                                Cognome = reader.GetString(4),
                                Nome = reader.GetString(5),
                                TipoViolazione = reader.GetString(6)
                            });
                        }
                    }
                }

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }
            return View(listaTotale);
        }
    }
}
