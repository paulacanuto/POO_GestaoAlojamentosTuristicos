using System;
using System.Drawing;
using System.Windows.Forms;
using POO_GestaoAlojamentosTuristicos.Business;

namespace POO_GestaoAlojamentosTuristicos.UI
{
    /// <summary>
    /// Formulário de login da aplicação
    /// Credenciais: Usuário = Ana, Senha = AP1234
    /// </summary>
    public partial class FormLogin : Form
    {
        private readonly Logger logger;
        private int tentativasRestantes = 3;

        // Controles
        private Label lblTitulo;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblSenha;
        private TextBox txtSenha;
        private Button btnEntrar;
        private Button btnCancelar;
        private Label lblTentativas;
        private PictureBox picIcone;

        public FormLogin()
        {
            logger = Logger.Instancia;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Login - Sistema de Gestão de Alojamentos";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            // Título
            lblTitulo = new Label
            {
                Text = "SISTEMA DE GESTÃO\nDE ALOJAMENTOS TURÍSTICOS",
                Location = new Point(50, 30),
                Size = new Size(350, 50),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Ícone decorativo (simulado com label)
            picIcone = new PictureBox
            {
                Location = new Point(195, 90),
                Size = new Size(60, 60),
                BackColor = Color.FromArgb(41, 128, 185),
                SizeMode = PictureBoxSizeMode.CenterImage
            };

            // Label "🏨" simulado
            var lblIcone = new Label
            {
                Text = "🏨",
                Location = new Point(0, 0),
                Size = new Size(60, 60),
                Font = new Font("Segoe UI Emoji", 24),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                Parent = picIcone
            };

            // Usuário
            lblUsuario = new Label
            {
                Text = "Usuário:",
                Location = new Point(80, 170),
                Size = new Size(80, 20),
                Font = new Font("Arial", 10, FontStyle.Regular)
            };

            txtUsuario = new TextBox
            {
                Location = new Point(170, 168),
                Size = new Size(200, 25),
                Font = new Font("Arial", 10),
                MaxLength = 50
            };

            // Senha
            lblSenha = new Label
            {
                Text = "Senha:",
                Location = new Point(80, 210),
                Size = new Size(80, 20),
                Font = new Font("Arial", 10, FontStyle.Regular)
            };

            txtSenha = new TextBox
            {
                Location = new Point(170, 208),
                Size = new Size(200, 25),
                Font = new Font("Arial", 10),
                MaxLength = 50,
                PasswordChar = '●',
                UseSystemPasswordChar = true
            };

            // Label tentativas
            lblTentativas = new Label
            {
                Text = "",
                Location = new Point(80, 240),
                Size = new Size(290, 20),
                Font = new Font("Arial", 9, FontStyle.Italic),
                ForeColor = Color.DarkRed,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Botão Entrar
            btnEntrar = new Button
            {
                Text = "Entrar",
                Location = new Point(140, 270),
                Size = new Size(100, 35),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEntrar.FlatAppearance.BorderSize = 0;
            btnEntrar.Click += BtnEntrar_Click;

            // Botão Cancelar
            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(250, 270),
                Size = new Size(100, 35),
                Font = new Font("Arial", 10, FontStyle.Regular),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelar_Click;

            // Adiciona controles ao form
            this.Controls.AddRange(new Control[]
            {
                lblTitulo,
                picIcone,
                lblUsuario, txtUsuario,
                lblSenha, txtSenha,
                lblTentativas,
                btnEntrar, btnCancelar
            });

            // Evento KeyPress para Enter
            txtSenha.KeyPress += TxtSenha_KeyPress;

            // Define foco inicial
            this.ActiveControl = txtUsuario;

            // Evento ao carregar
            this.Load += FormLogin_Load;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            logger.Info("Tela de login carregada");
        }

        private void TxtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Se pressionar Enter, faz login
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                BtnEntrar_Click(sender, e);
            }
        }

        private void BtnEntrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string senha = txtSenha.Text;

            // Valida campos vazios
            if (string.IsNullOrWhiteSpace(usuario))
            {
                MessageBox.Show("Por favor, digite o usuário.", "Campo Obrigatório",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por favor, digite a senha.", "Campo Obrigatório",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenha.Focus();
                return;
            }

            // Verifica credenciais
            if (ValidarCredenciais(usuario, senha))
            {
                logger.Info($"Login bem-sucedido: usuário '{usuario}'");

                MessageBox.Show($"Bem-vindo(a), {usuario}!", "Login Bem-Sucedido",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                tentativasRestantes--;
                logger.Aviso($"Tentativa de login falhou: usuário '{usuario}'. Tentativas restantes: {tentativasRestantes}");

                if (tentativasRestantes > 0)
                {
                    lblTentativas.Text = $"Credenciais inválidas. {tentativasRestantes} tentativa(s) restante(s).";

                    MessageBox.Show("Usuário ou senha incorretos!", "Erro de Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    txtSenha.Clear();
                    txtUsuario.Focus();
                }
                else
                {
                    logger.Erro("Número máximo de tentativas de login excedido");

                    MessageBox.Show("Número máximo de tentativas excedido.\nA aplicação será encerrada.",
                        "Acesso Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    this.DialogResult = DialogResult.Cancel;
                    Application.Exit();
                }
            }
        }

        private bool ValidarCredenciais(string usuario, string senha)
        {
            // Credenciais fixas: Usuário = Ana, Senha = AP1234
            return usuario.Equals("Ana", StringComparison.Ordinal) &&
                   senha.Equals("AP1234", StringComparison.Ordinal);
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            logger.Info("Login cancelado pelo usuário");

            var resultado = MessageBox.Show("Deseja realmente sair?", "Confirmar Saída",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                Application.Exit();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Se fechar sem fazer login, cancela
            if (this.DialogResult != DialogResult.OK && this.DialogResult != DialogResult.Cancel)
            {
                var resultado = MessageBox.Show("Deseja realmente sair?", "Confirmar Saída",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Application.Exit();
                }
            }

            base.OnFormClosing(e);
        }
    }
}
