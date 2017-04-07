using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;


namespace GenericInstaller
{
    public partial class ProgressForm : InstallerForm
    {

        public ManualResetEvent CancellationEvent { get; private set; }
        //Disable X button
        private const int CP_NOCLOSE_BUTTON = 0x200;


        private delegate void UpdateProgressBarLabel(string text);
        private delegate void CatchThreadException(ref Exception e);

        private Exception ThreadException;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public ProgressForm()
        {
            InitializeComponent();
            CancellationEvent = new ManualResetEvent(true); //Handles when user cancels installation manually
            backgroundWorker.RunWorkerAsync(this.InstallLabel.Text);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {

            CancellationEvent.Reset(); //Forces blockage in the InstallProcessor thread once it reaches WaitOne().
            
            if (backgroundWorker.WorkerSupportsCancellation)
            {
                DialogResult dialog = MessageBox.Show("Installation Not Finished. Close It?",
                    "Exit", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    CancellationEvent.Set(); //The Work thread needs to resume so that it can capture the Cancel Command
                    backgroundWorker.CancelAsync();
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.No;
                    CancellationEvent.Set();
                }
            }
        }

        const int wait_interval = 250; //ms
        
        internal void update_progressbar(string status, int percentage)
        {
            this.Invoke(new UpdateProgressBarLabel((string _status) => { this.InstallLabel.Text = _status; }), new object[] { status });   //Lets the child thread modify the UI  
            this.backgroundWorker.ReportProgress(percentage);
        }

        internal void catch_ThreadException(ref Exception e)
        {
            this.Invoke(new CatchThreadException((ref Exception _e) => { this.ThreadException = _e; }), new object[] { e });
        }
        
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
                Thread processingThread = null;
                InstallProcessor installHandler = new InstallProcessor(this);
                processingThread = new Thread(() => installHandler.beginInstallation());
                processingThread.Start();

                while (processingThread.IsAlive && ThreadException == null)
                {
                    if (backgroundWorker.CancellationPending) //will be true when the user replies Yes to cancellation window
                    {
                        e.Cancel = true;
                        processingThread.Abort(); //Nothing unsafe can happen here, because the installprocessor is at the WaitOne stage at this point (guaranteed)
                    }
                    Thread.Sleep(wait_interval);                    
                }
                if (ThreadException != null)
                {
                    throw ThreadException;
                }
        }


        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                DialogResult dialog = MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (dialog == DialogResult.OK) { Application.Exit(); }

            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Installation Cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Installation Done.", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                FinishForm FinishForm = new FinishForm();
                FinishForm.Show();
            }
        }
    }
}
