using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly CommentRepository _commentRepository;
        private readonly PostRepository _postRepository;

        public CommentsController(IConfiguration config)
        {
            _commentRepository = new CommentRepository(config);
            _postRepository = new PostRepository(config);
        }
        // GET: CommentsController
        public ActionResult CommentsIndex(int id)
        {
            var comments = _commentRepository.GetCommentsByPostId(id);
            return View(comments);
        }

        // GET: CommentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentsController/Create
        public ActionResult Create(int id)
        {
            
            return View();
        }

        // POST: CommentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment, int id)
        {
            try
            {

                comment.UserProfileId = GetCurrentUserProfileId();
                comment.PostId = id;
                _commentRepository.Add(comment);
                return RedirectToAction("CommentsIndex", new { id=id });
            }
            catch
            {
                return View(comment);
            }
        }

        // GET: CommentsController/Edit/5
        public ActionResult Edit(int id)
        {
            var comment = _commentRepository.GetCommentById(id);
            return View(comment);
        }

        // POST: CommentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comment comment)
        {
            try
            {
                _commentRepository.UpdateComment(comment);
                return RedirectToAction("CommentsIndex", new { id=comment.PostId });
            }
            catch
            {
                return View(comment);
            }
        }

        // GET: CommentsController/Delete/5
        public ActionResult Delete(int id)
        {
           
            var comment = _commentRepository.GetCommentById(id);
         
                return View(comment);
            
        }

        // POST: CommentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Comment comment)
        {

            try
            {
                _commentRepository.DeleteComment(id);

                return RedirectToAction("CommentsIndex", new { id = comment.PostId });
            }
            catch
            {
                return View(comment);
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
