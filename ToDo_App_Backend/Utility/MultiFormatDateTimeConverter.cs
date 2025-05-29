using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDo_App.Utility
{
  public class MultiFormatDateTimeConverter: JsonConverter<DateTime>
  {
    private static readonly string[] _formats = new[]
    {
      // Germany
      "dd.MM.yyyy", "d.M.yyyy", "dd.MM.yy",
      "dd.MM.yyyy HH:mm", "dd.MM.yyyy HH:mm:ss",
      "dddd, d. MMMM yyyy", "dddd, d. MMMM yyyy HH:mm",

      // Europe
      "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy",
      "dd/MM/yyyy HH:mm", "dd/MM/yyyy HH:mm:ss",
      "dddd, d MMMM yyyy", "dddd, d MMMM yyyy HH:mm:ss",

      // US
      "MM/dd/yyyy", "M/d/yyyy", "MM-dd-yyyy",
      "MM/dd/yyyy h:mm tt", "MM/dd/yyyy h:mm:ss tt",
      "dddd, MMMM d, yyyy h:mm tt", "MM/dd/yyyy HH:mm",

      // Singapore
      "dd/MM/yyyy", "yyyy-MM-dd",
      "dd/MM/yyyy HH:mm", "yyyy-MM-dd HH:mm:ss",
      "dd/MM/yyyy h:mm tt", "dd MMM yyyy HH:mm",

      // ISO 8601
      "yyyy-MM-ddTHH:mm:ss.fffZ",
      "yyyy-MM-ddTHH:mm:ssZ",
      "yyyy-MM-ddTHH:mm:ss",
      "yyyy-MM-dd HH:mm:ss",
      "yyyyMMddTHHmmssZ"
    };

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      var value = reader.GetString();

      foreach (var format in _formats)
      {
        // Assume universal because frontend already handles the conversion of all times into UTC
        if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var date))
        {
          return date;
        }
      }

      // As a fallback, try standard parsing
      if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var fallbackDate))
      {
        return fallbackDate;
      }

      throw new JsonException($"Unable to parse DateTime from '{value}'");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
      writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
    }
  }
}
