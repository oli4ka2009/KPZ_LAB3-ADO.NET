using AutoMapper;
using LAB3_ADO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace LAB3_ADO.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly string _connectionString = "Server=DESKTOP-95GPI7I;Database=KPZ3_ADO_database;Trusted_Connection=True;Encrypt=false;";

        public HomeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        private async Task<Client> GetClient(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            string sqlExpression = $"SELECT * FROM Clients WHERE id = '{id}'";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            var client = _mapper.Map<Client>(reader);
            return client;
        }

        public async Task<IActionResult> Index()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            string sqlExpression = "SELECT * FROM Clients";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            var clients = new List<Client>();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    clients.Add(_mapper.Map<Client>(reader));
                }
            }

            return View(clients);

        }

        public IActionResult Create()
        {
            return View(new Client());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                string sqlExpression = $"INSERT INTO Clients (FirstName, LastName, DayOfBirth, PhoneNumber, Email, Status) VALUES ('{client.FirstName}', '{client.LastName}', '{client.DayOfBirth}', '{client.PhoneNumber}', '{client.Email}', '{client.Status}');";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = await command.ExecuteNonQueryAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Update(int Id)
        {
            Client client = await GetClient(Id);
            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Client client)
        {
            if (ModelState.IsValid)
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                string sqlExpression = $"UPDATE Clients SET FirstName='{client.FirstName}'," +
                    $" LastName='{client.LastName}', DayOfBirth='{client.DayOfBirth}', " +
                    $"PhoneNumber='{client.PhoneNumber}', Email='{client.Email}', Status='{client.Status}' WHERE id='{client.Id}'";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = await command.ExecuteNonQueryAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);

        }

        public async Task<IActionResult> Delete(Client client)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            string sqlExpression = $"DELETE FROM Clients WHERE id='{client.Id}'";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = await command.ExecuteNonQueryAsync();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
