using System.ComponentModel;

namespace Ovr.BlazorApp.StatusGloblal
{
    public class AppState
    {
        private string _pageTitle = "Ópticas Vista Real";

        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                if (_pageTitle != value)
                {
                    _pageTitle = value;
                    NotifyStateChanged();
                }
            }
        }

        public event Func<Task>? OnChange;

        private async void NotifyStateChanged()
        {
            if (OnChange != null)
            {
                await OnChange.Invoke();
            }
        }
    }
}
