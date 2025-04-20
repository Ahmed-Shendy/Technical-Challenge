using Mapster;
using Microsoft.EntityFrameworkCore;
using Technical_Challenge.Abstractions;
using Technical_Challenge.Entity.contacts;
using Technical_Challenge.Errors;
using Technical_Challenge.model;
using Technical_Challenge.model.Context;
using Technical_Challenge.Pagnations;
using Technical_Challenge.Services.IServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Technical_Challenge.Services;

public class UserServices(ApplicationDbContext applicationDbContext) : IUserServices
{
    private readonly ApplicationDbContext db = applicationDbContext;

    public async Task<Result> AddContact(string userid, Usercontact request, CancellationToken cancellationToken = default)
    {
        var EmailCheck = await db.AddressBooks.AnyAsync(x => x.Email == request.Email, cancellationToken);
        if (EmailCheck)
            return Result.Failure(ContactErrors.EmailUnque);
        var PhoneCheck = await db.AddressBooks.AnyAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (PhoneCheck)
            return Result.Failure(ContactErrors.PhoneUnque);


        //var contact = request.Adapt<AddressBook>();
        //contact.Email = request.Email;
        var contact = new AddressBook
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Birthdate = request.Birthdate
        };
        contact.User_Address_Books.Add(new user_address_book
        {
            UserId = userid,
            AddressBookId = contact.Id
        });
        await db.AddressBooks.AddAsync(contact, cancellationToken);
       
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<PaginatedList<Usercontact>> GetAllContact(string userid, RequestFilters requestFilters, CancellationToken cancellationToken = default)
    {
        var contacts = db.User_Address_Books.Include(i => i.AddressBook)
            .Where(i => i.UserId == userid)
             .Select(i => new Usercontact
             {
                
                 FirstName = i.AddressBook.FirstName!,
                 LastName = i.AddressBook.LastName!,
                 PhoneNumber = i.AddressBook.PhoneNumber!,
                 Email = i.AddressBook.Email!,
                 Birthdate = i.AddressBook.Birthdate
             });
            


        if (!string.IsNullOrWhiteSpace(requestFilters.SearchValue))
        {
            contacts = contacts.Where(i => i.FirstName!.Contains(requestFilters.SearchValue));
        }

        // use this to sort data by column or Rate
        contacts = contacts
                .OrderBy(i => i.FirstName)
                .ThenBy(i => i.LastName)
                .ThenBy(i => i.Birthdate);


        var result = await PaginatedList<Usercontact>.CreateAsync( contacts, requestFilters.PageNumber, requestFilters.PageSize);
        return result!;
    }

    public async Task<Result<Usercontact>> GetById(string userId , int ContactId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.Include(i => i.User_Address_Books).ThenInclude(i => i.AddressBook)
            .SingleOrDefaultAsync(i =>i.Id  == userId, cancellationToken);
        if (user == null)
            return Result.Failure<Usercontact>(UserErrors.UserNotFound);
        var contact = user.User_Address_Books
            .SingleOrDefault(i => i.AddressBookId == ContactId);
        if (contact == null)
            return Result.Failure<Usercontact>(ContactErrors.ContactNotFound);
        
        var result = contact.AddressBook.Adapt<Usercontact>();

        return Result.Success(result);
    }






    public async Task<Result> UpdateContact(string userid, Usercontact request, CancellationToken cancellationToken = default)
    {
        var contact = await db.AddressBooks.Include(i => i.User_Address_Books)
            .SingleOrDefaultAsync(i => i.Email == request.Email && i.User_Address_Books.Any(x => x.UserId == userid), cancellationToken);
        if (contact == null)
            return Result.Failure(ContactErrors.ContactNotFound);
        contact.FirstName = request.FirstName;
        contact.LastName = request.LastName;
        contact.PhoneNumber = request.PhoneNumber;
        contact.Birthdate = request.Birthdate;
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }




    public async Task<Result> DeleteContact(string userid, int ContactId, CancellationToken cancellationToken = default)
    {
       
        var user = await db.Users.Include(i => i.User_Address_Books).ThenInclude(i => i.AddressBook)
            .SingleOrDefaultAsync(i => i.Id == userid, cancellationToken);
        if (user == null)
            return Result.Failure(UserErrors.UserNotFound);
        var contact = user.User_Address_Books
            .SingleOrDefault(i => i.AddressBookId == ContactId);
        if (contact == null)
            return Result.Failure(ContactErrors.ContactNotFound);

        db.User_Address_Books.Remove(contact);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();

    }


}