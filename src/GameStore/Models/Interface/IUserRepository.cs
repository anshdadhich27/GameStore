using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IUserRepository
    {
        Task<DbResult> RegisterUserAsync(User user);
        Task<DbResult> UpdateUserAsync(User user);
        Task<DbResult> GetUserByIdAsync(long Id);
        Task<DbResult> Change_PasswordAsync(User obj);
        Task<DbResult> Change_Password_By_Id(User obj);
        Task<DbResult> FindUserByEmailAsync(string Email);
        Task<DbResult> GenerateEmailConfirmationTokenAsync(long Id);
        Task<DbResult> FindUserByEmail_PasswordAsync(string Email, string Password);
        Task<DbResult> Email_ConfirmationAsync(string UserKey, string SecurityCode);

        Task<DbResult> Create_User_LoginsAsync(UserLogin obj);
        Task<DbResult> Check_User_LoginsAsync(UserLogin obj);
        Task<MyClass> Get_Profile_Score(long Id);
        Task<PaginatedEntity<User>> GetPaginated(long offset, int rows);
        Task<PaginatedEntity<User>> GetPaginated_By_Search(SearchQuery obj);




    }
}
