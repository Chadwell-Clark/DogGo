using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Controllers
{
    public class WalksController : Controller
    {

        private readonly IWalksRepository _walksRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly IDogRepository _dogRepo;

        public WalksController(
            IWalksRepository walksRepository,
            IWalkerRepository walkerRepository,
            IDogRepository dogRepository)
        {
            _walksRepo = walksRepository;
            _walkerRepo = walkerRepository;
            _dogRepo = dogRepository;

        }
        // GET: WalksController
        public ActionResult Index()
        {
            List<Walks> walks = _walksRepo.GetAllWalks();

            return View(walks);
        }

        // GET: WalksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WalksController/Create
        public ActionResult Create()
        {

            List<Walker> walkers = _walkerRepo.GetAllWalkers();
            List<Dog> dogs = _dogRepo.GetAllDogs();
            DateTime now =  DateTime.Now;
            WalksFormViewModel vm = new WalksFormViewModel()

            {
                Walks = new Walks(),
                Walkers = walkers,
                Dogs = dogs,
                DateTime = now
            };
            return View(vm);
        }

        // POST: WalksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WalksFormViewModel walksForm )
        {
            try
            {
                foreach (var dogId in walksForm.MultiSelectDogs)
                {
                    walksForm.Walks.DogId = dogId;
                _walksRepo.AddWalks(walksForm.Walks);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: WalksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalksController/Delete/5
        public ActionResult Delete()
        {
            List<Walks> walks = _walksRepo.GetAllWalks();
            WalksDeleteViewModel vm = new WalksDeleteViewModel()
            {
                Walks = walks
            };

            return View(vm);
        }

        // GET: WalksController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(WalksDeleteViewModel walksDelete)
        {
            try
            {
                foreach (var walkId in walksDelete.WalksToDelete)
                {
                    _walksRepo.DeleteWalks(walkId);
                
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }
    }
}



