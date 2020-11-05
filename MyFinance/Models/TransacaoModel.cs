using Microsoft.AspNetCore.Http;
using MyFinance.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Models
{
    public class TransacaoModel
    {
        public int Id { get; set; }
        public string DataTransacao { get; set; }
        public string DataFiltro { get; set; }
        public string Tipo { get; set; }
        public double Valor { get; set; }
        public string Descricao { get; set; }
        public int Conta_id { get; set; }
        public int PlanoConta_id { get; set; }
        public string Conta { get; set; }
        public string PlanoConta { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public TransacaoModel() { }

        public TransacaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<TransacaoModel> ListaTransacao()
        {
            List<TransacaoModel> lista = new List<TransacaoModel>();
            TransacaoModel item;

            //Utilizado para filtrar Extrato
            string filtro = "";
            if(DataTransacao != null && DataFiltro != null)
            {
                filtro += $" AND T.DATATRANSACAO >= '{DateTime.Parse(DataTransacao).ToString("yyyy/MM/dd")}' AND T.DATATRANSACAO <= '{DateTime.Parse(DataFiltro).ToString("yyyy/MM/dd")}'";
            }
            if(Tipo != null)
            {
                if (Tipo != "A")
                {
                    if(Tipo != "T")
                    {
                        filtro += $" AND T.TIPO = '{Tipo}'";
                    }
                }

            }
            if(Conta_id != 0)
            {
                filtro += $" AND T.CONTA_IDCONTA = '{Conta_id}'";
            }
            //Fim do filtro
            string idUsuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "SELECT T.IDTRANSACAO, T.DATATRANSACAO, T.TIPO, T.VALOR, T.DESCRICAO AS HISTORICO, T.CONTA_IDCONTA," +
            "T.PLANOCONTA_IDPLANOCONTA, T.USUARIO_ID, C.NOME AS CONTA, P.DESCRICAO AS PLANO " +
            "FROM TRANSACAO AS T INNER JOIN CONTA AS C ON T.CONTA_IDCONTA = C.IDCONTA " +
            $"INNER JOIN PLANOCONTA AS P ON T.PLANOCONTA_IDPLANOCONTA = P.IDPLANOCONTA WHERE T.USUARIO_ID = {idUsuario_Logado} {filtro}" +
            " ORDER BY T.DATATRANSACAO DESC LIMIT 10";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new TransacaoModel();
                item.Id = int.Parse(dt.Rows[i]["IDTRANSACAO"].ToString());
                item.DataTransacao = DateTime.Parse(dt.Rows[i]["DATATRANSACAO"].ToString()).ToString("dd/MM/yyyy");
                item.Tipo = dt.Rows[i]["TIPO"].ToString();
                item.Valor = double.Parse(dt.Rows[i]["VALOR"].ToString());
                item.Descricao = dt.Rows[i]["HISTORICO"].ToString();
                item.Conta_id = int.Parse(dt.Rows[i]["CONTA_IDCONTA"].ToString());
                item.PlanoConta_id = int.Parse(dt.Rows[i]["PLANOCONTA_IDPLANOCONTA"].ToString());
                item.Conta = dt.Rows[i]["CONTA"].ToString();
                item.PlanoConta = dt.Rows[i]["PLANO"].ToString();
                lista.Add(item);
            }
            return lista;
        }



        public TransacaoModel CarregarRegistro(int? id)
        {
            string idUsuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "SELECT T.IDTRANSACAO, T.DATATRANSACAO, T.TIPO, T.VALOR, T.DESCRICAO AS HISTORICO, T.CONTA_IDCONTA," +
            "T.PLANOCONTA_IDPLANOCONTA, T.USUARIO_ID, C.NOME AS CONTA, P.DESCRICAO AS PLANO " +
            "FROM TRANSACAO AS T INNER JOIN CONTA AS C ON T.CONTA_IDCONTA = C.IDCONTA " +
            $"INNER JOIN PLANOCONTA AS P ON T.PLANOCONTA_IDPLANOCONTA = P.IDPLANOCONTA WHERE T.USUARIO_ID = '{idUsuario_Logado}' AND T.IDTRANSACAO = '{id}'";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            TransacaoModel item = new TransacaoModel();
            item.Id = int.Parse(dt.Rows[0]["IDTRANSACAO"].ToString());
            item.DataTransacao = DateTime.Parse(dt.Rows[0]["DATATRANSACAO"].ToString()).ToString("dd/MM/yyyy");
            item.Tipo = dt.Rows[0]["TIPO"].ToString();
            item.Valor = double.Parse(dt.Rows[0]["VALOR"].ToString());
            item.Descricao = dt.Rows[0]["HISTORICO"].ToString();
            item.Conta_id = int.Parse(dt.Rows[0]["CONTA_IDCONTA"].ToString());
            item.PlanoConta_id = int.Parse(dt.Rows[0]["PLANOCONTA_IDPLANOCONTA"].ToString());
            item.Conta = dt.Rows[0]["CONTA"].ToString();
            item.PlanoConta = dt.Rows[0]["PLANO"].ToString();

            return item;
        }
        public void Insert()
        {
            string idUsuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"INSERT INTO TRANSACAO (DATATRANSACAO, TIPO, VALOR, DESCRICAO, CONTA_IDCONTA, PLANOCONTA_IDPLANOCONTA, USUARIO_ID) VALUES ('{DateTime.Parse(DataTransacao).ToString("yyyy/MM/dd")}', '{Tipo}', '{Valor}', '{Descricao}', '{Conta_id}', '{PlanoConta_id}', '{idUsuario_Logado}')";
            DAL objDAL = new DAL();
            objDAL.ExecutaComandoSQL(sql);
        }
        public void Update()
        {
            string idUsuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"UPDATE TRANSACAO SET DATATRANSACAO='{DateTime.Parse(DataTransacao).ToString("yyyy/MM/dd")}', TIPO = '{Tipo}', VALOR = '{Valor}', DESCRICAO = '{Descricao}', CONTA_IDCONTA = '{Conta_id}', PLANOCONTA_IDPLANOCONTA = '{PlanoConta_id}' WHERE USUARIO_ID = {idUsuario_Logado} AND IDTRANSACAO= {Id}";
            DAL objDAL = new DAL();
            objDAL.ExecutaComandoSQL(sql);
        }
        public void Excluir(int id_Transacao)
        {
            new DAL().ExecutaComandoSQL("DELETE FROM TRANSACAO WHERE IDTRANSACAO = " + id_Transacao);
        }
    }

    public class Dashboard
    {
        public double Total { get; set; }
        public string PlanoConta { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public Dashboard() { }

        public Dashboard(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public List<Dashboard> RetornarDadosGrafico()
        {
            List<Dashboard> Lista = new List<Dashboard>();
            Dashboard item;
            string idUsuario_Logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "SELECT SUM(T.VALOR) AS TOTAL, P.DESCRICAO FROM TRANSACAO AS T INNER JOIN PLANOCONTA AS P " +
                $"ON T.PLANOCONTA_IDPLANOCONTA = P.IDPLANOCONTA WHERE T.TIPO = 'D' AND T.USUARIO_ID = {idUsuario_Logado} GROUP BY P.DESCRICAO";

            DAL objDAL = new DAL();
            DataTable dt = new DataTable();
            dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new Dashboard();
                item.Total = double.Parse(dt.Rows[i]["TOTAL"].ToString());
                item.PlanoConta = dt.Rows[i]["Descricao"].ToString();
                Lista.Add(item);
            }
            return Lista;
        }
    }
}
