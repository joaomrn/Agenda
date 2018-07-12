using MyFInance.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyFInance.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe seu Nome!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe seu E-mail!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="O email informado é invalido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe sua Senha!")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Informe sua data de nacimento!")]
        public string Data_Nascimento { get; set; }
        
        /// <summary>
        /// Verifica se usuario esta cadastrado no sistema
        /// </summary>
        /// <returns>usuario cadastrado</returns>
        public bool ValidarLogin()
        {
            string sql = $"SELECT ID, NOME, DATA_NASCIMENTO FROM USUARIO WHERE EMAIL='{Email}' AND SENHA='{Senha}'";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            if(dt != null)
            {
                if(dt.Rows.Count == 1)
                {
                    Id = int.Parse(dt.Rows[0]["ID"].ToString());
                    Nome = dt.Rows[0]["NOME"].ToString();
                    Data_Nascimento = dt.Rows[0]["DATA_NASCIMENTO"].ToString();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Insere os dados de um novo usuario no banco
        /// </summary>
        public void RegistrarUsuario()
        {
            string dataNascimento = DateTime.Parse(Data_Nascimento).ToString("yyyy/MM/dd");
            string sql = $"insert into usuario (Nome, Email, Senha, Data_Nascimento) VALUES ('{Nome}','{Email}','{Senha}','{dataNascimento}')";
            DAL objDAL = new DAL();
            objDAL.ExecutarComandoSql(sql);
        }








    }
}
