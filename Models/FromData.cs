namespace a16.Models
{
    public class FormData
    {
        public FormSettings form { get; set; } = new FormSettings();
    }

    public class FormSettings
    {
        public int width { get; set; }
        public int height { get; set; }
        public List<int> location { get; set; } = new List<int> { 0, 0 };
    }
}
