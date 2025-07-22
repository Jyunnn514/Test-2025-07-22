using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;


namespace Test_2025_07_22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ValuesController(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection GetConnection() =>
            new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        // GET: api/<ValuesController>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] JsonElement json)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_Create_ACPD", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Json", JsonSerializer.Serialize(json));
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return Ok("Created");
        }

        // GET api/<ValuesController>/5
        [HttpGet("read")]
        public async Task<IActionResult> Read()
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_Read_ACPD_JSON", conn);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Ok(JsonDocument.Parse(result.ToString()).RootElement);
        }

        // POST api/<ValuesController>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] JsonElement json)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_Update_ACPD_JSON", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Json", JsonSerializer.Serialize(json));
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return Ok("Updated");
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] JsonElement json)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_Delete_ACPD_JSON", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Json", JsonSerializer.Serialize(json));
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return Ok("Deleted");
        }
    }
}
