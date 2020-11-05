using System;
using Xunit;
using MyFinance.Models;

namespace ProjetoTeste
{
    public class UnitTestModels
    {
        [Fact]
        public void Test1()
        {
            UsuarioModel usuarioModel = new UsuarioModel();
            usuarioModel.Email = "teste@teste.com";
            usuarioModel.Senha = "senha";
            Assert.True(usuarioModel.ValidarLogin());
        }
    }
}
