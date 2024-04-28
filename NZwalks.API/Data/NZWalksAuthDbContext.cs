using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZwalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var ReaderRoleId = "5212cbd0-9042-4b21-9f92-791aa9e75358";
            var WriterRoleId = "e7ce0375-20a5-4526-8c75-5bc9476f0fc9";

            var roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = ReaderRoleId,
                    ConcurrencyStamp = ReaderRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                },
                new IdentityRole()
                {
                    Id = WriterRoleId,
                    ConcurrencyStamp= WriterRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };
           
            builder.Entity<IdentityRole>().HasData(roles);
           
        }
    }
}
