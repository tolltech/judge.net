﻿using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Submit;
using Microsoft.AspNet.Identity;

namespace Judge.Web.Controllers
{
    public class ProblemsController : Controller
    {
        private readonly IProblemsService _problemsService;
        private readonly ISubmitSolutionService _submitSolutionService;
        private int _pageSize = 20;

        public ProblemsController(IProblemsService problemsService, ISubmitSolutionService submitSolutionService)
        {
            _problemsService = problemsService;
            _submitSolutionService = submitSolutionService;
        }


        public ActionResult Index(int? page)
        {
            long? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId<long>();
            }
            var model = _problemsService.GetProblemsList(page ?? 1, _pageSize, userId);
            return View(model);
        }

        public ActionResult Statement(long id)
        {
            var model = _problemsService.GetStatement(id);
            return View(model);
        }

        [HttpGet]
        public PartialViewResult SubmitSolution(long problemId)
        {
            var languages = _submitSolutionService.GetLanguages();
            var model = new SubmitSolutionViewModel { Languages = languages, ProblemId = problemId };
            return PartialView("Submits/_SubmitSolution", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitSolution(SubmitSolutionViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Success = true;
                var userId = User.Identity.GetUserId<long>();
                _submitSolutionService.SubmitSolution(model.ProblemId, model.SelectedLanguage, model.File, userId);
            }
            else
            {
                model.Success = false;
            }
            model.Languages = _submitSolutionService.GetLanguages();

            return PartialView("Submits/_SubmitSolution", model);
        }

        [Authorize]
        public ActionResult Solution(long submitId)
        {
            var userId = User.Identity.GetUserId<long>();

            try
            {
                var model = _submitSolutionService.GetSolution(submitId, userId);
                return View(model);
            }
            catch (AuthenticationException)
            {
                throw new HttpException(401, "AuthenticationException");
            }
        }
    }
}