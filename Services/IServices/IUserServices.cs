using Technical_Challenge.Abstractions;
using Technical_Challenge.Entity.contacts;
using Technical_Challenge.Pagnations;

namespace Technical_Challenge.Services.IServices;

public interface IUserServices
{
    Task<Result> AddContact(string userid, Usercontact request, CancellationToken cancellationToken = default);
    Task<Result<Usercontact>> GetById(string userId, int ContactId, CancellationToken cancellationToken = default);
    Task<Result> DeleteContact(string userid, int ContactId, CancellationToken cancellationToken = default);
    Task<Result> UpdateContact(string userid, Usercontact request, CancellationToken cancellationToken = default);
    Task<PaginatedList<Usercontact>> GetAllContact(string userid, RequestFilters requestFilters, CancellationToken cancellationToken = default);
}
