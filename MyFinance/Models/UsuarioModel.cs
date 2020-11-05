using MyFinance.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome inválido")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "E-mail inválido")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "O email informado é inválido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha inválida")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Data de nascimento inválida")]
        public string Nascimento { get; set; }

        public bool ValidarLogin()
        {
            string sql = $"SELECT IDUSUARIO, NOME, NASCIMENTO FROM USUARIO WHERE EMAIL='{Email}' AND SENHA='{Senha}'";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            if(dt != null)
            {
                if(dt.Rows.Count == 1)
                {
                    Id = int.Parse(dt.Rows[0]["IDUSUARIO"].ToString());
                    Nome = dt.Rows[0]["NOME"].ToString();
                    Nascimento = dt.Rows[0]["NASCIMENTO"].ToString();
                    return true;
                }
            }

            return false;
        }
        public void RegistrarUsuario()
        {
            string dataNascimento = DateTime.Parse(Nascimento).ToString("yyyy/MM/dd");
            string sql = $"INSERT INTO USUARIO (NOME, EMAIL, SENHA, NASCIMENTO) VALUES ('{Nome}','{Email}','{Senha}','{dataNascimento}')";
            DAL objDAL = new DAL();
            objDAL.ExecutaComandoSQL(sql);
        }
    }
}
