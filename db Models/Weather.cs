namespace db_context;

public partial class Weather
{
    public DateOnly Date { get; set; }

    public DateTimeOffset Time { get; set; }

    public sbyte Temperature { get; set; }

    public byte AirMoisture { get; set; }

    public sbyte DewPoint { get; set; }

    public ushort Pressure { get; set; }

    public string? AirDirection { get; set; }

    public byte AirSpeed { get; set; }

    public byte? Cloudiness { get; set; }

    public ushort LowerCloudinessTreshold { get; set; }

    public byte HorisontalVisibility { get; set; }

    public string? WeatherConditions{ get; set; }
}
