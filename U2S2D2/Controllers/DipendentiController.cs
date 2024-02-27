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

        //get action result per la modifica di un dipendente con form precompilato con i dati del dipendente selezionato
        public ActionResult EditDipendenti(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            Dipendente dipendente = new Dipendente();

            try
            {
                connection.Open();
                string query = "SELECT * FROM Dipendenti WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dipendente.ID = Convert.ToInt32(reader["ID"]);
                    dipendente.Nome = reader["Nome"].ToString();
                    dipendente.Cognome = reader["Cognome"].ToString();
                    dipendente.Indirizzo = reader["Indirizzo"].ToString();
                    dipendente.CodiceFiscale = reader["CodiceFiscale"].ToString();
                    dipendente.Coniugato = Convert.ToBoolean(reader["Coniugato"]);
                    dipendente.NumeroFigli = Convert.ToInt32(reader["NumeroFigli"]);
                    dipendente.Mansione = reader["Mansione"].ToString();

                    
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

            return View(dipendente);
        }

        //HttpPost action result per la modifica di un dipendente con connessione al database e query di update dei dati nel database
        [HttpPost]
        public ActionResult EditDipendenti(Dipendente dipendente)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                string query = "UPDATE Dipendenti SET Nome = @Nome, Cognome = @Cognome, Indirizzo = @Indirizzo, CodiceFiscale = @CodiceFiscale, Coniugato = @Coniugato, NumeroFigli = @NumeroFigli, Mansione = @Mansione WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", dipendente.ID);
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
        
        //get action result vuoto per la cancellazione di un dipendente 
        public ActionResult DeleteDipendenti(int id)
        {
          return View();
        }

        //post actionresult per la cancellazione di un dipendente -> elimina effettivamente il dipendente dal db
        [HttpPost]
        public ActionResult DeleteDipendenti(Dipendente dipendente)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                string query = "DELETE FROM Dipendenti WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", dipendente.ID);
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

