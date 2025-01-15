using System.Collections.Generic;
using System.Text.Json;

namespace Warehousing.DataServices_v1
{
    public interface IClientService
    {
        List<JsonElement> ReadClientsFromJson();
        void WriteClientsToJson(List<JsonElement> clients);
    }
}
