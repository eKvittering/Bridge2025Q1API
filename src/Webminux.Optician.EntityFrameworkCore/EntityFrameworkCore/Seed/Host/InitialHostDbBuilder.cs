namespace Webminux.Optician.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly OpticianDbContext _context;

        public InitialHostDbBuilder(OpticianDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new UserTypeCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new ActivityArtCreator(_context).Create();
            new ActivityTypeCreator(_context).Create();
            _context.SaveChanges();
        }
    }
}
