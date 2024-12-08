using System.Collections.Generic;
using System.Windows.Controls;

namespace Flidais.Helper
{
	public static class DataTypeHelper
	{
		public static void PopulateFileExtensionBox(string type, ListBox listBox)
		{
			List<string> list = new List<string>();
			listBox.Items.Clear();
			switch (type)
			{
				case "Select Media Type":
					break;
				case "Image":
					list = GetImageTypes();
					break;
				case "Video":
					list = GetVideoTypes();
					break;
				case "Documents":
					list = GetDocumentTypes();
					break;
				case "Audio":
					list = GetAudioTypes();
					break;
				case "Models":
					list = GetModelTypes();
					break;
				default:
					throw new System.Exception("unreckognized media type selected");
			}
			foreach (string file in list)
			{
				listBox.Items.Add(file);
			}
		}
		public static List<string> GetMediaTypes()
		{
			List<string> list = new List<string>()
			{
				"Select Media Type", "Image", "Video", "Documents", "Audio", "Models"
			};
			return list;
		}
		private static List<string> GetImageTypes()
		{
			List<string> list = new List<string>()
			{
				".png", ".jpg", ".jpeg", ".gif", ".bmp", ".tiff", ".tif", ".svg", ".webp", ".heic",
				".heif", ".ico", ".raw", ".psd"
			};
			return list;
		}
		private static List<string> GetVideoTypes()
		{
			List<string> list = new List<string>()
			{
				".mp4", ".mov", ".avi", "mkv", ".flv", ".wmv", ".webm", ".m4v", ".3gp", ".mpg", ".mpeg", ".vob", ".ogv",
				".rm", ".rmvb", ".ts", ".mxf"
			};
			return list;
		}
		private static List<string> GetDocumentTypes()
		{
			List<string> list = new List<string>()
			{
				".doc", ".docx", ".pdf", ".txt", ".rtf", ".odt", ".xls", ".xlsx", ".ppt", ".pptx", ".csv", ".html",
				".htm", ".epub", ".mobi", ".xps", ".md", ".wps", ".tex"
			};
			return list;
		}
		private static List<string> GetAudioTypes()
		{
			List<string> list = new List<string>()
			{
				".mp3", ".wav", ".flac", ".aac", ".ogg", ".wma", ".m4a", ".aiff", ".alac", "pcm", ".amr", ".opus", ".midi",
				".mid"
			};
			return list;
		}
		private static List<string> GetModelTypes()
		{
			List<string> list = new List<string>()
			{
				".obj", ".fbx", ".stl", ".dae", ".glb", ".gltf", ".3ds", ".blend", ".ply", ".x3d", ".wrl", ".max", ".ma",
				".mb", ".skp", ".lwo", ".sldprt", ".sldasm", ".iges", ".igs", ".step", ".stp", ".bvh", ".abc"
			};
			return list;
		}
	}
}
