using System.Collections.Specialized;
namespace Sql;

public interface IDataBaseService
{
    public bool CreateDatabase();
    public bool AddDatabase();
    public bool EditDatabase(int rowId, string? username, string? service, string? password);
    public bool DeleteDatabase();
    public IEnumerable<NameValueCollection> GetAllValues();
}