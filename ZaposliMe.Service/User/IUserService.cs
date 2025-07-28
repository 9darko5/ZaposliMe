namespace ZaposliMe.Service.User
{
    public interface IUserService
    {
        #region command
        public void CreateUser(Guid aspNetUserId, string firstName, string lastName, long? age);
        #endregion
    }
}
