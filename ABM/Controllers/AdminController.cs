﻿using ABM.Filters;
using ABM.Repository;
using ABM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABM.Controllers
{
    public class AdminController : Controller
    {
        const int administrador = 1;
        private readonly UnitOfWork unit = new UnitOfWork();

        // GET: Admin
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Index()
        {
            return RedirectToAction("Admin", "User");
        }
        [HttpGet]
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult DonationPanel()
        {
            var listButons = from b in unit.AdminRepository.GetDonationButtonList()
                             select new MercadoPagoViewModel()
                             {
                                 Id = b.id,
                                 Amount = b.amount,
                                 Link = b.link
                             };
            if (listButons != null)
            {

                return View(listButons.ToList());
            }
            return View();
        }
        [HttpGet]
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult DonationCreate()
        {
            return View();
        }
        [HttpPost]
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult DonationCreate(ABM.ViewModels.MercadoPagoViewModel mpVieModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var uri = new Uri(mpVieModel.Link);
                    if (mpVieModel != null)
                    {
                        if (mpVieModel.Amount > 0 && uri.Scheme == "https")
                        {
                            if (unit.AdminRepository.GetDonationButtonList().Count() < 4)
                            {
                                unit.AdminRepository.InsertDonationButton(mpVieModel.ToEntity());
                                TempData["Success"] = "El boton de MercadoPago fue guardado con exito.";
                                return RedirectToAction("DonationPanel", "Admin");
                            }
                            throw new Exception("El maximo de botones permitidos es 4!");
                        }
                        throw new Exception("Hubo un error al guardar el botón de MercadoPago, el monto es invalido o la URL no es valida( Ejemplo: https://mpago.la/xxxxxxx)");
                    }
                }
                throw new Exception("Hubo un error al guardar el boton de MercadoPago");
            }
            catch(Exception exception)
            {
                ViewBag.Error = exception.Message;
                return View();
            }
        }

        [HttpGet]
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult DonationEdit(int id)
        {
            try
            {
                var donationButton = unit.AdminRepository.GetByID(id);
                if (donationButton != null)
                {
                    MercadoPagoViewModel viewModel = new MercadoPagoViewModel()
                    {
                        Id = donationButton.id,
                        Amount = donationButton.amount,
                        Link = donationButton.link
                    };

                    return View(viewModel);
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Boton de MercadoPago invalido, se redirigio a la lista.";
                return RedirectToAction("DonationPanel");
            }

        }
        [HttpPost]
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult DonationEdit(MercadoPagoViewModel mpVieModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var uri = new Uri(mpVieModel.Link);
                    if (mpVieModel != null)
                    {
                        if (mpVieModel.Amount > 0 && uri.Scheme == "https" && uri.Host == "mpago.la")
                        {
                            unit.AdminRepository.UpdateDonationButton(mpVieModel.ToEntity());
                            TempData["Success"] = "El proyecto fue editado con exito.";
                            return RedirectToAction("DonationPanel", "Admin");
                        }
                        ViewBag.Error = "Hubo un error al guardar el botón de MercadoPago, el monto es invalido o la URL no es valida";
                        return View();
                    }
                }
                throw new Exception();
            }
            catch (Exception)
            {
                ViewBag.Error = "Hubo un error al guardar el boton de MercadoPago";
                return View();
            }
        }
        
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult DonationDelete(int id)
        {
            try
            {
                if (unit.AdminRepository.GetByID(id) != null)
                {
                    unit.AdminRepository.RemoveDonationButton(id);
                    ModelState.Clear();
                    TempData["Success"] = "El boton de donacion fue eliminado con exito.";
                    return RedirectToAction("DonationPanel", "Admin");
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Hubo un error al eliminar el boton de MercadoPago.";
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult ShowDonationButtons()
        {
            var listButons = from b in unit.AdminRepository.GetDonationButtonList()
                             select new MercadoPagoViewModel()
                             {
                                 Id = b.id,
                                 Amount = b.amount,
                                 Link = b.link
                             };
            return PartialView("ShowDonationButtons", listButons);
        }
    }
}