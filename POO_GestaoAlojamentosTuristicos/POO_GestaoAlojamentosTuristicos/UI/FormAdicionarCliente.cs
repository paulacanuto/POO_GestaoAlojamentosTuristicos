using System;
using System.Windows.Forms;
using POO_GestaoAlojamentosTuristicos.Business;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.UI
{
    /// <summary>
    /// Formulário para adicionar novo cliente
    /// </summary>
    public partial class FormAdicionarCliente : Form
    {
        private readonly ClienteService clienteService;
        private readonly Logger logger;

        // Controles
        private Label lblNome;
        private TextBox txtNome;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblTelefone;
        private TextBox txtTelefone;
        private Button btnSalvar;
        private Button btnCancelar;

        public FormAdicionarCliente(ClienteService service)
        {
            clienteService = service;
            logger = Logger.Instancia;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Adicionar Cliente";
            this.Size = new System.Drawing.Size(450, 280);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Nome
            lblNome = new Label
            {
                Text = "Nome:",
                Location = new System.Drawing.Point(30, 30),
                Size = new System.Drawing.Size(100, 20)
            };

            txtNome = new TextBox
            {
                Location = new System.Drawing.Point(140, 28),
                Size = new System.Drawing.Size(260, 25)
            };

            // Email
            lblEmail = new Label
            {
                Text = "Email:",
                Location = new System.Drawing.Point(30, 70),
                Size = new System.Drawing.Size(100, 20)
            };

            txtEmail = new TextBox
            {
                Location = new System.Drawing.Point(140, 68),
                Size = new System.Drawing.Size(260, 25)
            };

            // Telefone
            lblTelefone = new Label
            {
                Text = "Telefone (opcional):",
                Location = new System.Drawing.Point(30, 110),
                Size = new System.Drawing.Size(120, 20)
            };

            txtTelefone = new TextBox
            {
                Location = new System.Drawing.Point(140, 108),
                Size = new System.Drawing.Size(180, 25)
            };

            // Botões
            btnSalvar = new Button
            {
                Text = "Salvar",
                Location = new System.Drawing.Point(140, 170),
                Size = new System.Drawing.Size(100, 35),
                DialogResult = DialogResult.OK
            };
            btnSalvar.Click += BtnSalvar_Click;

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(250, 170),
                Size = new System.Drawing.Size(100, 35),
                DialogResult = DialogResult.Cancel
            };

            // Adiciona controles
            this.Controls.AddRange(new Control[]
            {
                lblNome, txtNome,
                lblEmail, txtEmail,
                lblTelefone, txtTelefone,
                btnSalvar, btnCancelar
            });

            this.AcceptButton = btnSalvar;
            this.CancelButton = btnCancelar;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validação simples
                if (string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    MessageBox.Show("O nome é obrigatório.", "Validação",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNome.Focus();
                    this.DialogResult = DialogResult.None;
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("O email é obrigatório.", "Validação",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    this.DialogResult = DialogResult.None;
                    return;
                }

                // Adiciona cliente
                clienteService.Adicionar(
                    txtNome.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtTelefone.Text.Trim()
                );

                logger.Info($"Cliente adicionado: {txtNome.Text}");
            }
            catch (DadosInvalidosException ex)
            {
                logger.Aviso($"Validação falhou: {ex.Message}");
                MessageBox.Show(ex.Message, "Dados Inválidos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
            }
            catch (Exception ex)
            {
                logger.Erro("Erro ao adicionar cliente", ex);
                MessageBox.Show("Erro ao adicionar cliente: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }
    }
}