using condogenius_api_reservation.Models;
using Microsoft.EntityFrameworkCore;


namespace condogenius_api_reservation.Database 
{
    public class  DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Reservation> Reservations { get; set; }
    }
}