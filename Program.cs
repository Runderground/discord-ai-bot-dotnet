using System;
using System.Threading.Tasks;
using System.Text.Json;
using Discord;
using Discord.WebSocket;

public class Message
{
  public string role { get; set; }
  public string content { get; set; }
}

public class MessageModel
{
  public string model { get; set; }
  public List<Message> messages { get; set; }

  public MessageModel()
  {
    messages = new List<Message>();
  }
}

public class MessageResponse
{
  
}

public class Program
{
  private DiscordSocketClient _client;
  public static async Task Main(string[] args) => await new Program().StartAsync();

  public async Task StartAsync()
  {
    _client = new DiscordSocketClient();
    _client.Log += LogAsync;
    _client.Ready += ReadyAsync;
    _client.SlashCommandExecuted += SlashCommandHandler;

    string token = System.Environment.GetEnvironmentVariable("BOT_TOKEN");

    // IA Context Setting Up

    try
    {
      DeepseekClient Ia = new DeepseekClient();
      Console.WriteLine("IA Context Setted Up");
    }
    catch (Exception e)
    {
      throw e;
    }

    await _client.LoginAsync(TokenType.Bot, token);
    await _client.StartAsync();

    await Task.Delay(-1);
  }

  private Task LogAsync(LogMessage log)
  {
    Console.WriteLine(log);
    return Task.CompletedTask;
  }

  private async Task ReadyAsync()
  {
    
  // Criando SlashCommands para o grupo
    var guild = _client.GetGuild(1047262941562548254);

    var guildCommand = new SlashCommandBuilder();

    guildCommand.WithName("guild-info");
    guildCommand.WithDescription($"Mostra informações do servidor {guild.Name}");

    var globalCommand = new SlashCommandBuilder();
    globalCommand.WithName("status");
    globalCommand.WithDescription("Verifica se o Byte está online.");

    globalCommand.WithName("ia");
    globalCommand.WithDescription("Faz uma pergunta para o Byte.");
    globalCommand.AddOption("pergunta", ApplicationCommandOptionType.String, "Pergunta para o Byte.", isRequired: true);

    try
    {
      // Builder para registrar os comandos no Grupo
      await guild.CreateApplicationCommandAsync(guildCommand.Build());
      // Builder para registrar os comandos no Global
      await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
    } catch (Exception exception)
    {
        var json = JsonSerializer.Serialize(exception);
        Console.WriteLine(json);
    }
  }
  
  // Captura o Evento quando um usuário utilizada algum Slash Command
  private async Task SlashCommandHandler(SocketSlashCommand command)
  {
    switch(command.Data.Name)
    {
    case "status":
      await Command.Status(command);
      break;

    case "ia":
        await Command.ChatIA(command);
      break;
    }
  }

  // Status Command
  
}