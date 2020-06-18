namespace Rocket.Services.KeyValue.Models
{
    public class ResponseObject<TObject>
    {
        public int Code { get; set; }
        public TObject Payload { get; set; }
    }
}