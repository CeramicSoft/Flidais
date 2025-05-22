using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Flidais.Enum;
using Flidais.Helper;


namespace Flidais
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region fields
		//number counters
		private double totalMedia = 0;
		private long totalData = 0;
		//brushes
		private static readonly Brush black;
		private static readonly Brush lightGray;
		private static readonly Brush darkGray;
		private static readonly Brush white;
		#endregion
		#region constructors
		static MainWindow()
		{
			//defines brushes
			black = new SolidColorBrush(Color.FromRgb(0, 0, 0));
			lightGray = new SolidColorBrush(Color.FromRgb(185, 185, 185));
			darkGray = new SolidColorBrush(Color.FromRgb(148, 148, 148));
			white = new SolidColorBrush(Color.FromRgb(255, 255, 255));
		}
		public MainWindow()
		{
			InitializeComponent();
		}
		private void WindowLoaded(object sender, RoutedEventArgs e)
		{
			SetUp();
		}
		#endregion
		#region methods
		private void FileCopyButtonClick(object sender, RoutedEventArgs e)
		{
			ProcessFiles((from, to) =>
			{
				File.Copy(from, to, true);
			}, ProcessTypes.Copy);
		}
		private void FileTransferButtonClick(object sender, RoutedEventArgs e)
		{
			ProcessFiles((from, to) =>
			{
				File.Move(from, to);
			}, ProcessTypes.Move);
		}
		private void FileDeleteButtonClick(object sender, RoutedEventArgs e)
		{
			ProcessFiles((from, to) =>
			{
				File.Delete(from);
			}, ProcessTypes.Delete);
		}
		private void ToBrowseClick(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.ShowDialog();
			PathToTextBox.Text = dialog.SelectedPath;
		}
		private void FromBrowseClick(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.ShowDialog();
			PathFromTextBox.Text = dialog.SelectedPath;
		}
		private void OnMediaTypeChange(object sender, RoutedEventArgs e)
		{
			DataTypeHelper.PopulateFileExtensionBox(MediaTypeComboBox.SelectedItem.ToString(), FileExtensionListBox);
		}
		/// <summary>
		/// sets the theme to be more friendly to the eyes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DarkMode(object sender, RoutedEventArgs e)
		{
			UIHelper.UpdateControlColors(this, black, lightGray, darkGray);
		}
		/// <summary>
		/// scorches users retinas with the full power of the sun
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LightMode(object sender, RoutedEventArgs e)
		{
			UIHelper.UpdateControlColors(this, white, black, lightGray);
		}
		/// <summary>
		/// checks if you can scan files if everything neccessary is present it proceeds
		/// </summary>
		/// <param name="action">action you wish to enact on the files</param>
		private void ProcessFiles(Action<string, string> action, ProcessTypes processTypes)
		{
			string errorMessage = "";

			//checks if the end location exists
			if (!Directory.Exists(PathToTextBox.Text) && processTypes != ProcessTypes.Delete)
			{
				errorMessage += "Path to is incorrect." + Environment.NewLine;
			}

			//checks if the file origin exists
			if (!Directory.Exists(PathFromTextBox.Text))
			{
				errorMessage += "Path from is incorrect." + Environment.NewLine;
			}

			//makes sure an extension is selected
			if (FileExtensionListBox.SelectedItem == null)
			{
				errorMessage += "File extension is not selected." + Environment.NewLine;
			}

			//makes sure if zipping the zip file has a name
			if (ZipCheckBox.IsChecked == true && ZipName.Text == "")
			{
				errorMessage += "Please enter a name for the zip file";
			}

			//runs the process
			if (string.IsNullOrEmpty(errorMessage))
			{
				try
				{
					string finalPath;

					//adds the zipname to the final path
					if (ZipCheckBox.IsChecked == true)
					{
						finalPath = Path.Combine(PathToTextBox.Text, ZipName.Text);
					}
					else
					{
						finalPath = PathToTextBox.Text;
					}

					Directory.CreateDirectory(finalPath);

					//applies the action to each file in directory
					foreach (string extension in FileExtensionListBox.SelectedItems)
					{
						ScanFiles(PathFromTextBox.Text, finalPath, action, $"*{extension}");
					}

					//checks for repetitive media
					if (ImageCheckCheckBox.IsChecked == true)
					{
						CheckRepetitvieMedia();
					}

					//zips the file
					if (ZipCheckBox.IsChecked == true)
					{
						ZipFile.CreateFromDirectory(finalPath, $"{finalPath}.zip");
						Directory.Delete(finalPath, true);
					}

					System.Windows.MessageBox.Show("File transfer complete");
				}
				catch (Exception ex)
				{
					System.Windows.MessageBox.Show(ex.Message);
				}
			}
			else
			{
				System.Windows.MessageBox.Show(errorMessage);
			}
		}

		private void CheckRepetitvieMedia(string file, string fileExtension)
		{
			Dictionary<string, byte[]> mediaHashes = new Dictionary<string, byte[]>();
		}
		private Dictionary<string, byte[]> GetHashesFromFolder(string file, string fileExtension)
		{
			IEnumerable<string> folders = Directory.EnumerateDirectories(file);
			IEnumerable<string> files = Directory.GetFiles(file, fileExtension);
			Dictionary<string, byte[]> mediaHashes = new Dictionary<string, byte[]>();
			foreach (string folder in folders)
			{
				Dictionary<string, byte[]> hashCollection = GetHashesFromFolder(folder, fileExtension);
				foreach (KeyValuePair<string, byte[]> hash in hashCollection)
				{
					mediaHashes.Add(hash.Key, hash.Value);
				}
			}
			foreach (string media in files)
			{
				mediaHashes.Add(media, getHashFromPath(media));
			}
			return mediaHashes;
		}
		private byte[] getHashFromPath(string path)
		{
			FileStream fileStream = File.OpenRead(path);
			SHA256 hash = SHA256.Create();
			return hash.ComputeHash(fileStream);
		}
		private void IsImageIdentical(KeyValuePair<string, byte[]> media1, KeyValuePair<string, byte[]> media2)
		{

		}
		/// <summary>
		/// scans files and applies the action determined (copy/move/delete)
		/// </summary>
		/// <param name="file">directory acting on</param>
		/// <param name="action">action being do to the files (copy/move/delete)</param>
		private void ScanFiles(string file, string finalDestination, Action<string, string> action, string fileExtension)
		{
			IEnumerable<string> folders = Directory.EnumerateDirectories(file);
			IEnumerable<string> medias = Directory.GetFiles(file, fileExtension);

			foreach (string media in medias)
			{
				string destination = Path.Combine(finalDestination, Path.GetFileName(media));
				TotalMedia++;
				FileInfo mediaData = new FileInfo(media);
				TotalData += mediaData.Length;
				action(media, destination);
			}
			foreach (string folder in folders)
			{
				ScanFiles(folder, finalDestination, action, fileExtension);
			}
		}
		/// <summary>
		/// sets up all the displays on start
		/// </summary>
		private void SetUp()
		{
			//sets the amount labels
			TotalMediaTransferedLabel.Content = $"Total Files Transfered {totalMedia}.";
			TotalDataTransferedLabel.Content = $"Total Bytes Transfered {ByteSizeHelper.DetermineByte(totalData)}.";

			UIHelper.UpdateControlColors(this, white, black, lightGray);

			//populates the combo boxes
			foreach (string type in DataTypeHelper.GetMediaTypes())
			{
				MediaTypeComboBox.Items.Add(type);
			}

			MediaTypeComboBox.SelectedIndex = 0;


		}
		#endregion
		#region properties
		/// <summary>
		/// total files in amount
		/// </summary>
		public double TotalMedia
		{
			get
			{
				return totalMedia;
			}
			set
			{
				totalMedia = value;
				TotalMediaTransferedLabel.Content = $"Total Files Transfered {totalMedia}.";
			}
		}
		/// <summary>
		/// total files in bytes
		/// </summary>
		public long TotalData
		{
			get
			{
				return totalData;
			}
			set
			{
				totalData = value;
				TotalDataTransferedLabel.Content = $"Total Bytes Transfered {ByteSizeHelper.DetermineByte(totalData)}.";
			}
		}
		#endregion
	}
}