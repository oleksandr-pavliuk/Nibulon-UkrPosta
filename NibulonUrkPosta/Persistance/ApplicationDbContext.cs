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
        entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
        entity.Property(e => e.PostIndexCode).HasColumnName("INDEX_A").HasColumnType("nvarchar(6)");
        entity.Property(e => e.CityId).HasColumnName("CITY").HasColumnType("int");
        entity.Property(e => e.CityName).HasColumnName("NCITY").HasColumnType("nvarchar(200)");
        entity.Property(e => e.DistrictId).HasColumnName("RAJ").HasColumnType("int");
        entity.Property(e => e.DistrictName).HasColumnName("NRAJ").HasColumnType("nvarchar(200)");
        entity.Property(e => e.RegionId).HasColumnName("OBL").HasColumnType("smallint");
        entity.Property(e => e.RegionName).HasColumnName("NOBL").HasColumnType("nvarchar(200)");

        // Update foreign key relationships to prevent multiple cascade paths
        entity.HasOne(e => e.City).WithMany().HasForeignKey(e => e.CityId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(e => e.District).WithMany().HasForeignKey(e => e.DistrictId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(e => e.Region).WithMany().HasForeignKey(e => e.RegionId).OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Entity<City>(entity =>
    {
        entity.ToTable("CITY");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("CITY_KOD").ValueGeneratedOnAdd();
        entity.Property(e => e.CityName).HasColumnName("CITY").HasColumnType("nvarchar(200)");
        entity.Property(e => e.DistrictId).HasColumnName("KRAJ").HasColumnType("int");
        entity.Property(e => e.RegionId).HasColumnName("OBL").HasColumnType("smallint");

        // Update foreign key relationships to prevent multiple cascade paths
        entity.HasOne(e => e.District).WithMany(e => e.Cities).HasForeignKey(d => d.DistrictId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(e => e.Region).WithMany(e => e.Cities).HasForeignKey(d => d.RegionId).OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Entity<District>(entity =>
    {
        entity.ToTable("RAJ");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("KRAJ").HasColumnType("int");
        entity.Property(e => e.DistrictName).HasColumnName("RAJ").HasColumnType("nvarchar(200)");
    });

    modelBuilder.Entity<Region>(entity =>
    {
        entity.ToTable("OBL");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("OBL").HasColumnType("smallint");
        entity.Property(e => e.RegionName).HasColumnName("NOBL").HasColumnType("nvarchar(200)");
    });
}
}