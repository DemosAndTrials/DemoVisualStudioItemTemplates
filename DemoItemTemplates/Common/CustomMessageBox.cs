using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace DemoItemTemplates.Common
{

    public class CustomMessageBox
    {
        /// <summary>
        /// CustomMessageBox using prepared message
        /// </summary>
        /// <param name="text">MessageBoxText - prepared messages</param>
        /// <param name="caption">Caption of MessageBox</param>
        /// <param name="button">Button to display</param>
        /// <returns>MessageBoxResult</returns>
        public static async Task<MessageBoxResult> ShowAsync(MessageBoxText text, string caption, MessageBoxButton button)
        {
            return await ShowAsync(text.Value, caption, button);
        }

        /// <summary>
        /// CustomMessageBox using custom message
        /// </summary>
        /// <param name="text">MessageBoxText - prepared messages</param>
        /// <param name="caption">Caption of MessageBox</param>
        /// <param name="button">Button to display</param>
        /// <returns>MessageBoxResult</returns>
        public static async Task<MessageBoxResult> ShowAsync(string messageBoxText, string caption, MessageBoxButton button)
        {
            MessageDialog md = new MessageDialog(messageBoxText, caption);
            MessageBoxResult result = MessageBoxResult.None;
            if (button.HasFlag(MessageBoxButton.OK))
            {
                md.Commands.Add(new UICommand("OK",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.OK)));
            }
            if (button.HasFlag(MessageBoxButton.Yes))
            {
                md.Commands.Add(new UICommand("Oui",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.Yes)));
            }
            if (button.HasFlag(MessageBoxButton.No))
            {
                md.Commands.Add(new UICommand("Non",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.No)));
            }
            if (button.HasFlag(MessageBoxButton.Cancel))
            {
                md.Commands.Add(new UICommand("ANNULER",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.Cancel)));
                md.CancelCommandIndex = (uint)md.Commands.Count - 1;
            }
            if (button.HasFlag(MessageBoxButton.Save))
            {
                md.Commands.Add(new UICommand("Enregistrer",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.Yes)));
            }
            if (button.HasFlag(MessageBoxButton.DontSave))
            {
                md.Commands.Add(new UICommand("Ne pas enregistrer",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.No)));
                md.CancelCommandIndex = (uint)md.Commands.Count - 1;
            }
            var op = await md.ShowAsync();
            return result;
        }
        /// <summary>
        /// CustomMessageBox using custom message with OK button w/o caption 
        /// </summary>
        /// <param name="messageBoxText">Custom message</param>
        /// <param name="button">Custom button, default is MessageBoxButton.OK</param>
        /// <returns>MessageBoxResult</returns>
        public static async Task<MessageBoxResult> ShowAsync(string messageBoxText, MessageBoxButton button = MessageBoxButton.OK)
        {
            return await CustomMessageBox.ShowAsync(messageBoxText, null, button);
        }
    }

    // Summary:
    //     Specifies the buttons to include when you display a message box.
    [Flags]
    public enum MessageBoxButton
    {
        // Summary:
        //     Displays only the OK button.
        OK = 1,
        // Summary:
        //     Displays only the Cancel button.
        Cancel = 2,
        //
        // Summary:
        //     Displays both the OK and Cancel buttons.
        OKCancel = OK | Cancel,
        // Summary:
        //     Displays only the OK button.
        Yes = 4,
        // Summary:
        //     Displays only the Cancel button.
        No = 8,
        //
        // Summary:
        //     Displays both the OK and Cancel buttons.
        YesNo = Yes | No,
        // Summary:
        //     Displays only the Save button.
        Save = 16,
        // Summary:
        //     Displays only the Cancel button.
        DontSave = 32,
        //
        // Summary:
        //     Displays both the OK and Cancel buttons.
        SaveDontSave = Save | DontSave,

    }

    // Summary:
    //     Represents a user's response to a message box.
    public enum MessageBoxResult
    {
        // Summary:
        //     This value is not currently used.
        None = 0,
        //
        // Summary:
        //     The user clicked the OK button.
        OK = 1,
        //
        // Summary:
        //     The user clicked the Cancel button or pressed ESC.
        Cancel = 2,
        //
        // Summary:
        //     This value is not currently used.
        Yes = 6,
        //
        // Summary:
        //     This value is not currently used.
        No = 7,
    }

    /// <summary>
    /// Prepared messages for MessageDialog
    public sealed class MessageBoxText
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly MessageBoxText OnDelete = new MessageBoxText("Delete");//
        /// <summary>
        /// "Votre saisie n’a pas été enregistrée. Voulez-vous quitter la page ?"
        /// </summary>
        public static readonly MessageBoxText OnLeaveInEditMode = new MessageBoxText("Votre saisie n’a pas été enregistrée. Voulez-vous quitter la page ?");
        /// <summary>
        /// "Veuillez remplir les champs obligatoires (*) pour valider votre saisie ?"
        /// </summary>
        public static readonly MessageBoxText OnSave = new MessageBoxText("Veuillez remplir les champs obligatoires (*) pour valider votre saisie.");
        /// <summary>
        /// 
        /// </summary>
        public static readonly MessageBoxText OnExit = new MessageBoxText("Exit");//

        private MessageBoxText(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}
