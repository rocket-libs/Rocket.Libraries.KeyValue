namespace Rocket.Services.KeyValue.Models
{
    public class KeyValueContainer
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public long LifetimeSeconds { get; set; }
    }
}