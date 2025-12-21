using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using POO_GestaoAlojamentosTuristicos.Business;
using POO_GestaoAlojamentosTuristicos.Models;

namespace POO_GestaoAlojamentosTuristicos.UI
{
    public partial class MainForm : Form
    {
        private readonly AlojamentoService alojamentoService;
        private readonly ClienteService clienteService;
        private readonly Logger logger;

        // Controles da interface
        private TabControl tabControl;
        private TabPage tabAlojamentos;
        private TabPage tabClientes;
        private TabPage tabEstatisticas;

        // Tab Alojamentos
        private ListBox lstAlojamentos;
        private Button btnAdicionarHotel;
        private Button btnAdicionarApartamento;
        private Button btnRemoverAlojamento;
        private Button btnDetalhesAlojamento;
        private TextBox txtBuscarAlojamento;
        private Button btnBuscar;

        // Tab Clientes
        private ListBox lstClientes;
        private Button btnAdicionarCliente;
        private Button btnRemoverCliente;
        private Button btnEditarCliente;
        private TextBox txtBuscarCliente;
        private Button btnBuscarCliente;

        // Tab Estatísticas
        private TextBox txtEstatisticas;
        private Button btnAtualizarEstatisticas;

        public MainForm()
        {
            alojamentoService = new AlojamentoService();
            clienteService = new ClienteService();
            logger = Logger.Instancia;

            logger.Info("Aplicação iniciada");

            InitializeComponent();
            ConfigurarInterface();
            CarregarDados();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tabControl = new TabControl();
            tabAlojamentos = new TabPage();
            tabClientes = new TabPage();
            tabEstatisticas = new TabPage();
            tabControl.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabAlojamentos);
            tabControl.Controls.Add(tabClientes);
            tabControl.Controls.Add(tabEstatisticas);
            tabControl.Location = new Point(32, 35);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(852, 484);
            tabControl.TabIndex = 0;
            // 
            // tabAlojamentos
            // 
            tabAlojamentos.Location = new Point(4, 24);
            tabAlojamentos.Name = "tabAlojamentos";
            tabAlojamentos.Size = new Size(844, 456);
            tabAlojamentos.TabIndex = 0;
            tabAlojamentos.Text = "Alojamentos";
            // 
            // tabClientes
            // 
            tabClientes.Location = new Point(4, 24);
            tabClientes.Name = "tabClientes";
            tabClientes.Size = new Size(844, 456);
            tabClientes.TabIndex = 1;
            tabClientes.Text = "Clientes";
            // 
            // tabEstatisticas
            // 
            tabEstatisticas.Location = new Point(4, 24);
            tabEstatisticas.Name = "tabEstatisticas";
            tabEstatisticas.Size = new Size(844, 456);
            tabEstatisticas.TabIndex = 2;
            tabEstatisticas.Text = "Estatísticas";
            // 
            // MainForm
            // 
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(918, 549);
            Controls.Add(tabControl);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sistema de Gestão de Alojamentos Turísticos";
            tabControl.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void ConfigurarInterface()
        {
            ConfigurarTabAlojamentos();
            ConfigurarTabClientes();
            ConfigurarTabEstatisticas();
        }

        private void ConfigurarTabAlojamentos()
        {
            // ListBox
            lstAlojamentos = new ListBox
            {
                Location = new Point(20, 50),
                Size = new Size(600, 400),
                Font = new Font("Consolas", 9)
            };

            // Busca
            var lblBuscar = new Label { Text = "Buscar:", Location = new Point(20, 20), AutoSize = true };
            txtBuscarAlojamento = new TextBox { Location = new Point(70, 17), Size = new Size(300, 25) };
            btnBuscar = new Button { Text = "Buscar", Location = new Point(380, 15), Size = new Size(80, 30) };
            btnBuscar.Click += BtnBuscar_Click;

            // Botões
            btnAdicionarHotel = new Button { Text = "Adicionar Hotel", Location = new Point(640, 50), Size = new Size(200, 40) };
            btnAdicionarHotel.Click += BtnAdicionarHotel_Click;

            btnAdicionarApartamento = new Button { Text = "Adicionar Apartamento", Location = new Point(640, 100), Size = new Size(200, 40) };
            btnAdicionarApartamento.Click += BtnAdicionarApartamento_Click;

            btnDetalhesAlojamento = new Button { Text = "Ver Detalhes", Location = new Point(640, 150), Size = new Size(200, 40) };
            btnDetalhesAlojamento.Click += BtnDetalhesAlojamento_Click;

            btnRemoverAlojamento = new Button { Text = "Remover", Location = new Point(640, 200), Size = new Size(200, 40), BackColor = Color.LightCoral };
            btnRemoverAlojamento.Click += BtnRemoverAlojamento_Click;

            tabAlojamentos.Controls.AddRange(new Control[]
            {
                lblBuscar, txtBuscarAlojamento, btnBuscar,
                lstAlojamentos, btnAdicionarHotel, btnAdicionarApartamento,
                btnDetalhesAlojamento, btnRemoverAlojamento
            });
        }

        private void ConfigurarTabClientes()
        {
            lstClientes = new ListBox { Location = new Point(20, 50), Size = new Size(600, 400), Font = new Font("Consolas", 9) };

            var lblBuscar = new Label { Text = "Buscar:", Location = new Point(20, 20), AutoSize = true };
            txtBuscarCliente = new TextBox { Location = new Point(70, 17), Size = new Size(300, 25) };
            btnBuscarCliente = new Button { Text = "Buscar", Location = new Point(380, 15), Size = new Size(80, 30) };
            btnBuscarCliente.Click += BtnBuscarCliente_Click;

            btnAdicionarCliente = new Button { Text = "Adicionar Cliente", Location = new Point(640, 50), Size = new Size(200, 40) };
            btnAdicionarCliente.Click += BtnAdicionarCliente_Click;

            btnEditarCliente = new Button { Text = "Editar", Location = new Point(640, 100), Size = new Size(200, 40) };
            btnEditarCliente.Click += BtnEditarCliente_Click;

            btnRemoverCliente = new Button { Text = "Remover", Location = new Point(640, 150), Size = new Size(200, 40), BackColor = Color.LightCoral };
            btnRemoverCliente.Click += BtnRemoverCliente_Click;

            tabClientes.Controls.AddRange(new Control[]
            {
                lblBuscar, txtBuscarCliente, btnBuscarCliente,
                lstClientes, btnAdicionarCliente, btnEditarCliente, btnRemoverCliente
            });
        }

        private void ConfigurarTabEstatisticas()
        {
            txtEstatisticas = new TextBox { Location = new Point(20, 60), Size = new Size(820, 380), Multiline = true, ReadOnly = true, Font = new Font("Consolas", 10), BackColor = Color.WhiteSmoke };
            btnAtualizarEstatisticas = new Button { Text = "Atualizar Estatísticas", Location = new Point(20, 20), Size = new Size(200, 35) };
            btnAtualizarEstatisticas.Click += BtnAtualizarEstatisticas_Click;
            tabEstatisticas.Controls.AddRange(new Control[] { btnAtualizarEstatisticas, txtEstatisticas });
        }

        private void CarregarDados()
        {
            CarregarAlojamentos();
            CarregarClientes();
        }

        private void CarregarAlojamentos()
        {
            lstAlojamentos.Items.Clear();
            foreach (var a in alojamentoService.ListarTodos())
                lstAlojamentos.Items.Add(a);
        }

        private void CarregarClientes()
        {
            lstClientes.Items.Clear();
            foreach (var c in clienteService.ListarTodos())
                lstClientes.Items.Add(c);
        }

        // ===== Eventos Alojamentos =====
        private void BtnAdicionarHotel_Click(object sender, EventArgs e)
        {
            var form = new FormAdicionarHotel(alojamentoService);
            if (form.ShowDialog() == DialogResult.OK) CarregarAlojamentos();
        }

        private void BtnAdicionarApartamento_Click(object sender, EventArgs e)
        {
            var form = new FormAdicionarApartamento(alojamentoService);
            if (form.ShowDialog() == DialogResult.OK) CarregarAlojamentos();
        }

        private void BtnRemoverAlojamento_Click(object sender, EventArgs e)
        {
            if (lstAlojamentos.SelectedItem == null) return;

            var a = (Alojamento)lstAlojamentos.SelectedItem;
            if (MessageBox.Show("Remover alojamento?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                alojamentoService.Remover(a.Id);
                CarregarAlojamentos();
            }
        }

        private void BtnDetalhesAlojamento_Click(object sender, EventArgs e)
        {
            if (lstAlojamentos.SelectedItem == null) return;
            var a = (Alojamento)lstAlojamentos.SelectedItem;
            MessageBox.Show(a.ObterDetalhes(), "Detalhes", MessageBoxButtons.OK);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string termo = txtBuscarAlojamento.Text.Trim();
            lstAlojamentos.Items.Clear();

            var resultados = string.IsNullOrWhiteSpace(termo)
                ? alojamentoService.ListarTodos()
                : alojamentoService.BuscarPorEndereco(termo);

            foreach (var a in resultados)
                lstAlojamentos.Items.Add(a);
        }

        // ===== Eventos Clientes =====
        private void BtnAdicionarCliente_Click(object sender, EventArgs e)
        {
            var form = new FormAdicionarCliente(clienteService);
            if (form.ShowDialog() == DialogResult.OK) CarregarClientes();
        }

        private void BtnEditarCliente_Click(object sender, EventArgs e)
        {
            if (lstClientes.SelectedItem == null) return;
            var c = (POO_GestaoAlojamentosTuristicos.Models.Cliente)lstClientes.SelectedItem;
            var form = new FormEditarCliente(clienteService, c);
            if (form.ShowDialog() == DialogResult.OK) CarregarClientes();
        }

        private void BtnRemoverCliente_Click(object sender, EventArgs e)
        {
            if (lstClientes.SelectedItem == null) return;
            var c = (POO_GestaoAlojamentosTuristicos.Models.Cliente)lstClientes.SelectedItem;
            if (MessageBox.Show("Remover cliente?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                clienteService.Remover(c.Id);
                CarregarClientes();
            }
        }

        private void BtnBuscarCliente_Click(object sender, EventArgs e)
        {
            string termo = txtBuscarCliente.Text.Trim();
            lstClientes.Items.Clear();

            var resultados = string.IsNullOrWhiteSpace(termo)
                ? clienteService.ListarTodos()
                : clienteService.BuscarPorNome(termo);

            foreach (var c in resultados)
                lstClientes.Items.Add(c);
        }

        // ===== Estatísticas =====
        private void BtnAtualizarEstatisticas_Click(object sender, EventArgs e)
        {
            var stats = alojamentoService.ObterEstatisticas();
            var totalClientes = clienteService.ContarTotal();

            txtEstatisticas.Text = $"Total de alojamentos: {stats["Total"]}\r\n";
            txtEstatisticas.Text += $"Hotéis: {stats["Hoteis"]}\r\n";
            txtEstatisticas.Text += $"Apartamentos: {stats["Apartamentos"]}\r\n";
            txtEstatisticas.Text += $"Preço médio: {stats["PrecoMedio"]:F2}\r\n";
            txtEstatisticas.Text += $"Preço mínimo: {stats["PrecoMinimo"]:F2}\r\n";
            txtEstatisticas.Text += $"Preço máximo: {stats["PrecoMaximo"]:F2}\r\n";
            txtEstatisticas.Text += $"Total de clientes: {totalClientes}\r\n";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            logger.Info("Aplicação encerrada");
            base.OnFormClosing(e);
        }
    }
}
