using Microsoft.Graph;
using Microsoft.Graph.Models;
using YCNBot.Core.Repositories;

namespace YCNBot.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GraphServiceClient _graphServiceClient;

        public UserRepository(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        public async Task<Core.Entities.User?> GetUserDetails(Guid id)
        {
            var requestBody = new Microsoft.Graph.Users.GetByIds.GetByIdsPostRequestBody
            {
                Ids = new List<string>
                {
                    id.ToString()
                },
                Types = new List<string>
                {
                    "user"
                }
            };

            User? user = await _graphServiceClient
                .Users[id.ToString()]
                .GetAsync();

            if (user != null)
            {
                return new Core.Entities.User
                {
                    Id = id,
                    Department = user.Department,
                    FirstName = user.GivenName,
                    LastName = user.Surname,
                    Email = user.Mail,
                    JobTitle = user.JobTitle
                };
            }

            return null;
        }

        public async Task<Dictionary<string, Core.Entities.User>?> GetUserDetails(IEnumerable<Guid> ids)
        {
            try
            {
                IEnumerable<User>? users = (await _graphServiceClient
                        .Users
                        .GetAsync(request =>
                        {
                            request.QueryParameters.Top = 500;
                            request.QueryParameters.Count = true;
                            request.QueryParameters.Orderby = new string[] { "displayName" };
                            request.QueryParameters.Select = new string[]
                            {
                                "id",
                                "department",
                                "givenName",
                                "jobTitle",
                                "mail",
                                "surname"
                            };
                            request.Headers.Add("ConsistencyLevel", "eventual");
                            request.QueryParameters.Filter = $"id in ({string.Join(", ", ids.Select(x => $"'{x}'"))})";

                        }))?.Value;

                if (users != null)
                {
                    return users
                        .Where(user => user.Id != null)
                        .ToDictionary(user => user.Id ?? "", user => new Core.Entities.User
                        {
                            Id = Guid.Parse(user.Id ?? ""),
                            Department = user.Department,
                            FirstName = user.GivenName,
                            LastName = user.Surname,
                            Email = user.Mail,
                            JobTitle = user.JobTitle
                        });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
    }
}
