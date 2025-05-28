using System.Text.RegularExpressions;
using BL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UI_MVC.Models.Dto.PostDtos;
using UI_MVC.Models.ViewModels;
using UI_MVC.Models.ViewModels.PostViewModels;

namespace UI_MVC.Controllers;

public class PanelProjectPageController : Controller
{
    private readonly ILogger<PanelProjectPageController> _logger;
    private readonly IPanelManager _panelManager;
    private readonly IStorageManager _storageManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICustomUserManager _customUserManager;
    private readonly ISendMailManager _sendMailManager;
    private readonly IPanelProjectPageManager _projectPageManager;

    private static readonly string[] AllowedFileExtensions =
        [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg", ".pdf", ".txt", ".dockx"];

    public PanelProjectPageController(ILogger<PanelProjectPageController> logger, IPanelManager panelManager,
        IStorageManager storageManager, ICustomUserManager customUserManager, UserManager<ApplicationUser> userManager,
        ISendMailManager sendMailManager, IPanelProjectPageManager projectPageManager)
    {
        _logger = logger;
        _panelManager = panelManager;
        _storageManager = storageManager;
        _customUserManager = customUserManager;
        _userManager = userManager;
        _sendMailManager = sendMailManager;
        _projectPageManager = projectPageManager;
    }

    private async Task<IActionResult> HandleValidationError(string message, string modalName, Guid panelId)
    {
        _logger.Log(LogLevel.Warning, modalName + ": Validation Error: " + message);
        ModelState.AddModelError("", message);
        return await SendBack(modalName, panelId);
    }

    private async Task<IActionResult> SendBack(string modalName, Guid panelId)
    {
        ViewBag.OpenModal = modalName;
        var user = await _userManager.GetUserAsync(User);
        return View("Index", new ProjectPageViewModel
        {
            CurrentUser = user,
            Panel = _projectPageManager.GetPanelWithTimeLinesAndPostsAndSuggestionsAndVotesAndDocuments(panelId),
        });
    }
    
    public async Task<IActionResult> Index(Guid? id)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (id.HasValue)
            return View(new ProjectPageViewModel
            {
                CurrentUser = user,
                Panel = _projectPageManager.GetPanelWithTimeLinesAndPostsAndSuggestionsAndVotesAndDocuments(id.Value),
            });
        if (HttpContext.User.IsInRole(CustomIdentityConstants.OrganisatieRole))
        {
            var panels = _panelManager.GetAllPanels().ToList();
            if (panels.Count != 1)
                return RedirectToAction("PanelSelection", "PanelManagement",
                    new { returnAction = "Index", returnController = "PanelProjectPage" });
            var returnAction = "Index";
            var action = string.IsNullOrEmpty(returnAction) ? "Index" : returnAction;
            return RedirectToAction(action, new { id = panels[0].Id });

        }
        id = _customUserManager.getPanelForUser(user.Id).Id;

        return View(new ProjectPageViewModel
        {
            CurrentUser = user,
            Panel = _projectPageManager.GetPanelWithTimeLinesAndPostsAndSuggestionsAndVotesAndDocuments(id.Value),
        });
    }


    [HttpPost]
    [Authorize(Roles = CustomIdentityConstants.OrganisatieRole)]
    public async Task<IActionResult> AddTextPost(NewTextPostDto newTextPost)
    {
        if (!ModelState.IsValid)
        {
            return await SendBack("addTextModal" + newTextPost.TimeLineId, newTextPost.PanelId);
        }

        _projectPageManager.AddTextPost(newTextPost.TimeLineId, newTextPost.Title, newTextPost.Content, newTextPost.VisibleForPanelMember, newTextPost.IsGloballyVisible );
        
        _ = HandleMailSending(newTextPost.InformPeopleViaMail, newTextPost.VisibleForPanelMember, newTextPost.PanelId)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    _logger.Log(LogLevel.Error, task.Exception, "Failed to send email notifications");
                }
            });


        return RedirectToAction("Index", new { newTextPost.PanelId });
    }

    [HttpPost]
    [Authorize(Roles = CustomIdentityConstants.OrganisatieRole)]
    public async Task<IActionResult> AddDocumentPost(NewDocumentPostViewDto newDocumentPost)
    {
        if (!ModelState.IsValid)
        {
            return await SendBack("addBestandModal" + newDocumentPost.TimeLineId, newDocumentPost.PanelId);
        }

        //Generate a unique filename
        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(newDocumentPost.File.FileName);
        // Save the file
        try
        {
             await _storageManager.AddFileAsync(uniqueFileName, newDocumentPost.File.ContentType,
                newDocumentPost.File.OpenReadStream());
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e);
        }
        //SAVE META DATA IN DB
        _projectPageManager.AddDocumentPost(newDocumentPost.TimeLineId, newDocumentPost.Title, uniqueFileName, newDocumentPost.VisibleForPanelMember, newDocumentPost.IsGloballyVisible);
        // handle mail sending
        _ = HandleMailSending(newDocumentPost.InformPeopleViaMail, newDocumentPost.VisibleForPanelMember,
                newDocumentPost.PanelId)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    _logger.Log(LogLevel.Error, task.Exception, "Failed to send email notifications");
                }
            });


        return RedirectToAction("Index", new { newDocumentPost.PanelId });
    }

    private async Task HandleMailSending(bool informPeopleViaMail, bool visibleForPanelMember, Guid panelId)
    {
        var name = _panelManager.GetPanel(panelId).Name;
        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        if (informPeopleViaMail)
        {
            //Informeer project group.
            var projectGroupMembers = _panelManager.GetAllPlanningGroupMembersWithIdentityUserForPanel(panelId);
            _logger.Log(LogLevel.Information, "Projectgroep op de hoogte brengen.");
            const string mailSubject = "Er is een nieuwe post geplaatst op een panel waaraan jij deelneemt!";
            var textPart = "Nieuwe post op" + name + "geplaatst";
            var htmlPart =
                $"<p>Gebruik onderstaande link om deze te bekijken</p><a href={baseUrl}/PanelProjectPage?panelId={panelId}>Project pagina bezoeken.</a>";
            var mails = projectGroupMembers.Select(member => member.User.Email).ToList();

            if (!mails.IsNullOrEmpty())
            {
                await _sendMailManager.SendBulkMails(mails, mailSubject, textPart, htmlPart);
                _logger.Log(LogLevel.Information, "Mail succesvol verstuurd!");
            }

            //Breng panelleden op de hoogte wanneer dit zichtbaar is voor hen.
            if (visibleForPanelMember)
            {
                _logger.Log(LogLevel.Information, "Panelmembers op de hoogte brengen.");

                var panelMemberMails = new List<string>();
                var allPanelMembersForPanel = _panelManager.GetAllPanelMembersForPanel(panelId);
                foreach (var panelMember in allPanelMembersForPanel)
                {
                    if (!panelMember.HasRegistered || !panelMember.Selected) continue;
                    panelMemberMails.Add(panelMember.Email);
                }

                await _sendMailManager.SendBulkMails(panelMemberMails,
                    mailSubject,
                    "Nieuwe post op " + name + " geplaatst",
                    "<h1>Nieuwe post op panel " + name + "</h1>" +
                    $"<p>Gebruik onderstaande link om deze te bekijken</p><a href={baseUrl}/PanelProjectPage?panelId={panelId}>Project pagina bezoeken.</a>");
                _logger.Log(LogLevel.Information, "Mail succesvol verstuurd!");
            }
        }
    }


    [HttpPost]
    [Authorize(Roles = CustomIdentityConstants.OrganisatieRole)]
    public async Task<IActionResult> AddVideoPost(NewVideoPostDto newVideoPost)
    {
        if (!ModelState.IsValid)
        {
            return await SendBack("addVideoModal" + newVideoPost.TimeLineId, newVideoPost.PanelId);
        }

        if (!string.IsNullOrWhiteSpace(newVideoPost.YoutubeUrl))
        {
            return await AddYoutubeVideoPost(
                newVideoPost.PanelId,
                newVideoPost.TimeLineId,
                newVideoPost.Title,
                newVideoPost.YoutubeUrl,
                newVideoPost.VisibleForPanelMember,
                newVideoPost.InformPeopleViaMail,
                newVideoPost.IsGloballyVisible);
        }

        return await AddEmbedVideoPost(
            newVideoPost.PanelId,
            newVideoPost.TimeLineId,
            newVideoPost.Title,
            newVideoPost.VideoUrl,
            newVideoPost.VisibleForPanelMember,
            newVideoPost.InformPeopleViaMail,
            newVideoPost.IsGloballyVisible);
    }

    private Task<IActionResult> AddEmbedVideoPost(
        Guid panelId,
        Guid timeLineId,
        string title,
        string videoUrl,
        bool visibleForPanelMember,
        bool informPeopleViaMail,
        bool isGloballyVisible)
    {
        _projectPageManager.AddEmbedVideoPost(
            timeLineId,
            title,
            videoUrl,
            visibleForPanelMember,
            isGloballyVisible
        );

        // Send email notifications
        _ = HandleMailSending(informPeopleViaMail, visibleForPanelMember, panelId)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    _logger.Log(LogLevel.Error, task.Exception, "Failed to send email notifications");
                }
            });


        return Task.FromResult<IActionResult>(RedirectToAction("Index", new { panelId }));
    }

    private async Task<IActionResult> AddYoutubeVideoPost(
        Guid panelId,
        Guid timeLineId,
        string title,
        string youtubeUrl,
        bool visibleForPanelMember,
        bool informPeopleViaMail,
        bool isGloballyVisible)
    {
        // Prepare video data
        string youtubeId = null;

        // Extract YouTube video ID using regex
        if (!string.IsNullOrWhiteSpace(youtubeUrl))
        {
            var match = new Regex(
                @"(?:youtube(?:-nocookie)?\.com/(?:[^/]+/.+/|(?:v|e(?:mbed)?)/|.*[?&]v=)|youtu\.be/)([^""&?/\s]{11})",
                RegexOptions.IgnoreCase).Match(youtubeUrl);

            if (match.Success && match.Groups.Count > 1)
            {
                youtubeId = match.Groups[1].Value;
            }
        }

        if (youtubeId == null)
        {
            return await HandleValidationError("Ongeldige YouTube URL. Voer een geldige YouTube video URL in.",
                "addVideoModal", panelId);
        }

        _projectPageManager.AddYoutubeVideoPost(
            timeLineId,
            title,
            youtubeId,
            visibleForPanelMember,
            isGloballyVisible
        );

        // Send email notifications
        _ = HandleMailSending(informPeopleViaMail, visibleForPanelMember, panelId)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    _logger.Log(LogLevel.Error, task.Exception, "Failed to send email notifications");
                }
            });


        return RedirectToAction("Index", new { panelId });
    }

    [HttpPost]
    [Authorize(Roles = CustomIdentityConstants.OrganisatieRole)]
    public async Task<IActionResult> AddWerksessiePost(NewWerksessiePostDto werksessiePost)
    {
        var modalName = "addWerksessieModal" + werksessiePost.TimeLineId;
        if (!ModelState.IsValid)
        {
            return await SendBack(modalName, werksessiePost.PanelId);
        }

        // Parse the time, throw an error if it fails.
        if (!TimeSpan.TryParse(werksessiePost.SessionTime, out var parsedTime))
        {
            return await HandleValidationError("Ongeldig tijdstip.", modalName, werksessiePost.PanelId);
        }

        // Add the meeting post to the panel
        _projectPageManager.AddMeetingPost(
            werksessiePost.TimeLineId, 
            werksessiePost.Title, 
            TimeZoneInfo.ConvertTimeToUtc(werksessiePost.SessionDate.Date + parsedTime), 
            true);

        // Notify panel members if requested
        _ = HandleMailSending(werksessiePost.InformPeopleViaMail, true, werksessiePost.PanelId)
            .ContinueWith(task => 
            {
                if (task.IsFaulted)
                {
                    _logger.Log(LogLevel.Error, task.Exception, "Failed to send email notifications");
                }
            });


        return RedirectToAction("Index", new { werksessiePost.PanelId });
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddSummaryToMeetingPost(Guid panelId, Guid meetingId, IFormFile verslagFile)
    {
        var extension = Path.GetExtension(verslagFile.FileName).ToLowerInvariant();
        if (!AllowedFileExtensions.Contains(extension))
        {
            ModelState.AddModelError("file",
                "Ongeldig bestandstype. Toegestane types: afbeeldingen, pdf, txt, dockx. Uw bestand is type: " +
                extension);
            ViewBag.SummaryCreationFailed = true;
            return RedirectToAction("Index", new { panelId });
        }

        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(verslagFile.FileName);
        try
        {
            await _storageManager.AddFileAsync(uniqueFileName, verslagFile.ContentType, verslagFile.OpenReadStream());
                _projectPageManager.AddDocumentToPost(meetingId, uniqueFileName);
        }
        catch (NullReferenceException e)
        {
            _logger.LogError("Failed to add to bucket" + e.Message);
        }
        return RedirectToAction("Index", new { panelId });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddSuggestionPost(NewSuggestionPostDto suggestionPostDto)
    {
        if (!ModelState.IsValid)
        {
            return await SendBack("addSuggestionModal" + suggestionPostDto.TimeLineId, suggestionPostDto.PanelId);
        }

        _projectPageManager.AddSuggestionPost(suggestionPostDto.TimeLineId, suggestionPostDto.Title,
            suggestionPostDto.VisibleForPanelMember, true, suggestionPostDto.VotingMajorityFactor);

        _ = HandleMailSending(suggestionPostDto.InformPeopleViaMail, true, suggestionPostDto.PanelId)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    _logger.Log(LogLevel.Error, task.Exception, "Failed to send email notifications");
                }
            });


        return RedirectToAction("Index", new { suggestionPostDto.PanelId });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddSuggestion(NewSuggestionDto newSuggestion)
    {
        if (!ModelState.IsValid)
        {
            return await SendBack("addSuggestionModal", newSuggestion.PanelId);
        }

        var user = await _userManager.GetUserAsync(HttpContext.User);
        var email = "onbekend";
        if (user != null && !string.IsNullOrEmpty(user.Email))
        {
            email = user.Email;
        }
        
        _projectPageManager.AddSuggestionToPost(newSuggestion.PostId, newSuggestion.Suggestion, email);

        return RedirectToAction("Index", new { newSuggestion.PanelId });

    }

    public async Task<IActionResult> AddTimeLine(NewTimeLineDto newTimeLine)
    {
        if (!ModelState.IsValid)
        {
            return await SendBack("addTimeLineModal", newTimeLine.PanelId);
        }
        
        _projectPageManager.AddTimeLine(
            newTimeLine.PanelId,
            newTimeLine.Title,
            TimeZoneInfo.ConvertTimeToUtc(newTimeLine.SessionDate.Date)
            );
        
        return RedirectToAction("Index", new { newTimeLine.PanelId });
    }

    [HttpPost]
    [Authorize(Roles = CustomIdentityConstants.OrganisatieRole)]
    public async Task<IActionResult> AddGoogleFormEmbed(NewGoogleFormEmbedDto googleFormEmbedDto)
    {
        if (!ModelState.IsValid)
        {
            return await SendBack("addGoogleFormEmbedModal" + googleFormEmbedDto.TimeLineId, googleFormEmbedDto.PanelId);
        }
        
        _projectPageManager.AddGoogleFormLink(
            googleFormEmbedDto.TimeLineId,
            googleFormEmbedDto.Title,
            googleFormEmbedDto.EmbeddedIframeUrl,
            googleFormEmbedDto.VisibleForPanelMember,
            googleFormEmbedDto.IsGloballyVisible
        );
        _ = HandleMailSending(googleFormEmbedDto.InformPeopleViaMail, googleFormEmbedDto.VisibleForPanelMember, googleFormEmbedDto.PanelId)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    _logger.Log(LogLevel.Error, task.Exception, "Failed to send email notifications");
                }
            });

        return RedirectToAction("Index", new { googleFormEmbedDto.PanelId });
    }
}