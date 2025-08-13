namespace a16.Models
{
    public class RootObject
    {
        public List<List<ButtonData>>? buttons { get; set; }
        public List<EventData>? events { get; set; }
    }

    public class ButtonData
    {
        public int id { get; set; }
        public string text { get; set; } = ""; // Varsayılan değer atandı
    }

    public class EventData
    {
        public int id { get; set; }
        public string? eventName { get; set; }
    }
}
