using System.Net;

namespace Didww.Api3.Http;

public class QueryParams
{
    private readonly Dictionary<string, string> _filters = new();
    private readonly List<string> _includes = new();
    private readonly List<string> _sorts = new();
    private int? _pageNumber;
    private int? _pageSize;

    public QueryParams Filter(string key, string value)
    {
        _filters[key] = value;
        return this;
    }

    public QueryParams Include(params string[] relationships)
    {
        _includes.AddRange(relationships);
        return this;
    }

    public QueryParams Sort(params string[] fields)
    {
        _sorts.AddRange(fields);
        return this;
    }

    public QueryParams Page(int number, int size)
    {
        _pageNumber = number;
        _pageSize = size;
        return this;
    }

    public string ToQueryString()
    {
        var parts = new List<string>();

        if (_includes.Count > 0)
            parts.Add("include=" + WebUtility.UrlEncode(string.Join(",", _includes)));

        foreach (var kvp in _filters)
            parts.Add("filter[" + WebUtility.UrlEncode(kvp.Key) + "]=" + WebUtility.UrlEncode(kvp.Value));

        if (_sorts.Count > 0)
            parts.Add("sort=" + WebUtility.UrlEncode(string.Join(",", _sorts)));

        if (_pageNumber.HasValue)
            parts.Add("page[number]=" + _pageNumber.Value);

        if (_pageSize.HasValue)
            parts.Add("page[size]=" + _pageSize.Value);

        return parts.Count == 0 ? "" : "?" + string.Join("&", parts);
    }
}
