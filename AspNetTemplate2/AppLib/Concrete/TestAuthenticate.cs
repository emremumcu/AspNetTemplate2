namespace AspNetTemplate2.AppLib.Concrete
{
    using AspNetTemplate2.AppLib.Abstract;

    public class TestAuthenticate : IAuthenticate
    {
        public bool AuthenticateUser(string userId, string password)
        {
            return true;
        }
    }
}
