using backend.Models;
using MongoDB.Driver;

namespace backend.Repository.VerifyCodeRepository
{
    public class VerifyRepository : IVerifyRepository<VerifyCode>
    {
        private readonly IMongoCollection<VerifyCode> _mongoDBCollection;
        public VerifyRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDBCollection = mongoDatabase.GetCollection<VerifyCode>("VerifyCode");
        }
        public async Task<IReadOnlyCollection<VerifyCode>> GetAllAsync()
        {
            return await _mongoDBCollection.Find(_ => true).ToListAsync();
        }

        public async Task<VerifyCode> GetByUserIdAsync(string id)
        {
            var verifyCode = await _mongoDBCollection.Find(v => v.UserId == id).FirstOrDefaultAsync();
            return verifyCode == null ? null : verifyCode;
        }

        public async Task<VerifyCode> PostAsync(VerifyCode verifyCode)
        {
            await _mongoDBCollection.InsertOneAsync(verifyCode);

            var newVerifyCode = await _mongoDBCollection.Find(v => v.UserId == verifyCode.UserId).FirstOrDefaultAsync();
            return newVerifyCode;
        }

        public async Task<VerifyCode> PutAsync(string id, int code)
        {
            var filter = Builders<VerifyCode>.Filter.Eq(v => v.Id, id);

            var update = Builders<VerifyCode>.Update
                .Set(v => v.CreatedTime, DateTime.UtcNow)
                .Set(v => v.CodeNumber, code);

            var result = await _mongoDBCollection.FindOneAndUpdateAsync(
                filter,
                update,
                new FindOneAndUpdateOptions<VerifyCode>
                {
                    ReturnDocument = ReturnDocument.After
                });

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var verify = await _mongoDBCollection.FindAsync(v => v.Id == id);

            if (verify == null)
            {
                return false;
            }

            var filter = Builders<VerifyCode>.Filter.Eq(v => v.Id, id);
            await _mongoDBCollection.DeleteOneAsync(filter);

            return true;
        }
    }
}