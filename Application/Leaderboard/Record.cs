using System.ComponentModel;
using Newtonsoft.Json;

namespace Application.Leaderboard;

public class Record
{
    public Record(string username, decimal distance, decimal maxAcceleration, DateTime created)
    {
        Username = username;
        Distance = distance;
        MaxAcceleration = maxAcceleration;
        Created = created;
    }

    [JsonProperty("athlete")]
    [DisplayName("User")]
    public string Username { get; }

    [JsonProperty("distance")]
    [DisplayName("Throw Distance")]
    public decimal Distance { get; }

    [JsonProperty("max_acceleration")]
    [DisplayName("Max Acceleration")]
    public decimal MaxAcceleration { get; }

    [JsonProperty("created")]
    [DisplayName("Date")]
    public DateTime Created { get; }
}