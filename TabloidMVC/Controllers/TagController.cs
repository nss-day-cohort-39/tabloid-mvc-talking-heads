using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{

    [Authorize]
    public class TagController : Controller
    {
        private readonly TagRepository _tagRepository;

        public TagController(IConfiguration config)
        {
            _tagRepository = new TagRepository(config);
        }

        // GET: TagController
        public IActionResult Index()
        {
            var tags = _tagRepository.GetAllTags();
            return View(tags);
        }

        // GET: TagController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: TagController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tag tag)
        {
            try
            {
                _tagRepository.Add(tag);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TagController/Edit/5
        public IActionResult Edit(int id)
        {
            var tag = _tagRepository.GetTagById(id);
            return View(tag);
        }

        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepository.UpdateTag(tag);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(tag);
            }
        }

        // GET: TagController/Delete/5
        public IActionResult Delete(int id)
        {
            var tag = _tagRepository.GetTagById(id);
            return View(tag);
        }

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Tag tag)
        {
            try
            {
                _tagRepository.DeleteTag(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
