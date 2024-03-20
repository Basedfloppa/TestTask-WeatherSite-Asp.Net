using Microsoft.EntityFrameworkCore;

namespace db_context;

public partial class WeatherContext : DbContext
{
    public WeatherContext() {}

    public WeatherContext(DbContextOptions<WeatherContext> options) : base(options) {}

    public virtual DbSet<Weather> Weathers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=weather;Username=postgres;Password=postgres;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Weather>(entity =>
        {
            entity.HasKey(e => new { e.Date, e.Time }).HasName("Weather_Pk");

            entity.ToTable("weather");

            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Time)
                .HasColumnType("time with time zone")
                .HasColumnName("time");
            entity.Property(e => e.AirDirection).HasColumnName("air_direction");
            entity.Property(e => e.AirMoisture).HasColumnName("air_moisture");
            entity.Property(e => e.AirSpeed).HasColumnName("air_speed");
            entity.Property(e => e.Cloudiness).HasColumnName("cloudiness");
            entity.Property(e => e.DewPoint).HasColumnName("dew_point");
            entity.Property(e => e.HorizontalVisibility).HasColumnName("horisontal_visibility");
            entity.Property(e => e.LowerCloudinessTreshold).HasColumnName("lower_cloudiness_treshold");
            entity.Property(e => e.Pressure).HasColumnName("pressure");
            entity.Property(e => e.Temperature).HasColumnName("temperature");
            entity.Property(e => e.WeatherConditions).HasColumnName("weather_condition");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
