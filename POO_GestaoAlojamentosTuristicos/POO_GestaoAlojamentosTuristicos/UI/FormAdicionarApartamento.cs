using System;
using System.Windows.Forms;
using POO_GestaoAlojamentosTuristicos.Business;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.UI
{
    /// <summary>
    /// Formulário para adicionar novo apartamento
    /// </summary>
    public partial class FormAdicionarApartamento : Form
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
        private Label lblQuartos;
        private NumericUpDown numQuartos;
        private CheckBox chkGaragem;
        private Button btnSalvar;
        private Button btnCancelar;

        public FormAdicionarApartamento(AlojamentoService service)
        {
            alojamentoService = service;
            logger = Logger.Instancia;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Adicionar Apartamento";
            this.Size = new System.Drawing.Size(450, 340);
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

            // Quartos
            lblQuartos = new Label
            {
                Text = "Nº de Quartos:",
                Location = new System.Drawing.Point(30, 125),
                Size = new System.Drawing.Size(100, 20)
            };

            numQuartos = new NumericUpDown
            {
                Location = new System.Drawing.Point(140, 123),
                Size = new System.Drawing.Size(80, 25),
                Minimum = 1,
                Maximum = 10,
                Value = 2
            };

            // Garagem
            chkGaragem = new CheckBox
            {
                Text = "Possui garagem",
                Location = new System.Drawing.Point(140, 158),
                Size = new System.Drawing.Size(150, 25)
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
                lblNome, txtNome,
                lblEndereco, txtEndereco,
                lblPreco, numPreco,
                lblQuartos, numQuartos,
                chkGaragem,
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

                // Adiciona apartamento
                alojamentoService.AdicionarApartamento(
                    txtNome.Text.Trim(),
                    txtEndereco.Text.Trim(),
                    (double)numPreco.Value,
                    (int)numQuartos.Value,
                    chkGaragem.Checked
                );

                logger.Info($"Apartamento adicionado: {txtNome.Text}");
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
                logger.Erro("Erro ao adicionar apartamento", ex);
                MessageBox.Show("Erro ao adicionar apartamento: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }
    }
}