using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using POO_GestaoAlojamentosTuristicos.Exceptions;
using POO_GestaoAlojamentosTuristicos.Interfaces;

namespace POO_GestaoAlojamentosTuristicos.Data
{
    /// <summary>
    /// Repositório genérico com persistência em JSON
    /// Pattern: Repository + Template Method
    /// </summary>
    public abstract class RepositorioBase<T> where T : class
    {
        protected List<T> entidades;
        protected readonly string caminhoFicheiro;

        protected RepositorioBase(string nomeFicheiro)
        {
            entidades = new List<T>();

            string pastaData = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(pastaData))
                Directory.CreateDirectory(pastaData);

            caminhoFicheiro = Path.Combine(pastaData, nomeFicheiro);
        }

        public virtual bool Adicionar(T entidade)
        {
            if (entidade == null)
                throw new DadosInvalidosException("entidade", "A entidade não pode ser nula.");

            entidades.Add(entidade);
            return true;
        }

        public virtual List<T> ObterTodos()
        {
            return new List<T>(entidades);
        }

        public int Contar() => entidades.Count;

        public virtual bool Guardar()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(entidades, options);
                File.WriteAllText(caminhoFicheiro, json);
                return true;
            }
            catch (Exception ex)
            {
                throw new PersistenciaException($"Erro ao guardar dados em {caminhoFicheiro}", ex);
            }
        }

        public virtual bool Carregar()
        {
            try
            {
                if (!File.Exists(caminhoFicheiro))
                    return false;

                string json = File.ReadAllText(caminhoFicheiro);
                if (string.IsNullOrWhiteSpace(json))
                    return false;

                entidades = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                return true;
            }
            catch (Exception ex)
            {
                throw new PersistenciaException($"Erro ao carregar dados de {caminhoFicheiro}", ex);
            }
        }

        public abstract T ObterPorId(int id);
        public abstract bool Atualizar(T entidade);
        public abstract bool Remover(int id);
    }
}