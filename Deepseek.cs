using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

public class DeepseekClient
{
  private readonly HttpClient _httpClient;
  private const string BaseUrl = "https://openrouter.ai/api/v1/chat/completions";
  private string ApiKey = System.Environment.GetEnvironmentVariable("OPENROUNTER_KEY");
  

  public DeepseekClient()
  {
    _httpClient = new HttpClient();

    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");

  }

  public async Task<string> GetDataAsync(string data)
  {
    using JsonDocument doc = JsonDocument.Parse(data);
    JsonElement root = doc.RootElement;

    return root.GetProperty("choices")[0].GetProperty("message").GetProperty("content").ToString();
  }

  public async Task<string> PostDataAsync(string query)
  {
    var data = new MessageModel();
    data.model = "deepseek/deepseek-r1:free";
    data.messages.Add(new Message { role = "user", content = $"{query}" });
    var jsonContent = JsonSerializer.Serialize(data);

    var stringContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
    
    var response = await _httpClient.PostAsync($"{BaseUrl}", stringContent);

    var responseString = await response.Content.ReadAsStringAsync();

    return responseString;
  }
}