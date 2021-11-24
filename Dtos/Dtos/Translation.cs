using EnglishBySongs.ViewModels;
using Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Dtos.Dtos
{
    public class Translation : INotifyPropertyChanged, IModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual ICollection<WordModel> Words { get; set; } = new List<WordModel>();
        public Translation()
        {

        }
        public override string ToString()
        {
            return Text;
        }

        // TODO: remove
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return;

            backingField = value;

            OnPropertyChanged(propertyName);
        }
    }
}
