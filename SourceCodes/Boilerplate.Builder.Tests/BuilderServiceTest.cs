using Boilerplate.Builder.Services;
using Boilerplate.Builder.Services.Interfaces;
using Boilerplate.Builder.Services.Utilities;
using Boilerplate.Builder.Services.Utilities.Interfaces;
using NUnit.Framework;

namespace Boilerplate.Builder.Tests
{
	[TestFixture]
	public class BuilderServiceTest
	{
		private ISettings _settings;
		private BuilderService _service;

		#region SetUp / TearDown

		[SetUp]
		public void Init()
		{
			this._settings = Settings.Instance;
			this._service = new BuilderService(this._settings);
		}

		[TearDown]
		public void Dispose()
		{
			
		}

		#endregion SetUp / TearDown

		#region Tests

		[Test]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.DataAccessFramework", 2)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.IoC", 2)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.Repositories", 2)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.Services", 2)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.Services.Extensions", 1)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.Services.Utilities", 1)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.ViewModels", 1)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.Web.UI", 7)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.Web.UI.Tests", 1)]
		public void GetSubdirectories_GivenDirectory_ReturnNumberOfSubdirectories(string directory, int count)
		{
			var subdirectories = this._service.GetSubdirectories(directory);

			Assert.AreEqual(count, subdirectories.Count);
		}

		[Test]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.DataAccessFramework", 11)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.IoC", 8)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.Repositories", 5)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.Services", 6)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.Services.Extensions", 3)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.Services.Utilities", 2)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.ViewModels", 1)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.Web.UI", 9)]
		[TestCase(@"C:\Development\OpenSources\Aliencube.WebApplication.Boilerplate\SourceCodes\Boilerplates\SourceCodes\Application.Web.UI.Tests", 4)]
		public void GetFiles_GivenDirectory_ReturnNumberOfFiles(string directory, int count)
		{
			var subdirectories = this._service.GetFilesFromDirectory(directory);

			Assert.AreEqual(count, subdirectories.Count);
		}

		#endregion Tests
	}
}