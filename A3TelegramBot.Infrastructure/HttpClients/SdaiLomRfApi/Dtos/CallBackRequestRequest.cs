namespace A3TelegramBot.Infrastructure.HttpClients.SdaiLomRfApi.Dtos;

public class CallBackRequestRequest(int Id, string Source, string Phone, string Name, bool Consent)
{
    public int Id { get; set; } = Id;
    public string Source { get; set; } = Source;
    public string Phone { get; set; } = Phone;
    public string Name { get; set; } = Name;
    public bool Consent { get; set; } = Consent;
}

public class Wrapper(List<CallBackRequestRequest> callBackRequestRequests)
{
    public List<CallBackRequestRequest> Documents { get; set; } = callBackRequestRequests;
}