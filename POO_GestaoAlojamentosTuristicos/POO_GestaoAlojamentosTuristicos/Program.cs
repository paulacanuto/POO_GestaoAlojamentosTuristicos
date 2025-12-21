using System;
using System.Windows.Forms;
using POO_GestaoAlojamentosTuristicos.UI;
using POO_GestaoAlojamentosTuristicos.Business;

namespace POO_GestaoAlojamentosTuristicos
{
    /// <summary>
    /// Classe principal da aplicação
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal da aplicação
        /// </summary>
        [STAThread]
        static void Main()
        {
            var logger = Logger.Instancia;

            try
            {
                // Configuração da aplicação
                ApplicationConfiguration.Initialize();

                // Limpa logs antigos ao iniciar
                logger.LimparLogsAntigos();
                logger.Info("=== Sistema de Gestão de Alojamentos Turísticos ===");
                logger.Info("Aplicação iniciando...");

                // Mostra tela de login primeiro
                using (var formLogin = new FormLogin())
                {
                    if (formLogin.ShowDialog() == DialogResult.OK)
                    {
                        // Se login bem-sucedido, abre o sistema
                        logger.Info("Abrindo sistema principal após login bem-sucedido");
                        Application.Run(new MainForm());
                    }
                    else
                    {
                        // Se cancelou ou falhou, encerra
                        logger.Info("Login cancelado ou falhou - aplicação encerrada");
                    }
                }

                logger.Info("Aplicação encerrada normalmente");
            }
            catch (Exception ex)
            {
                // Captura erros não tratados
                logger.Erro("Erro crítico na aplicação", ex);

                MessageBox.Show(
                    $"Ocorreu um erro crítico:\n\n{ex.Message}\n\nVerifique os logs para mais detalhes.",
                    "Erro Fatal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}