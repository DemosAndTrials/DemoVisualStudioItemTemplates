using Windows.UI.Xaml;
using System.Linq;


namespace $rootnamespace$.Views
{
    /// <summary>
    /// Default blank page
    /// @createdBy  	    Ros Haitovich, 31/05/2015
    /// @lastModifiedBy  	$username$,    $$
    /// * refactoring
    /// </summary>
    public sealed partial class $safeitemname$ : BasicPage
    {
        public $safeitemname$()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(LoadStateEventArgs e)
        {
            base.LoadState(e);
        }

        protected override void SaveState(SaveStateEventArgs e)
        {
            base.SaveState(e);
        }

        /// <summary>
        /// Return back to the add products page (1st step)
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateBack();
        }

    }
}
