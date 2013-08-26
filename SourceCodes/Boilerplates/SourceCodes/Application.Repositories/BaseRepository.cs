using Application.Repositories.Interfaces;

namespace Application.Repositories
{
	/// <summary>
	/// This represents the base repository class inherited to all repository classes.
	/// </summary>
	public abstract class BaseRepository : IBaseRepository
	{
		/// <summary>
		/// Adds a new item object to the repository.
		/// </summary>
		/// <param name="item">Item object.</param>
		/// <returns>Returns the item object added.</returns>
		public abstract T Add<T>(T item);

		/// <summary>
		/// Gets the item object by itemId.
		/// </summary>
		/// <param name="itemId">Item Id.</param>
		/// <returns>Returns the item object.</returns>
		public abstract T Get<T>(int itemId);

		/// <summary>
		/// Updates the item object.
		/// </summary>
		/// <param name="item">Item object.</param>
		public abstract void Update<T>(T item);

		/// <summary>
		/// Deletes the item object from the repository.
		/// </summary>
		/// <param name="item">Item object.</param>
		public abstract void Delete<T>(T item);
	}
}