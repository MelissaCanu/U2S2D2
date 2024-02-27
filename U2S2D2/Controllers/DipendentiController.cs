using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using U2S2D2.Models;

namespace U2S2D2.Controllers
{
    public class DipendentiController : Controller
    {   
        //CONTROLLER DIPENDENTI
        

        //apro connessione nell'action result Index
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            //creo una lista di tipo Dipendente chiamata dipendenti
            List<Dipendente> dipendenti = new List<Dipendente>();

            try
            {
                connection.Open();
                string query = "SELECT * FROM Dipendenti";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                // creo ciclo while per leggere i dati dal database e inserirli in una lista di oggetti Dipendente
                while (reader.Read())
                {
                    //creo un oggetto Dipendente chiamato dipendente
                    Dipendente dipendente = new Dipendente
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString(),
                        Indirizzo = reader["Indirizzo"].ToString(),
                        CodiceFiscale = reader["CodiceFiscale"].ToString(),
                        Coniugato = Convert.ToBoolean(reader["Coniugato"]),
                        NumeroFigli = Convert.ToInt32(reader["NumeroFigli"]),
                        Mansione = reader["Mansione"].ToString()
                    };
                    //aggiungo l'oggetto Dipendente alla lista
                    dipendenti.Add(dipendente);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return View(dipendenti);
        }

        
        public ActionResult CreaDipendenti()
        {
            return View();
        }

        //uso httpost per inviare i dati al database
        [HttpPost]

        //action result per la creazione di un nuovo dipendente con connessione al database e query di inserimento dei dati nel database 
        public ActionResult CreaDipendenti(Dipendente dipendente)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                //query per inserire i dati nel database tramite i parametri passati dal form di creazione di un nuovo dipendente
                string query = "INSERT INTO Dipendenti (Nome, Cognome, Indirizzo, CodiceFiscale, Coniugato, NumeroFigli, Mansione) VALUES (@Nome, @Cognome, @Indirizzo, @CodiceFiscale, @Coniugato, @NumeroFigli, @Mansione)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nome", dipendente.Nome);
                cmd.Parameters.AddWithValue("@Cognome", dipendente.Cognome);
                cmd.Parameters.AddWithValue("@Indirizzo", dipendente.Indirizzo);
                cmd.Parameters.AddWithValue("@CodiceFiscale", dipendente.CodiceFiscale);
                cmd.Parameters.AddWithValue("@Coniugato", dipendente.Coniugato);
                cmd.Parameters.AddWithValue("@NumeroFigli", dipendente.NumeroFigli);
                cmd.Parameters.AddWithValue("@Mansione", dipendente.Mansione);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return RedirectToAction("Index");
        }

  

    }
    }

