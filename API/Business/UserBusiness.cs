using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Business
{
        #region interface
    public interface IUserBusiness
    {
        public  Task<Visitor?> GetUserByEmailAsync(string email);
        public  Task<Visitor> AddVistorAsync(Visitor visitor);
        public Task<Tracker> AddTrackerAsync(Tracker tracker);

    }

        #endregion
    public class UserBusiness: IUserBusiness
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
            var existingVisitor = await GetUserByEmailAsync(visitor.Email);
            if (existingVisitor != null)
            {
                return existingVisitor; // Visitor already exists
            }
            _adoptContext.Visitors.Add(visitor);
            await _adoptContext.SaveChangesAsync();
            return visitor;
        }

        public async Task<Tracker> AddTrackerAsync(Tracker tracker)
        {
            _adoptContext.Trackers.Add(tracker);
            await _adoptContext.SaveChangesAsync();
            return tracker;
        }
    }
}
