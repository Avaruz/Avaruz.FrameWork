using System;

namespace Avaruz.FrameWork.Controls.Win.Wizard
{
    /// <summary>
    /// Delegate definition for handling NextPageEvents
    /// </summary>
    public delegate void PageEventHandler(object sender, PageEventArgs e);

    /// <summary>
    /// Arguments passed to an application when Page is closed in a wizard. The Next page to be displayed
    /// can be changed, by the application, by setting the NextPage to a wizardPage which is part of the
    /// wizard that generated this event.
    /// </summary>
    /// <remarks>
    /// Constructs a new event
    /// </remarks>
    /// <param name="index">The index of the next page in the collection</param>
    /// <param name="pages">Pages in the wizard that are acceptable to be </param>
    public class PageEventArgs(int index, PageCollection pages) : EventArgs()
    {
        private int vPage = index;
        private readonly PageCollection vPages = pages;

        /// <summary>
        /// Gets/Sets the wizard page that will be displayed next. If you set this it must be to a wizardPage from the wizard.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public WizardPage Page
        {
            get
            {
                //Is this a valid page
                return vPage >= 0 && vPage < vPages.Count ? vPages[vPage] : null;
            }
            set
            {
                if (vPages.Contains(value))
                {
                    //If this is a valid page then set it
                    vPage = vPages.IndexOf(value);
                }
                else
                {
                    ArgumentOutOfRangeException argumentOutOfRangeException = new("NextPage", value, "The page you tried to set was not found in the wizard.");
                    throw argumentOutOfRangeException;
                }
            }
        }


        /// <summary>
        /// Gets the index of the page
        /// </summary>
        public int PageIndex
        {
            get
            {
                return vPage;
            }
        }

    }
}
