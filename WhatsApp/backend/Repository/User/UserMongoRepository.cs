
using backend.Models;
using MongoDB.Driver;

namespace WhatsApp.Repository;

public class UserMongoRepository : IUserRepository<User>
{
    private readonly IMongoCollection<User> _mongoDBCollection;

    public UserMongoRepository(IMongoDatabase mongoDatabase)
    {
        _mongoDBCollection = mongoDatabase.GetCollection<User>("Users");
    }

    // check about the async here
    public async Task<IReadOnlyCollection<User>> GetAllAsync()
    {
        return await _mongoDBCollection.Find(_ => true).ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(string id)
    {
        return await _mongoDBCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _mongoDBCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByPhoneAsync(string phoneNumber)
    {
        return await _mongoDBCollection.Find(u => u.Phone == phoneNumber).FirstOrDefaultAsync();
    }

    public async Task<User> AddContactAsync(string currentUserId, string contactUserId)
    {
        var currentUser = _mongoDBCollection.Find(u => u.Id == currentUserId).FirstOrDefault();
        var contactUser = _mongoDBCollection.Find(u => u.Id == contactUserId).FirstOrDefault();

        if (currentUser != null && contactUser != null)
        {
            currentUser.Contacts.Add(contactUser.Id);

            var filter = Builders<User>.Filter.Eq(u => u.Id, currentUserId);
            var update = Builders<User>.Update.Set(u => u.Contacts, currentUser.Contacts);

            await _mongoDBCollection.UpdateOneAsync(filter, update);

            return currentUser;
        }

        return null;
    }

    public async Task<User> RemoveContactAsync(string currentUserId, string contactUserId)
    {
        var currentUser = _mongoDBCollection.Find(u => u.Id == currentUserId).FirstOrDefault();
        var contactUser = _mongoDBCollection.Find(u => u.Id == contactUserId).FirstOrDefault();

        if (currentUser == null)
        {
            return null;
        }
        else if (contactUser == null)
        {
            return currentUser;
        }

        currentUser.Contacts.Remove(contactUser.Id);

        var filter = Builders<User>.Filter.Eq(u => u.Id, currentUserId);
        var update = Builders<User>.Update.Set(u => u.Contacts, currentUser.Contacts);

        await _mongoDBCollection.UpdateOneAsync(filter, update);

        return currentUser;
    }

    public async Task<User> AddUserAsync(User user)
    {
        await _mongoDBCollection.InsertOneAsync(user);
        var newUser = await _mongoDBCollection.Find(u => u.Phone == user.Phone).FirstOrDefaultAsync();
        return newUser;
    }

    public async Task<bool> RemoveUserAsync(string id)
    {
        var user = await _mongoDBCollection.FindAsync(u => u.Id == id);

        if (user == null)
        {
            return false;
        }

        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        await _mongoDBCollection.DeleteOneAsync(filter);

        return true;
    }
}