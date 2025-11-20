
namespace Wallet.Interfaces
{
	public interface ISqlDataAccess
	{
		Task<IEnumerable<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters);
		Task SaveDataAsync<T>(string storedProcedure, T parameters);
		Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null);
		Task<T> QuerySingleAsync<T>(string sql, object? parameters = null);
		Task<int> ExecuteAsync(string sql, object? parameters = null);
	}
}
