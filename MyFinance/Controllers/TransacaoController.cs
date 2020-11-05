using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyFinance.Models;

namespace MyFinance.Controllers
{
    public class TransacaoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;

        public TransacaoController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            TransacaoModel objTransacao = new TransacaoModel(HttpContextAccessor);
            ViewBag.ListaTransacao = objTransacao.ListaTransacao();
            return View();
        }
        [HttpPost]
        public IActionResult CadastrarTransacao(TransacaoModel formulario)
        {
            if (ModelState.IsValid)
            {
                if (formulario.Id != 0)
                {
                    formulario.HttpContextAccessor = HttpContextAccessor;
                    formulario.Update();
                    return RedirectToAction("Index");
                }
                else
                {
                    formulario.HttpContextAccessor = HttpContextAccessor;
                    formulario.Insert();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult CadastrarTransacao(int? id)
        {
            if (id != null)
            {
                TransacaoModel objTransacao = new TransacaoModel(HttpContextAccessor);
                ViewBag.Registro = objTransacao.CarregarRegistro(id);
            }
            ViewBag.ListaConta = new ContaModel(HttpContextAccessor).ListaConta();
            ViewBag.ListaPlanoConta = new PlanoContaModel(HttpContextAccessor).ListaPlanoConta();
            return View();
        }
        [HttpGet]
        public IActionResult ExcluirTransacao(int id)
        {
            TransacaoModel objTransacao = new TransacaoModel(HttpContextAccessor);
            objTransacao.Excluir(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [HttpPost]
        public IActionResult Extrato(TransacaoModel formulario)
        {
            formulario.HttpContextAccessor = HttpContextAccessor;
            ViewBag.ListaTransacao = formulario.ListaTransacao();
            ViewBag.ListaConta = new ContaModel(HttpContextAccessor).ListaConta();

            return View();
        }
        public IActionResult Dashboard()
        {
            List<Dashboard> lista = new Dashboard(HttpContextAccessor).RetornarDadosGrafico();
            string valores = "";
            string Cores = "";
            string Labels = "";
            var randon = new Random();

            for (int i = 0; i < lista.Count; i++)
            {
                valores += lista[i].Total.ToString() + ",";
                Labels += "'" + lista[i].PlanoConta.ToString() + "' ,";
                Cores += "'" + String.Format("#{0:x6}", randon.Next(0x1000000)) + "' ,";
            }

            ViewBag.ListaValores = valores;
            ViewBag.ListaCores = Cores;
            ViewBag.ListaLabels = Labels;

            return View();
        }
    }
}