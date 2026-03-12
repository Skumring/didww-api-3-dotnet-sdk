using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Didww.Api3.Resource;

public abstract class BaseResource
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    public abstract string Type { get; }

    private readonly HashSet<string> _dirtyFields = new();

    private bool _dirtyTrackingEnabled;

    public static T Build<T>(string id) where T : BaseResource, new()
    {
        var instance = new T();
        instance.Id = id;
        instance.EnableDirtyTracking();
        return instance;
    }

    protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        field = value;
        MarkDirty(propertyName ?? string.Empty);
    }

    protected T MarkDirty<T>(string fieldName, T value)
    {
        MarkDirty(fieldName);
        return value;
    }

    protected void MarkDirty(string fieldName)
    {
        if (!_dirtyTrackingEnabled || string.IsNullOrEmpty(fieldName))
            return;
        _dirtyFields.Add(ToSnakeCase(fieldName));
    }

    public bool IsFieldDirty(string fieldName)
    {
        if (string.IsNullOrEmpty(fieldName))
            return false;
        return _dirtyFields.Contains(ToSnakeCase(fieldName));
    }

    [JsonIgnore]
    public bool HasDirtyFields => _dirtyFields.Count > 0;

    public void EnableDirtyTracking()
    {
        _dirtyTrackingEnabled = true;
    }

    internal bool IsDirtyTrackingEnabled => _dirtyTrackingEnabled;

    internal HashSet<string> GetDirtyFields() => _dirtyFields;

    private static string ToSnakeCase(string fieldName)
    {
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < fieldName.Length; i++)
        {
            char c = fieldName[i];
            if (char.IsUpper(c))
            {
                if (i > 0 && fieldName[i - 1] != '_')
                    sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}
