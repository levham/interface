using a16.Models;
using System.Drawing;
using System.Text.Json;

namespace a16.Controllers
{
    public class FormController
    {
     public FormData LoadFormSettings()
    {
        string jsonPath = "button.json";
        if (!File.Exists(jsonPath)) return null;

        string jsonText = File.ReadAllText(jsonPath);
        return JsonSerializer.Deserialize<FormData>(jsonText);
    }

    public RootObject LoadButtons()
    {
        string jsonPath = "button.json";
        if (!File.Exists(jsonPath)) return null;

        string jsonText = File.ReadAllText(jsonPath);
        return JsonSerializer.Deserialize<RootObject>(jsonText);
    }

    public List<ButtonData> GetButtons()
    {
        var data = LoadButtons();
        return data?.buttons?.SelectMany(b => b).ToList() ?? new List<ButtonData>();
    }

    public Dictionary<int, string> GetEventMappings()
    {
        var data = LoadButtons();
        return data?.events?.ToDictionary(e => e.id, e => e.eventName ?? "Bilinmeyen Olay") ?? new Dictionary<int, string>();
    }
    }
}
