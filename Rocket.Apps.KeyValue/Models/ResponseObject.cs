namespace Rocket.Apps.KeyValue.Models
{
    public class ResponseObject<TObject>
    {
        public int Code { get; set; }
        public TObject Payload { get; set; }
    }
}