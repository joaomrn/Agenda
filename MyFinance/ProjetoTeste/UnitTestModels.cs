using System;
using Xunit;
using MyFInance.Models;

namespace ProjetoTeste
{
    public class UnitTestModels
    {
        [Fact]
        public void TestLoginUsuario()
        {
            UsuarioModel usuarioModel = new UsuarioModel();
            usuarioModel.Email = "fe@gmail.com";
            usuarioModel.Senha = "123";
            bool result = usuarioModel.ValidarLogin();
            Assert.True(result);
            
        }

        [Fact]
        public void TestRegistrarUsuario()
        {
            UsuarioModel usuarioModel = new UsuarioModel();
            usuarioModel.Nome = "Teste";
            usuarioModel.Data_Nascimento = "1991/06/24";
            usuarioModel.Email = "carlos@gmail.com.br";
            usuarioModel.Senha = "4321";
            usuarioModel.RegistrarUsuario();
            bool result = usuarioModel.ValidarLogin();
            Assert.True(result);

        }
    }
}
