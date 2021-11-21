using Dal;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(EnglishBySongs.Droid.AndroidDatabasePath))]
namespace EnglishBySongs.Droid
{
    public class AndroidDatabasePath : IDatabasePath
    {
        public string GetPath(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
        }
    }
}