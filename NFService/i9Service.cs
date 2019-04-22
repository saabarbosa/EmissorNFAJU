using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NFBusiness;

namespace NFService
{
    public partial class i9Service : ServiceBase
    {
        //private int eventId = 1;
        Timer i9Timer;
        string fileLog = "Log-i9TiService.txt";
        // criando na area de trabalho
        String path = @"c:\temp\Logs\";

        public i9Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //verifica se existe o diretorio
                if (System.IO.Directory.Exists(path) == false)
                    System.IO.Directory.CreateDirectory(path);

                i9Timer = new Timer(new TimerCallback(i9Timer_Tick), null, 10000, 120000); //a cada 1 minuto
                //i9Timer = new Timer(new TimerCallback(i9Timer_Tick), null, 10000, 180000); //Apenas uma vez (para teste depuracao)
                using (StreamWriter vWriter = new StreamWriter(path + fileLog, true))
                {
                    vWriter.WriteLine("Servico Iniciado: " + DateTime.Now.ToString());
                    vWriter.Flush();
                    vWriter.Close();
                }
            }
            catch (Exception ex)
            {

            }

        }

   
        protected override void OnStop()
        {
            try
            {
                using (StreamWriter vWriter = new StreamWriter(path + fileLog, true))
                {
                    vWriter.WriteLine("Servico Parado: " + DateTime.Now.ToString());
                    vWriter.Flush();
                    vWriter.Close();
                }
            }
            catch (Exception ex)
            {

            }

        }


        private void i9Timer_Tick(object sender)
        {

            try
            {
                NotaFiscal nf = new NotaFiscal();
                string msg = nf.Gerar();

                using (StreamWriter vWriter = new StreamWriter(path + fileLog, true))
                {
                    vWriter.WriteLine("i9ti Service: Serviço de Emissão da NF rodou em " + DateTime.Now.ToString() + " - Mensagem de Retorno: " + msg);
                    vWriter.Flush();
                    vWriter.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            finally
            {

            }

 

           // i9eventLog.WriteEntry("i9ti Service: Serviço monitorado.", EventLogEntryType.Information, eventId++);
        }

        private void i9eventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}
