namespace Didww.Api3.Internal;

/// <summary>
/// Shared helpers for masking sensitive credential values in default
/// <c>ToString</c> / debugger / log output. The wire format is never
/// affected — only Stringer-style methods route through these helpers.
/// </summary>
internal static class Redact
{
    /// <summary>
    /// Returns "[FILTERED]" for any non-null, non-empty input and "null"
    /// otherwise. The "null" passthrough avoids leaking "this field was set"
    /// information when the value is genuinely unset.
    /// </summary>
    public static string Mask(string? value) => string.IsNullOrEmpty(value) ? "null" : "[FILTERED]";
}
