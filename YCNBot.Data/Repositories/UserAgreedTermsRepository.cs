using Microsoft.EntityFrameworkCore;
using YCNBot.Core.Entities;
using YCNBot.Core.Repositories;

namespace YCNBot.Data.Repositories
{
    public class UserAgreedTermsRepository : Repository<UserAgreedTerms>, IUserAgreedTermsRepository
    {
        public UserAgreedTermsRepository(YCNBotContext context) : base(context) { }

        public async Task<UserAgreedTerms?> GetByUser(Guid userIdentifier)
        {
            return await _context.UserAgreedTerms
                .FirstOrDefaultAsync(x => x.UserIdentifier == userIdentifier);
        }

    }
}
