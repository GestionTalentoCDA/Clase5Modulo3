using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Clase5Modulo3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {

        //HttpClient
        //HttpClientFactory

        private IHttpClientFactory _httpClientFactory;
        public PostsController(IHttpClientFactory httpFactory)
        {
            _httpClientFactory = httpFactory;
        }

        [HttpGet] // api/Posts GET
        public  async Task< IActionResult> GetPosts() 
        {
            //HttpClient client = new HttpClient(); //Establecer una nueva conexion 

            var client = _httpClientFactory.CreateClient("servicio-posts");

            //client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

            var resultService =  await client.GetAsync("posts"); // Se manda la peticion HTTP 

            
            var content = await resultService.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<GetPostResponse>>(content);


            //fetch("url).then(res => res.json()).then(res=> res.Id)


            return Ok(result); 
        }
        [HttpPost]
        public async Task<IActionResult> AddPosts([FromBody] AddPostRequest request) 
        {
            //HttpClient client = new HttpClient();

            var client = _httpClientFactory.CreateClient("servicio-posts");

            //client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");


            var requestString = JsonSerializer.Serialize(request);
            var payload = new StringContent(requestString,System.Text.Encoding.UTF8, "application/json");


            var resultService = await client.PostAsync("posts", payload); // Se manda la peticion HTTP 
        

            var content = await resultService.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<AddPostResponse>(content);

            return Ok(response);
        }

        public class AddPostResponse 
        {
            [JsonPropertyName("userId")]
            public int IdUsuario { get; set; }

            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("title")]
            public string Titulo { get; set; }

            [JsonPropertyName("body")]
            public string Descripcion { get; set; }


        }
        public class AddPostRequest 
        {
            public int userId { get; set; }
            public string body { get; set; }
            public string title { get; set; }

        }
        public class GetPostResponse 
        {
            [JsonPropertyName("userId")]
            public int IdUsuario { get; set; }

            [JsonPropertyName("title")]
            public string Titulo { get; set; }

            [JsonPropertyName("body")]
            public string Descripcion { get; set; }

        }
    }


}
