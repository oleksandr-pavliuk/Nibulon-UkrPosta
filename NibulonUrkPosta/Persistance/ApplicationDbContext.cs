namespace NibulonUrkPosta.Persistance;

public class ApplicationDbContext : DbContext
{
    public DbSet<PostIndex> PostIndexes { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Region> Regions { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PostIndex>(entity =>
        {
            entity.ToTable("AUP");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PostIndexCode).HasColumnName("INDEX_A");
            entity.Property(e => e.CityId).HasColumnName("CITY");
            entity.Property(e => e.CityName).HasColumnName("NCITY");
            entity.Property(e => e.DistrictId).HasColumnName("RAJ");
            entity.Property(e => e.DistrictName).HasColumnName("NRAJ");
            entity.Property(e => e.RegionId).HasColumnName("OBL");
            entity.Property(e => e.RegionName).HasColumnName("NOBL");

            entity.HasOne(e => e.City);
            entity.HasOne(e => e.District);
            entity.HasOne(e => e.Region);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("CITY");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("CITY_KOD");
            entity.Property(e => e.CityName).HasColumnName("CITY");
            entity.Property(e => e.DistrictId).HasColumnName("KRAJ");
            entity.Property(e => e.RegionId).HasColumnName("OBL");
            entity.HasOne(e => e.District).WithMany(e => e.Cities).HasForeignKey(d => d.DistrictId);
            entity.HasOne(e => e.Region).WithMany(e => e.Cities).HasForeignKey(d => d.RegionId);
        });
        
        modelBuilder.Entity<District>(entity =>
        {
            entity.ToTable("RAJ");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("KRAJ");
            entity.Property(e => e.DistrictName).HasColumnName("RAJ");
        });
        
        modelBuilder.Entity<Region>(entity =>
        {
            entity.ToTable("OBL");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("OBL");
            entity.Property(e => e.RegionName).HasColumnName("NOBL");
        });
    }
}