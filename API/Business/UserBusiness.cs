using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Business
{
    #region interface
    public interface IUserBusiness
    {
        Task<Visitor?> GetUserByEmailAsync(string email);
        Task<Visitor> AddVistorAsync(Visitor visitor);
        Task<Tracker> AddTrackerAsync(Tracker tracker);

    }

    #endregion
    public class UserBusiness : IUserBusiness
    {
        private readonly AdoptContext _adoptContext;
        public UserBusiness(AdoptContext adoptContext)
        {
            _adoptContext = adoptContext;
        }

        public async Task<Visitor?> GetUserByEmailAsync(string email)
        {
            return await _adoptContext.Visitors.FirstOrDefaultAsync(v => v.Email == email);
        }

        public async Task<Visitor> AddVistorAsync(Visitor visitor)
        {
            Visitor? existingVisitor = await GetUserByEmailAsync(visitor.Email);
            if (existingVisitor != null)
            {
                return existingVisitor; // Visitor already exists
            }
            _ = _adoptContext.Visitors.Add(visitor);
            _ = await _adoptContext.SaveChangesAsync();
            return visitor;
        }

        public async Task<Tracker> AddTrackerAsync(Tracker tracker)
        {
            _ = _adoptContext.Trackers.Add(tracker);
            _ = await _adoptContext.SaveChangesAsync();
            return tracker;
        }
    }
}
