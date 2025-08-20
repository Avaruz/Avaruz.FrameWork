using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Avaruz.FrameWork.Controls.Win.Wizard
{
    /// <summary>
    /// A wizard is the control added to a form to provide a step by step functionality.
    /// It contains <see cref="WizardPage"/>s in the <see cref="Pages"/> collection, which
    /// are containers for other controls. Only one wizard page is shown at a time in the client
    /// are of the wizard.
    /// </summary>
    [Designer(typeof(Avaruz.FrameWork.Controls.Win.Wizard.WizardDesigner))]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Avaruz.FrameWork.Controls.Win.Wizard.Wizard))]
    public class Wizard : System.Windows.Forms.UserControl
    {
        protected internal System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Panel pnlButtonBright3d;
        private System.Windows.Forms.Panel pnlButtonDark3d;
        protected internal System.Windows.Forms.Button btnBack;
        protected internal System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCancel;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly System.ComponentModel.Container components = null;

        /// <summary>
        /// Wizard control with designer support
        /// </summary>
        public Wizard()
        {
            //Empty collection of Pages
            Pages = new PageCollection(this);

            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        private void Wizard_Load(object sender, System.EventArgs e)
        {
            //Attempt to activate a page
            ActivatePage(0);

            //Can I add my self as default cancel button
            Form form = this.FindForm();
            if (form != null && !DesignMode)
            {
                form.CancelButton = btnCancel;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.pnlButtonBright3d = new System.Windows.Forms.Panel();
            this.pnlButtonDark3d = new System.Windows.Forms.Panel();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlButtons
            //
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Controls.Add(this.pnlButtonBright3d);
            this.pnlButtons.Controls.Add(this.pnlButtonDark3d);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 224);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(444, 48);
            this.pnlButtons.TabIndex = 0;
            //
            // btnCancel
            //
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(356, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += this.BtnCancel_Click;
            //
            // btnNext
            //
            this.btnNext.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnNext.Location = new System.Drawing.Point(272, 12);
            this.btnNext.Name = "btnNext";
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "&Siguiente >";
            this.btnNext.Click += this.BtnNext_Click;
            this.btnNext.MouseDown += this.BtnNext_MouseDown;
            //
            // btnBack
            //
            this.btnBack.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBack.Location = new System.Drawing.Point(196, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "< &Atrás";
            this.btnBack.Click += this.BtnBack_Click;
            this.btnBack.MouseDown += this.BtnBack_MouseDown;
            //
            // pnlButtonBright3d
            //
            this.pnlButtonBright3d.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlButtonBright3d.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtonBright3d.Location = new System.Drawing.Point(0, 1);
            this.pnlButtonBright3d.Name = "pnlButtonBright3d";
            this.pnlButtonBright3d.Size = new System.Drawing.Size(444, 1);
            this.pnlButtonBright3d.TabIndex = 1;
            //
            // pnlButtonDark3d
            //
            this.pnlButtonDark3d.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlButtonDark3d.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtonDark3d.Location = new System.Drawing.Point(0, 0);
            this.pnlButtonDark3d.Name = "pnlButtonDark3d";
            this.pnlButtonDark3d.Size = new System.Drawing.Size(444, 1);
            this.pnlButtonDark3d.TabIndex = 2;
            //
            // Wizard
            //
            this.Controls.Add(this.pnlButtons);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.Name = "Wizard";
            this.Size = new System.Drawing.Size(444, 272);
            this.Load += this.Wizard_Load;
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        /// <summary>
        /// Returns the collection of Pages in the wizard
        /// </summary>
        [Category("Wizard")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PageCollection Pages { get; }

        private WizardPage vActivePage = null;
        /// <summary>
        /// Gets/Sets the activePage in the wizard
        /// </summary>
        [Category("Wizard")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal int PageIndex
        {
            get
            {
                return Pages.IndexOf(vActivePage);
            }
            set
            {
                //Do I have any pages?
                if (Pages.Count == 0)
                {
                    //No then show nothing
                    ActivatePage(-1);
                    return;
                }
                // Validate the page asked for
                if (value < -1 || value >= Pages.Count)
                {
                    throw new ArgumentOutOfRangeException("PageIndex",
                        value,
                        "The page index must be between 0 and " + Convert.ToString(Pages.Count - 1)
                        );
                }
                //Select the new page
                ActivatePage(value);
            }
        }

        /// <summary>
        /// Alternative way of getting/Setiing  the current page by using wizardPage objects
        /// </summary>
        public WizardPage Page
        {
            get
            {
                return vActivePage;
            }
            //Dont use this anymore, see Next, Back, NextTo and BackTo
            //			set
            //			{
            //				ActivatePage(value);
            //			}
        }


        protected internal void ActivatePage(int index)
        {
            //If the new page is invalid
            if (index < 0 || index >= Pages.Count)
            {
                btnNext.Enabled = false;
                btnBack.Enabled = false;

                return;
            }


            //Change to the new Page
            WizardPage tWizPage = Pages[index];

            //Really activate the page
            ActivatePage(tWizPage);
        }

        protected internal void ActivatePage(WizardPage page)
        {
            //Deactivate the current
            if (vActivePage != null)
            {
                vActivePage.Visible = false;
            }


            //Activate the new page
            vActivePage = page;

            if (vActivePage != null)
            {
                //Ensure that this panel displays inside the wizard
                vActivePage.Parent = this;
                if (!this.Contains(vActivePage))
                {
                    this.Container.Add(vActivePage);
                }
                //Make it fill the space
                vActivePage.Dock = DockStyle.Fill;
                vActivePage.Visible = true;
                vActivePage.BringToFront();
                vActivePage.FocusFirstTabIndex();
            }

            //What should the back button say
            btnBack.Enabled = this.PageIndex > 0;

            //What should the Next button say
            if (Pages.IndexOf(vActivePage) < Pages.Count - 1
                && !vActivePage.IsFinishPage)
            {
                btnNext.Text = "&Siguiente>";
                btnNext.Enabled = true;
                //Don't close the wizard :)
                btnNext.DialogResult = DialogResult.None;
            }
            else
            {
                btnNext.Text = "&Finalizar";
                //Dont allow a finish in designer
                if (DesignMode && Pages.IndexOf(vActivePage) == Pages.Count - 1)
                {
                    btnNext.Enabled = false;
                }
                else
                {
                    btnNext.Enabled = true;
                    //If Not in design mode then allow a close
                    btnNext.DialogResult = DialogResult.OK;
                }
            }

            //Cause a refresh
            if (vActivePage != null)
            {
                vActivePage.Invalidate();
            }
            else
            {
                this.Invalidate();
            }
        }


        private void BtnNext_Click(object sender, System.EventArgs e)
        {
            Next();
        }

        private void BtnNext_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (DesignMode)
            {
                Next();
            }
        }

        private void BtnBack_Click(object sender, System.EventArgs e)
        {
            Back();
        }

        private void BtnBack_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (DesignMode)
            {
                Back();
            }
        }

        private void BtnCancel_Click(object sender, System.EventArgs e)
        {
            CancelEventArgs arg = new();

            //Throw the event out to subscribers
            CloseFromCancel?.Invoke(this, arg);
            //If nobody told me to cancel
            if (!arg.Cancel)
            {
                //Then we close the form
                this.FindForm().Close();
            }
        }

        /// <summary>
        /// Gets/Sets the enabled state of the Next button.
        /// </summary>
        [Category("Wizard")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool NextEnabled
        {
            get
            {
                return btnNext.Enabled;
            }
            set
            {
                btnNext.Enabled = value;
            }
        }
        /// <summary>
        /// Gets/Sets the enabled state of the back button.
        /// </summary>
        [Category("Wizard")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool BackEnabled
        {
            get
            {
                return btnBack.Enabled;
            }
            set
            {
                btnBack.Enabled = value;
            }
        }
        /// <summary>
        /// Gets/Sets the enabled state of the cancel button.
        /// </summary>
        [Category("Wizard")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CancelEnabled
        {
            get
            {
                return btnCancel.Enabled;
            }
            set
            {

                btnCancel.Enabled = value;
            }
        }


        /// <summary>
        /// Called when the cancel button is pressed, before the form is closed. Set e.Cancel to true if
        /// you do not wish the cancel to close the wizard.
        /// </summary>
        public event EventHandler<CancelEventArgs> CloseFromCancel;


        /// <summary>
        /// Closes the current page after a <see cref="WizardPage.CloseFromNext"/>, then moves to
        /// the Next page and calls <see cref="WizardPage.ShowFromNext"/>
        /// </summary>
        public void Next()
        {
            Debug.Assert(this.PageIndex >= 0, "Page Index was below 0");
            //Tell the Application I just closed a Page
            int newPage = vActivePage.OnCloseFromNext(this);

            //Did I just press Finish instead of Next
            if (this.PageIndex < Pages.Count - 1
                && (!vActivePage.IsFinishPage || DesignMode))
            {
                //No still going
                ActivatePage(newPage);
                //Tell the application, I have just shown a page
                vActivePage.OnShowFromNext(this);
            }
            else
            {
                Debug.Assert(this.PageIndex < Pages.Count, "Error I've just gone past the finish",
                    "btnNext_Click tried to go to page " + Convert.ToString(this.PageIndex + 1)
                    + ", but I only have " + Convert.ToString(Pages.Count));
                //yep Finish was pressed
                if (!DesignMode)
                {
                    this.ParentForm.Close();
                }
            }
        }

        /// <summary>
        /// Moves to the page given and calls <see cref="WizardPage.ShowFromNext"/>
        /// </summary>
        /// <remarks>Does NOT call <see cref="WizardPage.CloseFromNext"/> on the current page</remarks>
        /// <param name="page"></param>
        public void NextTo(WizardPage page)
        {
            //Since we have a page to go to, then there is no need to validate most of it
            ActivatePage(page);
            //Tell the application, I have just shown a page
            vActivePage.OnShowFromNext(this);
        }

        /// <summary>
        /// Closes the current page after a <see cref="WizardPage.CloseFromBack"/>, then moves to
        /// the previous page and calls <see cref="WizardPage.ShowFromBack"/>
        /// </summary>
        public void Back()
        {
            Debug.Assert(this.PageIndex < Pages.Count, "Page Index was beyond Maximum pages");
            //Can I press back
            Debug.Assert(this.PageIndex > 0 && this.PageIndex < Pages.Count, "Attempted to go back to a page that doesn't exist");
            //Tell the application that I closed a page
            int newPage = vActivePage.OnCloseFromBack(this);

            ActivatePage(newPage);
            //Tell the application I have shown a page
            vActivePage.OnShowFromBack(this);
        }

        /// <summary>
        /// Moves to the page given and calls <see cref="WizardPage.ShowFromBack"/>
        /// </summary>
        /// <remarks>Does NOT call <see cref="WizardPage.CloseFromBack"/> on the current page</remarks>
        /// <param name="page"></param>
        public void BackTo(WizardPage page)
        {
            //Since we have a page to go to, then there is no need to validate most of it
            ActivatePage(page);
            //Tell the application, I have just shown a page
            vActivePage.OnShowFromNext(this);
        }

#if DEBUG
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (DesignMode)
            {
                const string noPagesText = "No wizard pages inside the wizard.";


                SizeF textSize = e.Graphics.MeasureString(noPagesText, this.Font);
                RectangleF layout = new((this.Width - textSize.Width) / 2,
                    (this.pnlButtons.Top - textSize.Height) / 2,
                    textSize.Width, textSize.Height);

                Pen dashPen = (Pen)SystemPens.GrayText.Clone();
                dashPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                e.Graphics.DrawRectangle(dashPen,
                    this.Left + 8, this.Top + 8,
                    this.Width - 17, this.pnlButtons.Top - 17);

                e.Graphics.DrawString(noPagesText, this.Font, new SolidBrush(SystemColors.GrayText), layout);
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (DesignMode)
            {
                this.Invalidate();
            }
        }

#endif


    }
}