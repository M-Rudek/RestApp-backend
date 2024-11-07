using RestApp.Entities;

namespace RestApp
{
    public class AppSeeder
    {
        private readonly AppDbContext dbContext;

        public AppSeeder(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Seed()
        {
            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                dbContext.SaveChanges();
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Admin"
                },
            };

            return roles;
        }
    }
}
