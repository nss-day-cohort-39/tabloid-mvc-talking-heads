using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly PostRepository _postRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly CommentRepository _commentRepository;
        

        public PostController(IConfiguration config)
        {
            _postRepository = new PostRepository(config);
            _categoryRepository = new CategoryRepository(config);
            _commentRepository = new CommentRepository(config);
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult UserIndex()
        {
            int userId = GetCurrentUserProfileId();
            var posts = _postRepository.GetPostsByUserId(userId);
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var vm = new PostCommentViewModel()
            {
                Post = _postRepository.GetPublishedPostById(id),
                Comments = _commentRepository.GetCommentsByPostId(id)
            };
          
            if (vm.Post == null)
            {
                int userId = GetCurrentUserProfileId();
                vm.Post = _postRepository.GetUserPostById(id, userId);

                if (vm.Post == null)
                {
                    return NotFound();
                }
            }
            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }


        // GET: Delete
        [Authorize]
        public IActionResult Delete(int id)
        {
            int userId = GetCurrentUserProfileId();
            var post = _postRepository.GetPublishedPostById(id);
            if (post.UserProfileId != userId)
            {
                return NotFound();
            }
            else
            {
                return View(post);
            }
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(post);
            }
        }


        //GET: Post Controller
        [Authorize]
        public IActionResult Edit(int id)
        {
           int userId = GetCurrentUserProfileId();
            Post post = _postRepository.GetPublishedPostById(id);
            List<Category> categories = _categoryRepository.GetAll();

            UserEditViewModel vm = new UserEditViewModel()
            {
                Post = post,
                Category = categories
            };
           


            if (vm.Post.UserProfile.Id != userId)
            {
                return NotFound();
            }

            return View(vm);
        }

        //POST: Post Controller

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Post post)
        {
            //The list of categories is neccessary in case you change the value on the edit

            List<Category> categories = _categoryRepository.GetAll();
            UserEditViewModel vm = new UserEditViewModel()

            {
                Post = post,
                Category = categories
              
            };

            try
            {
                //setting the UserProfileId. Doing this here instead of making it hidden in the view
                post.UserProfileId = GetCurrentUserProfileId();
                _postRepository.UpdatePost(post);

                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                return View(vm);
            }
        }
    


        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
