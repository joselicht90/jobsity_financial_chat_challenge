using Josbity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobsityFinancialChat.Data
{
    public class JobsityContext : IdentityDbContext<JobsityUser>
    {
        public JobsityContext(DbContextOptions<JobsityContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Message>()
                .HasOne<JobsityUser>(a => a.Sender)
                .WithMany(d => d.Messages)
                .HasForeignKey(d => d.UserID);

            builder.Entity<Command>()
                .ToTable("Commands")
                .HasData(
                    new Command
                    {
                        Id = 1,
                        CommandName = "stock",
                        CommandDescription = "This command gets the information for a single stock and retrieve it's symbol and value per share",
                        StockBotEndpoint = "GetStock?stockCode="
                    }
                );


        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Command> Commands { get; set; }
    }
}
