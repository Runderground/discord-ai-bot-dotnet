using System;
using Discord;
using Discord.WebSocket;
using System.Text.Json;

public class Command
{
  public static async Task Status(SocketSlashCommand command)
  {
    var watch = System.Diagnostics.Stopwatch.StartNew();
      var embed = new EmbedBuilder()
        .WithTitle("<a:loading:1346666766499315723>")
        .WithColor(Color.Green);
    await command.RespondAsync(embed: embed.Build());
    watch.Stop();
    string time = watch.ElapsedMilliseconds.ToString();
    var result = new EmbedBuilder()
    .WithTitle($":white_check_mark: Byte está online! \n\n:timer: Tempo de resposta: ``{time}ms``")
    .WithColor(Color.Green);
    command.ModifyOriginalResponseAsync(x => x.Embed = result.Build());
  }

  public static async Task ChatIA(SocketSlashCommand command)
  {
    var query = command.Data.Options.First().Value;
    string result = "";
    DeepseekClient client = new DeepseekClient();
    try
    {
      command.RespondAsync("<a:loading:1346666766499315723>");
      Task.Run(async () => {
        var response = await client.PostDataAsync(query.ToString());
        result = await client.GetDataAsync(response);
        await command.ModifyOriginalResponseAsync(x => x.Content = result);
      });
    } 
    catch (Exception e)
    {
      Console.WriteLine("[ChatIa Error] " + e.Message);
    }
  }
}