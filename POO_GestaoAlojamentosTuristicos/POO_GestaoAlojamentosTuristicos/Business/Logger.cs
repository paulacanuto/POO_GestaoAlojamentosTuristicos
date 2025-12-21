using System;
using System.IO;

namespace POO_GestaoAlojamentosTuristicos.Business
{
    /// <summary>
    /// Sistema simples de logging
    /// Pattern: Singleton
    /// </summary>
    public sealed class Logger
    {
        private static Logger instancia;
        private static readonly object lockObj = new object();
        private readonly string caminhoLog;

        // Construtor privado (Singleton)
        private Logger()
        {
            string pastaLogs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

            if (!Directory.Exists(pastaLogs))
                Directory.CreateDirectory(pastaLogs);

            string nomeArquivo = $"log_{DateTime.Now:yyyy-MM-dd}.txt";
            caminhoLog = Path.Combine(pastaLogs, nomeArquivo);
        }

        /// <summary>
        /// Obtém instância única do Logger (Singleton)
        /// Thread-safe usando double-check locking
        /// </summary>
        public static Logger Instancia
        {
            get
            {
                if (instancia == null)
                {
                    lock (lockObj)
                    {
                        if (instancia == null)
                            instancia = new Logger();
                    }
                }
                return instancia;
            }
        }

        /// <summary>
        /// Registra mensagem informativa
        /// </summary>
        public void Info(string mensagem)
        {
            Escrever("INFO", mensagem);
        }

        /// <summary>
        /// Registra aviso
        /// </summary>
        public void Aviso(string mensagem)
        {
            Escrever("AVISO", mensagem);
        }

        /// <summary>
        /// Registra erro
        /// </summary>
        public void Erro(string mensagem, Exception ex = null)
        {
            string mensagemCompleta = mensagem;

            if (ex != null)
                mensagemCompleta += $"\nExceção: {ex.Message}\nStackTrace: {ex.StackTrace}";

            Escrever("ERRO", mensagemCompleta);
        }

        /// <summary>
        /// Escreve no arquivo de log
        /// Thread-safe
        /// </summary>
        private void Escrever(string nivel, string mensagem)
        {
            lock (lockObj)
            {
                try
                {
                    string entrada = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{nivel}] {mensagem}";
                    File.AppendAllText(caminhoLog, entrada + Environment.NewLine);
                }
                catch
                {
                    // Se falhar ao escrever log, ignora silenciosamente
                    // para não interromper execução do programa
                }
            }
        }

        /// <summary>
        /// Limpa logs antigos (mantém últimos 7 dias)
        /// </summary>
        public void LimparLogsAntigos()
        {
            try
            {
                string pastaLogs = Path.GetDirectoryName(caminhoLog);
                var arquivos = Directory.GetFiles(pastaLogs, "log_*.txt");

                foreach (var arquivo in arquivos)
                {
                    var dataArquivo = File.GetCreationTime(arquivo);

                    if ((DateTime.Now - dataArquivo).TotalDays > 7)
                        File.Delete(arquivo);
                }
            }
            catch
            {
                // Ignora erros ao limpar logs
            }
        }
    }
}
