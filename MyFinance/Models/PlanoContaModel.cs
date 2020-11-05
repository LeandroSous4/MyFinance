using Microsoft.AspNetCore.Http;
using MyFinance.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Models
{
    public class PlanoContaModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public int Usuario_id { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public PlanoContaModel() { }

        public PlanoContaModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<PlanoContaModel> ListaPlanoConta()
        {
            List<PlanoContaModel> lista = new List<PlanoContaModel>();
            PlanoContaModel item;

            string idUsuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"SELECT IDPLANOCONTA, DESCRICAO, TIPO, USUARIO_ID FROM PLANOCONTA WHERE USUARIO_ID = {idUsuario_Logado}";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new PlanoContaModel();
                item.Id = int.Parse(dt.Rows[i]["IDPLANOCONTA"].ToString());
                item.Descricao = dt.Rows[i]["DESCRICAO"].ToString();
                item.Tipo = dt.Rows[i]["TIPO"].ToString();
                item.Usuario_id = int.Parse(dt.Rows[i]["USUARIO_ID"].ToString());
                lista.Add(item);
            }
            return lista;
        }
        public PlanoContaModel CarregarRegistro(int? id)
        {
            PlanoContaModel item = new PlanoContaModel();
            string idUsuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"SELECT IDPLANOCONTA, DESCRICAO, TIPO, USUARIO_ID FROM PLANOCONTA WHERE USUARIO_ID = {idUsuario_Logado} AND IDPLANOCONTA = {id}";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            item.Id = int.Parse(dt.Rows[0]["IDPLANOCONTA"].ToString());
            item.Descricao = dt.Rows[0]["DESCRICAO"].ToString();
            item.Tipo = dt.Rows[0]["TIPO"].ToString();
            item.Usuario_id = int.Parse(dt.Rows[0]["USUARIO_ID"].ToString());

            return item;
        }
        public void Insert()
        {
            string idUsuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"INSERT INTO PLANOCONTA (DESCRICAO, TIPO, USUARIO_ID) VALUES ('{Descricao}', '{Tipo}', '{idUsuario_Logado}')";
            DAL objDAL = new DAL();
            objDAL.ExecutaComandoSQL(sql);
        }
        public void Update()
        {
            string idUsuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"UPDATE PLANOCONTA SET DESCRICAO = '{Descricao}', TIPO = '{Tipo}' WHERE USUARIO_ID = {idUsuario_Logado} AND IDPLANOCONTA = {Id}";
            DAL objDAL = new DAL();
            objDAL.ExecutaComandoSQL(sql);
        }
        public void Excluir(int id_planoconta)
        {
            new DAL().ExecutaComandoSQL("DELETE FROM PLANOCONTA WHERE IDPLANOCONTA = " + id_planoconta);
        }
    }
}
