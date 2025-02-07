using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TestApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCoreApi.Controllers
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
            List<RecipeItem> recipes = [];

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

        // GET api/recipes/2
        [HttpGet("{id}")]
        public ActionResult<RecipeItem> Get(int id)
        {
            using var connection = GetConnection();
            connection.Open();

            var sql = "SELECT * FROM Recipes WHERE Id=@0";
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@0", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                RecipeItem recipe = new RecipeItem();
                recipe.Id = reader.GetInt32(0);
                recipe.Name = reader.GetString(1);
                recipe.Description = reader.GetString(2);
                recipe.ImagePath = reader.GetString(3);
                return recipe;
            }

            return NotFound();
        }

        // POST api/<RecipesController>
        [HttpPost]
        public ActionResult<RecipeItem> Post([FromBody] RecipeItem recipe)
        {
            using var connection = GetConnection();
            connection.Open();

            var sql = "INSERT INTO Recipes(Name,Description,ImagePath) VALUES(@0,@1,@2)";
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@0", recipe.Name);
            command.Parameters.AddWithValue("@1", recipe.Description);
            command.Parameters.AddWithValue("@2", recipe.ImagePath);

            int nRows = command.ExecuteNonQuery();
            if (nRows <= 0)
                return NotFound();

            return Ok(nRows);
        }

        // PUT api/<RecipesController>/5
        [HttpPut("{id}")]
        public ActionResult<RecipeItem> Put(int id, [FromBody] RecipeItem recipe)
        {
            using var connection = GetConnection();
            connection.Open();

            var sql = "UPDATE Recipes SET Name=@1,Description=@2,ImagePath=@3 WHERE id=@0";
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@0", id);
            command.Parameters.AddWithValue("@1", recipe.Name);
            command.Parameters.AddWithValue("@2", recipe.Description);
            command.Parameters.AddWithValue("@3", recipe.ImagePath);

            int nRows = command.ExecuteNonQuery();
            if (nRows <= 0)
                return NotFound();

            return Ok(nRows);
        }

        // DELETE api/recipes/2
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var connection = GetConnection();
            connection.Open();

            var sql = "DELETE Recipes WHERE Id=@0";
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@0", id);

            int nRows = command.ExecuteNonQuery();
            if (nRows <= 0) 
                return NotFound();

            return Ok(nRows);
        }
    }
}
