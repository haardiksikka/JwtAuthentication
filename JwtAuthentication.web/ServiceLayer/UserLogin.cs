using JwtAuthentication.endpoints.EndPoint;
using ServiceStack.ServiceInterface;
using JwtAuthentication.application.Database.Repository;
using JwtAuthentication.application.JwtToken;

namespace JwtAuthentication.web.ServiceLayer
{
    public class UserLogin : Service
    {
        private readonly UserDal userdal;
        private readonly TokenFactory token;
        public UserLogin()
        {
            userdal = new UserDal();
            token = new TokenFactory();
        }
        public object POST(Login userDetail)
        {
            var loginUser = userDetail.User;
            if(loginUser == null)
            {
                return false;
            }
            var user = userdal.FindUserAsync(loginUser.Username, loginUser.Password);
            if (user == null || !user.IsActive)
            {
                return false;
            }
            var result = token.CreateJwtTokenAsync(user);
            return new { access_token = result.AccessToken, refresh_token = result.RefreshToken };

        }
    }
}