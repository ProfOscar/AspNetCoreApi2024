using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TestApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController(IWebHostEnvironment webHostEnvironment) : ControllerBase
    {
        private SqlConnection GetConnection()
        {
            string dbPath = Path.Combine(webHostEnvironment.ContentRootPath, "App_Data\\RecipesDB.mdf");
            string connStr = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30";
            return new SqlConnection(connStr);
        }

        // GET: api/recipes
        [HttpGet]
        public IEnumerable<RecipeItem> Get()
        {
            List<RecipeItem> recipes = new();

            try
            {
                using var connection = GetConnection();
                connection.Open();

                var sql = "SELECT * FROM Recipes";
                using var command = new SqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    RecipeItem recipe = new RecipeItem();
                    recipe.Id = reader.GetInt32(0);
                    recipe.Name = reader.GetString(1);
                    recipe.Description = reader.GetString(2);
                    recipe.ImagePath = reader.GetString(3);

                    recipes.Add(recipe);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return recipes;
        }

        // GET api/<RecipesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RecipesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RecipesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RecipesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
