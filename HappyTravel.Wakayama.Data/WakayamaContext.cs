using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Wakayama.Data;

public class WakayamaContext : DbContext
{
    public WakayamaContext(DbContextOptions<WakayamaContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }

}