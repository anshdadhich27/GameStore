using GameStore.Models.Interface;
using System.Threading.Tasks;
using GameStore.Models.Entity;
using Dapper;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.Models.DataProvider
{
    public class UserRepository : IUserRepository
    {
        private const string RegisterUserSP = "User_RegisterUserSP";
        private const string UpdateUserSP = "User_UpdateUserSP";
        private const string GetUserByIdSP = "User_GetUserByIdSP";
        private const string FindUserByEmailSP = "User_FindUserByEmailSP";
        private const string FindUserByEmail_PasswordSP = "User_FindUserByEmail_PasswordSP";
        private const string GenerateEmailConfirmationTokenSP = "User_GenerateEmailConfirmationToken";
        private const string Email_ConfirmationSP = "User_Email_Confirmation";
        private const string Change_PasswordSP = "User_Change_Password";
        private const string Change_Password_By_IdSP = "User_Change_Password_By_Id";
        private const string User_Get_Profile_ScoreSP = "User_Get_Profile_Score";
        private const string GetPaginatedSP = "User_GetPaginated";
        private const string GetPaginated_By_SearchSP = "User_GetPaginated_By_Search";

        private const string Create_User_LoginsSP = "User_Create_User_Logins";
        private const string Check_User_LoginsSP = "User_Check_User_Logins";


        public async Task<PaginatedEntity<User>> GetPaginated(long offset, int rows)
        {
            var list = new PaginatedEntity<User>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<User>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<User>> GetPaginated_By_Search(SearchQuery obj)
        {
            var list = new PaginatedEntity<User>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginated_By_SearchSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<User>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<MyClass> Get_Profile_Score(long Id)
        {

            MyClass obj = new MyClass();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(User_Get_Profile_ScoreSP, new { Id }, commandType: CommandType.StoredProcedure))
                {
                    obj = multiple.ReadFirstOrDefault<MyClass>();
                }
                
            }
            return obj;
        }

        public async Task<DbResult> Change_Password_By_Id(User obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.Password = EncryptionHelper.Encrypt(obj.Password);
                using (var multiple = await con.QueryMultipleAsync(Change_Password_By_IdSP, new { obj.Id, obj.Password }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                }
            }
            return result;
        }

        public async Task<DbResult> Check_User_LoginsAsync(UserLogin obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Check_User_LoginsSP, new { obj.LoginProvider, obj.ProviderKey }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<User>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Create_User_LoginsAsync(UserLogin obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Create_User_LoginsSP, new { obj.UserId, obj.LoginProvider, obj.ProviderKey, obj.ProviderName }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<User>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Change_PasswordAsync(User obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.Password = EncryptionHelper.Encrypt(obj.Password);
                using (var multiple = await con.QueryMultipleAsync(Change_PasswordSP, new { obj.Id, obj.Password, obj.UserKey, obj.SecurityCode }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<User>(); }
                }
            }
            return result;
        }


        public async Task<DbResult> Email_ConfirmationAsync(string UserKey, string SecurityCode)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Email_ConfirmationSP, new { UserKey, SecurityCode }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<User>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> GenerateEmailConfirmationTokenAsync(long Id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                string UserKey = DbHelper.GetUniqueKey(20);
                string SecurityCode = DbHelper.GetUniqueKey(20);
                using (var multiple = await con.QueryMultipleAsync(GenerateEmailConfirmationTokenSP, new { Id, UserKey, SecurityCode }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<User>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> FindUserByEmail_PasswordAsync(string Email, string Password)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                Password = EncryptionHelper.Encrypt(Password);
                using (var multiple = await con.QueryMultipleAsync(FindUserByEmail_PasswordSP, new { Email, Password }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<User>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> FindUserByEmailAsync(string Email)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(FindUserByEmailSP, new { Email }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<User>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> GetUserByIdAsync(long Id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetUserByIdSP, new { Id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<User>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> RegisterUserAsync(User user)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                user.NameUrl = string.Format("{0}-{1}", user.FirstName, user.LastName);
                user.NameUrl = Regex.Replace(user.NameUrl.Trim(), "[^a-zA-Z0-9]", "-");
                user.NameUrl = Regex.Replace(user.NameUrl, @"(?<=(-))\1+", "").ToLower();
                user.NameUrl = user.NameUrl.EndsWith("-") ? user.NameUrl.Remove(user.NameUrl.Length - 1, 1) : user.NameUrl;
                user.Password = EncryptionHelper.Encrypt(user.Password);

                using (var multiple = await con.QueryMultipleAsync(RegisterUserSP, new { user.FirstName, user.LastName, user.NameUrl, user.Email, user.Password, user.ReferalKey }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<User>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> UpdateUserAsync(User user)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                user.NameUrl = string.Format("{0}-{1}", user.FirstName, user.LastName);
                user.NameUrl = Regex.Replace(user.NameUrl.Trim(), "[^a-zA-Z0-9]", "-");
                user.NameUrl = Regex.Replace(user.NameUrl, @"(?<=(-))\1+", "").ToLower();
                user.NameUrl = user.NameUrl.EndsWith("-") ? user.NameUrl.Remove(user.NameUrl.Length - 1, 1) : user.NameUrl;

                using (var multiple = await con.QueryMultipleAsync(UpdateUserSP, new { user.Id, user.ISDCode, user.PhoneNumber, user.Country, user.Gender, user.DOB, user.Avatar, user.Background, user.FirstName, user.LastName, user.NameUrl, user.Email, user.PhoneVefified, user.EmailVerified }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<User>(); }
                }
            }
            return result;
        }
    }
}
