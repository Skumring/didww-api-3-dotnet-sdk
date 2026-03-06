using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public class Balance : BaseResource
{
    public override string Type => "balances";

    [JsonProperty("total_balance")]
    public double? TotalBalance { get; set; }

    [JsonProperty("balance")]
    public double? BalanceAmount { get; set; }

    [JsonProperty("credit")]
    public double? Credit { get; set; }
}
