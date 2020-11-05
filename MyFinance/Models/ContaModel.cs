using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MyFinance.Util;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Models
{
    public class ContaModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o nome da conta")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Informe o saldo da conta")]
        public double Saldo { get; set; }
        public int Usuario { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public ContaModel() { }

        public ContaModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<ContaModel> ListaConta()
        {
            List<ContaModel> lista = new List<ContaModel>();
            ContaModel item;

            string idUsuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"SELECT IDCONTA, NOME, SALDO, USUARIO_IDUSUARIO FROM CONTA WHERE USUARIO_IDUSUARIO = {idUsuario_Logado}";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new ContaModel();
                item.Id = int.Parse(dt.Rows[i]["IDCONTA"].ToString());
                item.Nome = dt.Rows[i]["NOME"].ToString();
                item.Saldo = double.Parse(dt.Rows[i]["SALDO"].ToString());
                item.Usuario = int.Parse(dt.Rows[i]["USUARIO_IDUSUARIO"].ToString());
                lista.Add(item);
            }
            return lista;
        }
        public void Insert()
        {
            string idUsuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"INSERT INTO CONTA (NOME, SALDO, USUARIO_IDUSUARIO) VALUES ('{Nome}', '{Saldo}', '{idUsuario_Logado}')";
            DAL objDAL = new DAL();
            objDAL.ExecutaComandoSQL(sql);
        }
        public void Excluir(int id_conta)
        {
            new DAL().ExecutaComandoSQL("DELETE FROM CONTA WHERE IDCONTA = " + id_conta);
        }
    }
}
