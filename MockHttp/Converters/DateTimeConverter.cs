
namespace MockHttp.Converters;

public class NullableDateTimeConverter : JsonConverter<DateTime?>
{
    private const string Format = "yyyy-MM-dd HH:mm:ss";

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrWhiteSpace(str))
            return null;

        return DateTime.TryParse(str, out DateTime date)
            ? date
            : null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToString(Format));
        else
            writer.WriteNullValue();
    }
}
public class NullableDateTimeConverterTwo : JsonConverter<DateTime?>
{
    private static readonly string[] Formats = new[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd" };

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrWhiteSpace(str))
            return null;

        if (DateTime.TryParse(str, out DateTime date))
            return date;
        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd HH:mm:ss"));
        else
            writer.WriteNullValue();
    }
}




public class DateTimeConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd HH:mm:ss";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (DateTime.TryParseExact(str, Format, null, System.Globalization.DateTimeStyles.None, out var date))
        {
            return date;
        }
        return DateTime.Parse(str!); // fallback
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}
