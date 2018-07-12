using Microsoft.AspNetCore.Http;
using MyFInance.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyFInance.Models
{
    public class ContaModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe  o nome da conta!")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Informe  o saldo da conta!")]
        public double Saldo { get; set; }
        public int Usuario_Id { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public ContaModel()
        {

        }

        private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
        }

        //Recebe o contexto para acesso ás variáveis de sesssão.
        public ContaModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Busca no banco todas as contas
        /// </summary>
        /// <returns>contas no banco</returns>
        public List<ContaModel> ListaConta()
        {
            List<ContaModel> lista = new List<ContaModel>();
            ContaModel item;

            string sql = $"SELECT ID, NOME, SALDO, USUARIO_ID FROM CONTA WHERE USUARIO_ID = {IdUsuarioLogado()}";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new ContaModel();
                item.Id = int.Parse(dt.Rows[i]["ID"].ToString());
                item.Nome = dt.Rows[i]["NOME"].ToString();
                item.Saldo = double.Parse(dt.Rows[i]["Saldo"].ToString());
                item.Usuario_Id = int.Parse(dt.Rows[i]["USUARIO_ID"].ToString());
                lista.Add(item);
            }
            return lista;
        }

        /// <summary>
        /// Insere uma nova conta no banco
        /// </summary>
        public void Insert()
        {
            string sql = $"INSERT INTO CONTA (NOME, SALDO, USUARIO_ID) VALUES('{Nome}','{Saldo}','{IdUsuarioLogado()}')";
            DAL objDAl = new DAL();
            objDAl.ExecutarComandoSql(sql);
        }

        public void Excluir(int id_conta)
        {
            new DAL().ExecutarComandoSql("DELETE FROM CONTA WHERE ID = " + id_conta);
        }











    }
}
