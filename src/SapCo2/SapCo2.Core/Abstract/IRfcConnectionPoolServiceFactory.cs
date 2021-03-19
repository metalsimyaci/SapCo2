namespace SapCo2.Core.Abstract
{
    public interface IRfcConnectionPoolServiceFactory
    {
        IRfcConnectionPool GetService(string serverAlias);
    }
}
