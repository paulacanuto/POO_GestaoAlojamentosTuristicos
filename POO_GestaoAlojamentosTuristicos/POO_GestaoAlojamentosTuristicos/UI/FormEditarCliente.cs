using System;
using System.Windows.Forms;
using POO_GestaoAlojamentosTuristicos.Business;
using POO_GestaoAlojamentosTuristicos.Models;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.UI
{
    /// <summary>
    /// Formulário para editar cliente existente
    /// </summary>
    public partial class FormEditarCliente : Form
    {
        private readonly ClienteService clienteService;
        private readonly Cliente clienteOriginal;
        private readonly Logger logger;

        // Controles
        private Label lblId;
        private Label lblIdValor;
        private Label lblNome;
        private TextBox txtNome;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblTelefone;
        private TextBox txtTelefone;
        private Button btnSalvar;
        private Button btnCancelar;

        public FormEditarCliente(ClienteService service, Cliente cliente)
        {
            clienteService = service;
            clienteOriginal = cliente;
            logger = Logger.Instancia;

            InitializeComponent();
            CarregarDados();
        }

        private void InitializeComponent()
        {
            this.Text = "Editar Cliente";
            this.Size = new System.Drawing.Size(450, 310);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // ID (apenas leitura)
            lblId = new Label
            {
                Text = "ID:",
                Location = new System.Drawing.Point(30, 30),
                Size = new System.Drawing.Size(100, 20)
            };

            lblIdValor = new Label
            {
                Text = "",
                Location = new System.Drawing.Point(140, 30),
                Size = new System.Drawing.Size(100, 20),
                Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold)
            };

            // Nome
            lblNome = new Label
            {
                Text = "Nome:",
                Location = new System.Drawing.Point(30, 70),
                Size = new System.Drawing.Size(100, 20)
            };

            txtNome = new TextBox
            {
                Location = new System.Drawing.Point(140, 68),
                Size = new System.Drawing.Size(260, 25)
            };

            // Email
            lblEmail = new Label
            {
                Text = "Email:",
                Location = new System.Drawing.Point(30, 110),
                Size = new System.Drawing.Size(100, 20)
            };

            txtEmail = new TextBox
            {
                Location = new System.Drawing.Point(140, 108),
                Size = new System.Drawing.Size(260, 25)
            };

            // Telefone
            lblTelefone = new Label
            {
                Text = "Telefone:",
                Location = new System.Drawing.Point(30, 150),
                Size = new System.Drawing.Size(100, 20)
            };

            txtTelefone = new TextBox
            {
                Location = new System.Drawing.Point(140, 148),
                Size = new System.Drawing.Size(180, 25)
            };

            // Botões
            btnSalvar = new Button
            {
                Text = "Salvar",
                Location = new System.Drawing.Point(140, 210),
                Size = new System.Drawing.Size(100, 35),
                DialogResult = DialogResult.OK
            };
            btnSalvar.Click += BtnSalvar_Click;

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(250, 210),
                Size = new System.Drawing.Size(100, 35),
                DialogResult = DialogResult.Cancel
            };

            // Adiciona controles
            this.Controls.AddRange(new Control[]
            {
                lblId, lblIdValor,
                lblNome, txtNome,
                lblEmail, txtEmail,
                lblTelefone, txtTelefone,
                btnSalvar, btnCancelar
            });

            this.AcceptButton = btnSalvar;
            this.CancelButton = btnCancelar;
        }

        private void CarregarDados()
        {
            lblIdValor.Text = clienteOriginal.Id.ToString();
            txtNome.Text = clienteOriginal.Nome;
            txtEmail.Text = clienteOriginal.Email;
            txtTelefone.Text = clienteOriginal.Telefone;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validações
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

                // Atualiza cliente
                clienteService.Atualizar(
                    clienteOriginal.Id,
                    txtNome.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtTelefone.Text.Trim()
                );

                logger.Info($"Cliente ID {clienteOriginal.Id} atualizado");
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
                logger.Erro("Erro ao atualizar cliente", ex);
                MessageBox.Show("Erro ao atualizar cliente: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }
    }
}