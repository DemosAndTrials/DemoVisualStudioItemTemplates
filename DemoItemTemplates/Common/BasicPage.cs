using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DemoItemTemplates.Common
{
    // <summary>
    /// Base page for all views
    /// @createdBy  	    Ros Haitovich, 2014   
    /// @lastModifiedBy  	Ros Haitovich, 22/06/2015
    /// - Navigate extending + remove from backstack
    /// - refactoring
    /// </summary>
    public class BasicPage : Page
    {

        private NavigationHelper navigationHelper;
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets or sets source page for return
        /// </summary>
        public Page SourcePage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool InEditMode
        {
            get { return ViewModels.ViewModels.inEditMode; }
            set
            {
                ViewModels.ViewModels.inEditMode = value;
                if (!value)
                    ViewModels.ViewModels.ClearSaveRequiredHandler();
            }
        }

        public BasicPage()
        {
            // init the navigation helper
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            LoadState(e);
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            SaveState(e);
        }

        protected virtual void LoadState(LoadStateEventArgs e) { }
        protected virtual void SaveState(SaveStateEventArgs e) { }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        public void NavigateBack(Frame frame = null)
        {
            this.Navigate(frame: frame);
        }

        /// <summary>
        /// Custom navigation method with notification
        /// Provide Frame only or leave it blank to GoBack
        /// </summary>
        /// <param name="destPageType">The data type of the content to load</param>
        /// <param name="parameter">The navigation parameter to pass to the target page</param>
        /// <param name="frame">The controlling Frame for the Page content (to use GoBack() method)</param>
        /// <param name="forceNavigate">The value that force navigation bypassing InEditMode verifications</param>
        /// <param name="removeFromStack">The value to indicate if page must be removed from navigation history</param>
        public async void Navigate(Type destPageType = null, object parameter = null, Frame frame = null, bool forceNavigate = false, bool removeFromStack = false)
        {
            if (frame == null)
                frame = Frame;
            if (forceNavigate || !InEditMode)
            {
                InEditMode = false;
                if (destPageType == null)
                    TryGoBack(frame);
                else
                    frame.Navigate(destPageType, parameter);
            }
            else
            {
                var result = await CustomMessageBox.ShowAsync("Votre saisie n'a pas été enregistrée. Voulez-vous quitter la page?", "Notification !",MessageBoxButton.SaveDontSave);
                if (result == MessageBoxResult.Yes)
                {
                    if (!await ViewModels.ViewModels.SaveRequired())
                    {
                        //await GlobalPage.Current.ShowDialogAsync(message: "Veuillez remplir les champs obligatoires (*) pour valider votre saisie.", buttonSave: "OK", onlyButtonSave: true);
                    }
                    else
                    {
                        InEditMode = false;
                        if (destPageType == null)
                            TryGoBack(frame);
                        else
                            frame.Navigate(destPageType, parameter);
                    }
                }
                else
                {
                    InEditMode = false;
                    if (destPageType == null)
                        TryGoBack(frame);
                    else
                        frame.Navigate(destPageType, parameter);
                }
            }
            // remove page from backstack
            if (removeFromStack)
            {
                RemovePageFromBackStack(frame);
            }
        }

        /// <summary>
        /// Try go back if possible
        /// </summary>
        private void TryGoBack(Frame frame)
        {
            if (frame.CanGoBack)
                frame.GoBack();
        }

        /// <summary>
        /// Removes current page from navigation history (backstack)
        /// to prevent navigation back to the page (back button)
        /// usually used for create/edit pages 
        /// </summary>
        public void RemovePageFromBackStack(Frame frame)
        {
            if (frame.BackStackDepth > 0)
                frame.BackStack.RemoveAt(frame.BackStackDepth - 1);
        }

        /// <summary>
        /// Removes all navigation history (backstack)
        /// </summary>
        public void ClearBackStack()
        {
            Frame.BackStack.Clear();
        }

        /// <summary>
        /// Highlight text on focus
        /// </summary>
        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt != null && !string.IsNullOrEmpty(txt.Text) && !txt.IsReadOnly)
                txt.SelectAll();
        }

        /// <summary>
        /// Set start year and end year of DatePicker
        /// </summary>
        public void datePicker_Loaded(object sender, RoutedEventArgs e)
        {
            var date = sender as DatePicker;
            if (date != null)
            {
                date.MinYear = new DateTimeOffset(DateTime.Today.AddYears(-1));
                date.MaxYear = new DateTimeOffset(DateTime.Today.AddYears(+3));
            }
        }

        /// <summary>
        /// Event to check if data is numeric (maybe changes in future)
        /// Extra parameter can be provided by control's Tag property
        /// 1) NoTag: Only numbers allowed
        /// 2) Tag: "dot_comma" numbers and dot or comma
        /// </summary>
        protected virtual void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt != null)
            {
                if (txt.Tag != null)
                {
                    switch (txt.Tag.ToString())
                    {
                        case "dot_comma":
                            string temp = txt.Text;
                            bool hasDot = false;
                            foreach (char c in temp)
                            {
                                if (c < '0' || c > '9' || c == '.' || c == ',')
                                {
                                    if ((c == '.' || c == ',') && !hasDot)//
                                    {
                                        hasDot = true;
                                        continue;
                                    }
                                    txt.Text = txt.Text.Remove(txt.SelectionStart - 1, 1);
                                    txt.SelectionStart = txt.Text.Length + 1;
                                }
                            }
                            break;
                        default:// tag empty nothing todo
                            break;
                    }
                }
                else
                {// no tag ,only numbers allowed
                    string temp = txt.Text;
                    foreach (char c in temp)
                    {
                        if (c < '0' || c > '9')
                        {
                            txt.Text = txt.Text.Replace(c.ToString(), "");
                            txt.SelectionStart = txt.Text.Length + 1;
                        }
                    }
                }
            }
        }
    }
}
