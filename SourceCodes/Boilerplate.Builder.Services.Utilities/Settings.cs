namespace Boilerplate.Builder.Services.Utilities
{
	/// <summary>
	/// This represents the application configuration settings entity.
	/// </summary>
	public class Settings
	{
		#region Constructors

		/// <summary>
		///	Initialises a new instance of the Settings object as private.
		/// </summary>
		private Settings()
		{
		}

		#endregion Constructors

		#region Fields

		private static Settings _instance;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets the instance of the settings object.
		/// </summary>
		public static Settings Instance
		{
			get
			{
				if (_instance == null)
					_instance = new Settings();
				return _instance;
			}
		}

		#endregion Properties

		#region Methods

		#endregion Methods
	}
}