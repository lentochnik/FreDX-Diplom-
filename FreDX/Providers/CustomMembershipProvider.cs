using System;
using System.Linq;
using System.Web.Security;
using System.Web.Helpers;
using FreDX.Models;
using FreDX.Functions;



namespace FreDX.Providers
{
    public class CustomMembershipProvider : MembershipProvider
    {
//-------------------Функция Авторизаций пользователя-------------------------------------

        public override bool ValidateUser(string Name, string Password)
        {
            bool isValid = false; // не авторизирован

            using (UserContext _db = new UserContext()) // создаем новый экземпляр подключения
            {
                User user = _db.Users.FirstOrDefault(u => u.Name == Name); // Проверяем пользователя в базе данных

                if (user != null && Crypto.VerifyHashedPassword(user.Password, Password)) // имя пользователя и пароль  если не нул то
                {
                    isValid = true;
                  
                    using (LogdbContent _db2 = new LogdbContent()) // создаем новый экземпляр подключения к базе данных логов
                    {
                        Log log = new Log(); // определяем ссылку на класс
                        string s = "Loging"; // описание действия
                        GetIP gp = new GetIP(); // определяем ссылку на класс получения ipадреса
                        string userip = gp.GetIPAddress(); // выполняем функцию и записываем значение в переменную userip 
                        // определяем структуру пользователя
                        log.Name = Name; 
                        log.Ip = userip;
                        log.Last_login = DateTime.Now;
                        log.Procedure = s;
                        // добавляем запись в базу данных logs.logs
                        _db2.logss.Add(log);
                        _db2.SaveChanges();
                    }
                }
                return isValid; // возвращаем значение валидации истина
            }
        }
        //---------------- функция создания пользователя--------------------------------------

        public MembershipUser CreateUser(string Name, string Password, string Post)
        {
            MembershipUser membershipUser = GetUser(Name, false);


            if (membershipUser == null)
            {
                try
                {
                    using (UserContext _db = new UserContext())
                    {
                        User user = new User();
                        user.Name = Name;
                        user.Password = Crypto.HashPassword(Password);
                        user.CreationDate = DateTime.Now;

                        if (_db.Roles.Find(6) != null)
                        {
                            user.RoleId = 6;
                        }

                        _db.Users.Add(user);
                        _db.SaveChanges();

                        using (LogdbContent _db2 = new LogdbContent())
                        {
                            Log log = new Log();
                            string s = "Registred";
                            GetIP gp = new GetIP();
                            string userip = gp.GetIPAddress();

                            log.Name = Name;
                            log.Ip = userip;
                            log.Last_login = DateTime.Now;
                            log.Procedure = s;

                            _db2.logss.Add(log);
                            _db2.SaveChanges();
                        }

                        membershipUser = GetUser(Name, false);
                        return membershipUser;
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

//-------------------------Функциясоздания экземпляра членства пользователя--------------------------------------------------
//-- Это билет cookies который выдается при авторизации и регистрации

        public override MembershipUser GetUser(string Name, bool userIsOnline)
        {
            try
            {
                using (UserContext _db = new UserContext())
                {
                    var users = from u in _db.Users
                                where u.Name == Name
                                select u;
                    if (users.Count() > 0)
                    {
                        User user = users.First();
                        MembershipUser memberUser = new MembershipUser("CustomMembershipProvider", user.Name, null, null, null, null,
                            false, false, user.CreationDate, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                        return memberUser;
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string Post, string email, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }
        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }
        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }
        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
    }
}