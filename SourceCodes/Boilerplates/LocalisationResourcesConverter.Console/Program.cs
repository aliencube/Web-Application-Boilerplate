using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace LocalisationResourcesConverter.Console
{
    /// <summary>
    /// This represents the main program entry.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Execute the main method.
        /// </summary>
        /// <param name="args">Arguments from console.</param>
        static void Main(string[] args)
        {
            try
            {
                ShowSplitHeader();

                var param = GetParameter(args.ToList());
                if (param == null)
                    ShowHelp();
                else
                    ProcessRequests(param);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine();
                System.Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Shows the splash header.
        /// </summary>
        private static void ShowSplitHeader()
        {
            var messages = new StringBuilder();
            messages.AppendLine();
            messages.AppendLine("Aliencube Localisation Resource Converter Console:");
            messages.AppendLine("----------------");
            messages.AppendLine("");

            System.Console.WriteLine(messages.ToString());
        }

        /// <summary>
        /// Shows the help message.
        /// </summary>
        private static void ShowHelp()
        {
            var messages = new StringBuilder();
            messages.AppendLine("Usage:");
            messages.AppendLine("/t|/test (Option)           Flag if this conversion is for the test purpose.");
            messages.AppendLine("/cc|     (Option)           Flag if this conversion includes culture-specific.");
            messages.AppendLine("/s:[Source Directory]       Source directory to read .resx files.");
            messages.AppendLine("/d:[Destination Directory]  Destination directory to save .xml files.");
            messages.AppendLine("");
            messages.AppendLine("Output:");
            messages.AppendLine("  [Filename].resx --> [Filename].xml : SUCCESS.");
            messages.AppendLine("  [Filename].resx --> [Filename].xml : !!FAIL!!");
            messages.AppendLine("");

            System.Console.WriteLine(messages.ToString());
        }

        /// <summary>
        /// Gets the parameter from the arguments passed from the console.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        /// <returns>Returns the parameter.</returns>
        private static Parameter GetParameter(IList<string> args)
        {
            var param = new Parameter();
            if (args != null && args.Count > 0)
            {
                foreach (var arg in args)
                {
                    if (arg.ToLower() == "/t" || arg.ToLower() == "/test")
                        param.IsTest = true;
                    else if (arg.ToLower() == "/cc")
                        param.WithCountryCode = true;
                    else if (arg.ToLower().StartsWith("/s:"))
                        param.SourceDirectory = arg.Replace("/s:", "");
                    else if (arg.ToLower().StartsWith("/d:"))
                        param.DestinationDirectory = arg.Replace("/d:", "");
                }
            }
            //	Sets the source directory to the executable path,
            //	If the source directory is not set.
            if (String.IsNullOrWhiteSpace(param.SourceDirectory))
                param.SourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace(@"\bin\Debug", "");
            //	Sets the destination directory to the same as the source directory,
            //	If the destination directory is not set.
            if (String.IsNullOrWhiteSpace(param.DestinationDirectory))
                param.DestinationDirectory = param.SourceDirectory;
            return param;
        }

        /// <summary>
        /// Processes the conversion request.
        /// </summary>
        /// <param name="param">Parameter.</param>
        private static void ProcessRequests(Parameter param)
        {
            if (!ValidateDirectory(param.SourceDirectory, true))
                throw new ApplicationException("Invalid source directory");
            if (!ValidateDirectory(param.DestinationDirectory))
                throw new ApplicationException("Invalid destination directory");

            foreach (var filepath in Directory.GetFiles(param.SourceDirectory, "Resource*.resx"))
            {
                var sourceFile = filepath;
                var targetFile = GetTargetFile(filepath.Replace(param.SourceDirectory, param.DestinationDirectory),
                                                  param.WithCountryCode);
                if (ConvertResxToXml(sourceFile, targetFile, param.IsTest))
                    System.Console.WriteLine(sourceFile + " --> " + targetFile + " : SUCCESS.");
                else
                    System.Console.WriteLine(sourceFile + " --> " + targetFile + " : !!FAIL!!");
            }
        }

        /// <summary>
        /// Gets the target filename.
        /// </summary>
        /// <param name="filepath">File path.</param>
        /// <param name="withCountryCode">Flag if the culture requires a specific country or not.</param>
        /// <returns>Returns the target filename.</returns>
        private static string GetTargetFile(string filepath, bool withCountryCode = false)
        {
            var targetFile = filepath.Replace(".resx", ".xml");
            return targetFile;
        }

        /// <summary>
        /// Converts the .resx file to .xml file.
        /// </summary>
        /// <param name="sourceFile">Source file to read.</param>
        /// <param name="targetFile">Destination file to save.</param>
        /// <param name="isTest">Flag if this is a test conversion or not.</param>
        /// <returns>Returns <c>True</c>, if converted successfully; otherwise returns <c>False</c>.</returns>
        private static bool ConvertResxToXml(string sourceFile, string targetFile, bool isTest)
        {
            bool converted;
            var resx = new XmlDocument();
            try
            {
                var xml = new XmlDocument();
                xml.AppendChild(xml.CreateXmlDeclaration("1.0", "utf-8", null));

                var items = xml.CreateElement("items");
                resx.Load(sourceFile);

                var nodes = resx.SelectNodes("//data").Cast<XmlNode>().ToList();
                foreach (var node in nodes.OrderBy(p => p.Attributes["name"].Value))
                {
                    var item = xml.CreateElement("item");
                    item.SetAttribute("key", node.Attributes["name"].Value);
                    item.InnerText = node.SelectSingleNode("value").InnerText;

                    items.AppendChild(item);
                }
                var resource = xml.CreateElement("resource");
                resource.AppendChild(items);

                xml.AppendChild(resource);
                if (!isTest)
                {
                    if (File.Exists(targetFile))
                        File.Delete(targetFile);
                    xml.Save(targetFile);
                }
                converted = true;
            }
            catch
            {
                converted = false;
            }
            return converted;
        }

        /// <summary>
        /// Validates if both directory and .resx files under the directory exist or not.
        /// </summary>
        /// <param name="directory">Directory path.</param>
        /// <param name="checkFiles">Check if .resx files exist or not.</param>
        /// <returns>Returns <c>True</c>, if both directory and .resx files under the directory exist; otherwise returns <c>False</c>.</returns>
        private static bool ValidateDirectory(string directory, bool checkFiles = false)
        {
            var validated = false;
            try
            {
                if (!String.IsNullOrWhiteSpace(directory) && Directory.Exists(directory))
                {
                    validated = true;
                    if (checkFiles)
                    {
                        var files = Directory.GetFiles(directory, "Resource*.resx").ToList();
                        validated = files.Count > 0;
                    }
                }
            }
            catch
            {
                validated = false;
            }
            return validated;
        }
    }

    /// <summary>
    /// This represents the parameter for the program.
    /// </summary>
    public class Parameter
    {
        #region Constructors
        /// <summary>
        /// Initialises the instance of Parameter object.
        /// </summary>
        public Parameter()
        {
            this.IsTest = false;
            this.SourceDirectory = String.Empty;
            this.DestinationDirectory = String.Empty;
            this.WithCountryCode = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the flag if this is for test or not.
        /// </summary>
        public bool IsTest { get; set; }

        /// <summary>
        /// Gets or sets the source directory where the resource files are located.
        /// </summary>
        public string SourceDirectory { get; set; }

        /// <summary>
        /// Gets or sets the destination (target) directory where the converted XML files will be located.
        /// </summary>
        public string DestinationDirectory { get; set; }

        /// <summary>
        /// Gets or sets the flag if the country code needs to be included or not.
        /// </summary>
        public bool WithCountryCode { get; set; }
        #endregion
    }
}
