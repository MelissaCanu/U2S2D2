using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using U2S2D2.Models;

namespace U2S2D2.Controllers
{
    //CONTROLLER PAGAMENTI
    public class PagamentiController : Controller
    {
        //apro connessione nell'action result index

        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            //creo una lista di tipo Pagamento chiamata pagamenti
            List<Pagamento> pagamenti = new List<Pagamento>();

            try
            {
                connection.Open();
                string query = "SELECT * FROM Pagamenti"; //modifico la query per visualizzare in ordine decrescente per lo storico pagamenti
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                //creo ciclo while per leggere i dati dal database e inserirli in una lista di oggetti Pagamento
                while (reader.Read())
                {
                    //creo un oggetto Dipendente chiamato dipendente
                    Pagamento pagamento = new Pagamento
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        IDDipendente = Convert.ToInt32(reader["IDDipendente"]),
                        PeriodoPagamento = Convert.ToDateTime(reader["PeriodoPagamento"]).ToString("dd/MM/yyyy"),
                        AmmontarePagamento = Convert.ToDecimal(reader["AmmontarePagamento"]),
                        TipoPagamento = reader["TipoPagamento"].ToString()

                    };
                    //aggiungo l'oggetto Dipendente alla lista
                    pagamenti.Add(pagamento);
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

            return View(pagamenti);
        }

        public ActionResult CreaPagamenti()
        {
            return View();
        }

        //uso httpost per inviare i dati al database
        [HttpPost]
        public ActionResult CreaPagamenti(Pagamento pagamento)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                //query per inserire i dati nel database tramite i parametri passati dal form di creazione di un nuovo pagamento 
                string query = "INSERT INTO Pagamenti (IDDipendente, PeriodoPagamento, AmmontarePagamento, TipoPagamento) VALUES (@IDDipendente, @PeriodoPagamento, @AmmontarePagamento, @TipoPagamento)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@IDDipendente", pagamento.IDDipendente);
                cmd.Parameters.AddWithValue("@PeriodoPagamento", pagamento.PeriodoPagamento);
                cmd.Parameters.AddWithValue("@AmmontarePagamento", pagamento.AmmontarePagamento);
                cmd.Parameters.AddWithValue("@TipoPagamento", pagamento.TipoPagamento);
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
            //ritorno alla view Index dopo aver inserito i dati nel database e visualizzo la lista aggiornata
            return RedirectToAction("Index");
        }

        //aggiungo action StoricoPagamenti per restituire la View relativa di StoricoPagamenti.cshtml
        public ActionResult StoricoPagamenti()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            List<Pagamento> pagamenti = new List<Pagamento>();

            try
            {
                connection.Open();
                string query = "SELECT * FROM Pagamenti ORDER BY PeriodoPagamento DESC";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Pagamento pagamento = new Pagamento
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        IDDipendente = Convert.ToInt32(reader["ID"]),
                        PeriodoPagamento = Convert.ToDateTime(reader["PeriodoPagamento"]).ToString("dd/MM/yyyy"),
                        AmmontarePagamento = Convert.ToDecimal(reader["AmmontarePagamento"]),
                        TipoPagamento = reader["TipoPagamento"].ToString()
                    };
                    pagamenti.Add(pagamento);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }

            return View(pagamenti);
        }

    }
}