namespace NFService
{
    partial class i9Service
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.i9eventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.i9eventLog)).BeginInit();
            // 
            // i9eventLog
            // 
            this.i9eventLog.Log = "Application";
            this.i9eventLog.Source = "i9Ti Service: EventNF";
            this.i9eventLog.EntryWritten += new System.Diagnostics.EntryWrittenEventHandler(this.i9eventLog_EntryWritten);
            // 
            // i9Service
            // 
            this.ServiceName = "i9ti Services: Geração de NF";
            ((System.ComponentModel.ISupportInitialize)(this.i9eventLog)).EndInit();

        }

        #endregion

        private System.Diagnostics.EventLog i9eventLog;
    }
}
