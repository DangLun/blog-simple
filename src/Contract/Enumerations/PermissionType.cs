using System.Text.Json.Serialization;

namespace Contract.Enumerations
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PermissionType
    {
        ADMIN,
        USER
    }
}
