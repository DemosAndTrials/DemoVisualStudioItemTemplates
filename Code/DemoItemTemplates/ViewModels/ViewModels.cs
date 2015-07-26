using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoItemTemplates.ViewModels
{
    public static class ViewModels
    {
        public static bool inEditMode = false;
       

        /// <summary>
        /// Event to save data before leave page in EditMode
        /// </summary>
        public delegate Task<bool> SaveRequiredHandler();
        /// <summary>
        /// Will fire when in edit mode, and the user presses Save in the dialog alerting him of navigation from page
        /// </summary>
        public static event SaveRequiredHandler SaveRequiredEvent;
        public async static Task<bool> SaveRequired()
        {
            if (SaveRequiredEvent != null)
                return await SaveRequiredEvent();
            return true;
        }

        public static void ClearSaveRequiredHandler()
        {
            SaveRequiredEvent = null;
        }

        /// <summary>
        /// Load/Update/Refresh all nesessary viewmodels
        /// Used on app loading or syncing
        /// </summary>
        public static void GetData()
        {
        }
    }
}
