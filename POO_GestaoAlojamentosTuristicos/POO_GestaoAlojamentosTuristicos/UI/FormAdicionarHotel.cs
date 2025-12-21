using System;
using System.Windows.Forms;
using POO_GestaoAlojamentosTuristicos.Business;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.UI
{
    /// <summary>
    /// Formulário para adicionar novo hotel
    /// </summary>
    public partial class FormAdicionarHotel : Form
    {
        private readonly AlojamentoService alojamentoService;
        private readonly Logger logger;

        // Controles
        private Label lblNome;
        private TextBox txtNome;
        private Label lblEndereco;
        private TextBox txtEndereco;
        private Label lblPreco;
        private NumericUpDown numPreco;
        private Label lblEstrelas;
        private NumericUpDown numEstrelas;
        private Button btnSalvar;
        private Button btnCancelar;

        public FormAdicionarHotel(AlojamentoService service)
        {
            alojamentoService = service;
            logger = Logger.Instancia;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Adicionar Hotel";
            this.Size = new System.Drawing.Size(450, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Nome
            lblNome = new Label
            {
                Text = "Nome:",
                Location = new System.Drawing.Point(30, 20),
                Size = new System.Drawing.Size(100, 20)
            };

            txtNome = new TextBox
            {
                Location = new System.Drawing.Point(140, 18),
                Size = new System.Drawing.Size(260, 25)
            };

            // Endereço
            lblEndereco = new Label
            {
                Text = "Endereço:",
                Location = new System.Drawing.Point(30, 55),
                Size = new System.Drawing.Size(100, 20)
            };

            txtEndereco = new TextBox
            {
                Location = new System.Drawing.Point(140, 53),
                Size = new System.Drawing.Size(260, 25)
            };

            // Preço
            lblPreco = new Label
            {
                Text = "Preço/Noite (€):",
                Location = new System.Drawing.Point(30, 90),
                Size = new System.Drawing.Size(100, 20)
            };

            numPreco = new NumericUpDown
            {
                Location = new System.Drawing.Point(140, 88),
                Size = new System.Drawing.Size(120, 25),
                Minimum = 0,
                Maximum = 10000,
                DecimalPlaces = 2,
                Value = 50
            };

            // Estrelas
            lblEstrelas = new Label
            {
                Text = "Estrelas (1-5):",
                Location = new System.Drawing.Point(30, 125),
                Size = new System.Drawing.Size(100, 20)
            };

            numEstrelas = new NumericUpDown
            {
                Location = new System.Drawing.Point(140, 123),
                Size = new System.Drawing.Size(80, 25),
                Minimum = 1,
                Maximum = 5,
                Value = 3
            };

            // Botões
            btnSalvar = new Button
            {
                Text = "Salvar",
                Location = new System.Drawing.Point(140, 180),
                Size = new System.Drawing.Size(100, 35),
                DialogResult = DialogResult.OK
            };
            btnSalvar.Click += BtnSalvar_Click;

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(250, 180),
                Size = new System.Drawing.Size(100, 35),
                DialogResult = DialogResult.Cancel
            };

            // Adiciona controles
            this.Controls.AddRange(new Control[]
            {
        lblNome, txtNome,
        lblEndereco, txtEndereco,
        lblPreco, numPreco,
        lblEstrelas, numEstrelas,
        btnSalvar, btnCancelar
            });

            this.AcceptButton = btnSalvar;
            this.CancelButton = btnCancelar;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validação básica
                if (string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    MessageBox.Show("O nome é obrigatório.", "Validação",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNome.Focus();
                    this.DialogResult = DialogResult.None;
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtEndereco.Text))
                {
                    MessageBox.Show("O endereço é obrigatório.", "Validação",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEndereco.Focus();
                    this.DialogResult = DialogResult.None;
                    return;
                }

                // Adiciona hotel
                alojamentoService.AdicionarHotel(
                    txtNome.Text.Trim(),
                    txtEndereco.Text.Trim(),
                    (double)numPreco.Value,
                    (int)numEstrelas.Value
                );

                logger.Info($"Hotel adicionado: {txtNome.Text}");
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
                logger.Erro("Erro ao adicionar hotel", ex);
                MessageBox.Show("Erro ao adicionar hotel: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }
    }
}