using System.Text.Json;

try
{
    if (args.Length != 1) throw new ArgumentException("Pass one JSON input file");
    using var data = JsonDocument.Parse(File.ReadAllText(args[0]));
    Console.WriteLine(JsonSerializer.Serialize(Analyze(data.RootElement)));
    return 0;
}
catch (Exception error) when (error is IOException or JsonException or FormatException
                               or ArgumentException or InvalidOperationException
                               or KeyNotFoundException or OverflowException)
{
    Console.Error.WriteLine(error.Message);
    return 2;
}

static object Analyze(JsonElement input)
{
    int original = input.GetProperty("base").GetInt32(), target = input.GetProperty("target").GetInt32();
    if (original <= 0 || target <= 0) throw new ArgumentException("Serving counts must be positive");
    var grams = new Dictionary<string,decimal>();
    foreach (var item in input.GetProperty("ingredients").EnumerateArray())
    {
        if (item.GetArrayLength() != 3) throw new ArgumentException("Each ingredient needs name, grams, and resolution");
        string name = item[0].GetString() ?? "";
        decimal weight = item[1].GetDecimal(), resolution = item[2].GetDecimal();
        if (String.IsNullOrWhiteSpace(name) || weight < 0 || resolution <= 0 || grams.ContainsKey(name))
            throw new ArgumentException("Invalid or duplicate ingredient");
        grams.Add(name, Math.Round(weight * target / original / resolution, MidpointRounding.AwayFromZero) * resolution);
    }
    return new { grams, total = grams.Values.Sum() };
}
