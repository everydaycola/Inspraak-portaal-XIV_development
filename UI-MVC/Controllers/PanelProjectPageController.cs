using System.Text.RegularExpressions;
using BL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto.ProjectPage;

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
    private static readonly string[] AllowedFileExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg", ".pdf", ".txt", ".dockx"];

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
        return View("Index", new ProjectPageDto
        {
            Panel = _projectPageManager.GetPanelWithPostsAndSuggestionsAndVotesAndDocuments(panelId),
            CurrentUser = user
        });
    }

    //[Authorize(Roles = "Organisatie,PanelMember")]
    public async Task<IActionResult> Index(Guid? panelId)
    {
        var user = await _userManager.GetUserAsync(User);
        
        if (!panelId.HasValue)
        {
            panelId = _customUserManager.getPanelForUser(user.Id).Id;
        }

        return View(new ProjectPageDto
        {
            Panel = _projectPageManager.GetPanelWithPostsAndSuggestionsAndVotesAndDocuments(panelId.Value),
            CurrentUser = user
        });
    }

    [HttpPost]
    [Authorize(Roles = CustomIdentityConstants.OrganisatieRole)]
    public async Task<IActionResult> AddTextPost(NewTextPostDto newTextPost)
    {
        if (!ModelState.IsValid)
        {
            return await SendBack("addTextModal", newTextPost.PanelId);
        }

        _projectPageManager.AddTextPost(newTextPost.PanelId, newTextPost.Title, newTextPost.Content, newTextPost.VisibleForPanelMember, newTextPost.IsGloballyVisible);
        
        _ = HandleMailSending(newTextPost.InformPeopleViaMail, newTextPost.VisibleForPanelMember, newTextPost.PanelId)
            .ContinueWith(task => 
            {
                if (task.IsFaulted)
                {
                    _logger.Log(LogLevel.Error,task.Exception, "Failed to send email notifications");
                }
            });


        return RedirectToAction("Index", new { newTextPost.PanelId });
    }

    [HttpPost]
    [Authorize(Roles = CustomIdentityConstants.OrganisatieRole)]
    public async Task<IActionResult> AddDocumentPost(NewDocumentPostDto newDocumentPost)
    {
        if (!ModelState.IsValid)
        {
            return await SendBack("addBestandModal", newDocumentPost.PanelId);
        }
        
        //Generate a unique filename
        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(newDocumentPost.File.FileName);
        // Save the file
        await _storageManager.AddFileAsync(uniqueFileName, newDocumentPost.File.ContentType, newDocumentPost.File.OpenReadStream());
        //SAVE META DATA IN DB
        _projectPageManager.AddDocumentPost(newDocumentPost.PanelId, newDocumentPost.Title, uniqueFileName, newDocumentPost.VisibleForPanelMember, newDocumentPost.IsGloballyVisible);
        // handle mail sending
        _ = HandleMailSending(newDocumentPost.InformPeopleViaMail, newDocumentPost.VisibleForPanelMember, newDocumentPost.PanelId)
            .ContinueWith(task => 
            {
                if (task.IsFaulted)
                {
                    _logger.Log(LogLevel.Error,task.Exception, "Failed to send email notifications");
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
            foreach (var member in projectGroupMembers)
            {
                await _sendMailManager.SendSingleMailAsync(member.User.Email,
                    mailSubject, textPart, htmlPart);
                _logger.Log(LogLevel.Information, "Mail succesvol verstuurd!");
            }

            //Breng panelleden op de hoogte wanneer dit zichtbaar is voor hen.
            if (visibleForPanelMember)
            {
                _logger.Log(LogLevel.Information, "Panelmembers op de hoogte brengen.");
                foreach (var panelMember in _panelManager.GetAllPanelMembersForPanel(panelId))
                {
                    if (!panelMember.HasRegistered || !panelMember.Selected) continue;
                    await _sendMailManager.SendSingleMailAsync(panelMember.Email,
                        mailSubject,
                        "Nieuwe post op " + name + " geplaatst",
                        "<h1>Nieuwe post op panel " + name + "</h1>" +
                        $"<p>Gebruik onderstaande link om deze te bekijken</p><a href={baseUrl}/PanelProjectPage?panelId={panelId}>Project pagina bezoeken.</a>");
                    _logger.Log(LogLevel.Information, "Mail succesvol verstuurd!");
                }
            }
        }
    }
    
    
    [HttpPost]
    [Authorize(Roles = CustomIdentityConstants.OrganisatieRole)]
    public async Task<IActionResult> AddVideoPost(NewVideoPostDto newVideoPost)
    {
        if (!ModelState.IsValid)
        {
            return await SendBack("addVideoModal", newVideoPost.PanelId);
        }
        
        if (!string.IsNullOrWhiteSpace(newVideoPost.YoutubeUrl))
        {
            return await AddYoutubeVideoPost(
                newVideoPost.PanelId,
                newVideoPost.Title,
                newVideoPost.YoutubeUrl,
                newVideoPost.VisibleForPanelMember,
                newVideoPost.InformPeopleViaMail,
                newVideoPost.IsGloballyVisible);
        } 
        
        return await AddEmbedVideoPost(
            newVideoPost.PanelId,
            newVideoPost.Title,
            newVideoPost.VideoUrl,
            newVideoPost.VisibleForPanelMember,
            newVideoPost.InformPeopleViaMail,
            newVideoPost.IsGloballyVisible);
            
    }

    private Task<IActionResult> AddEmbedVideoPost(
        Guid panelId,
        string title,
        string videoUrl,
        bool visibleForPanelMember,
        bool informPeopleViaMail,
        bool isGloballyVisible)
    {
        _projectPageManager.AddEmbedVideoPost(
            panelId,
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
                    _logger.Log(LogLevel.Error,task.Exception, "Failed to send email notifications");
                }
            });


        return Task.FromResult<IActionResult>(RedirectToAction("Index", new { panelId }));
    }
    
    private async Task<IActionResult> AddYoutubeVideoPost(
        Guid panelId,
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

            if (match.Success && match.Groups.Count > 1) {
                youtubeId = match.Groups[1].Value;
            }
        }
        
        if (youtubeId == null)
        {
            return await HandleValidationError("Ongeldige YouTube URL. Voer een geldige YouTube video URL in.", "addVideoModal", panelId);
        }

        _projectPageManager.AddYoutubeVideoPost(
            panelId,
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
                    _logger.Log(LogLevel.Error,task.Exception, "Failed to send email notifications");
                }
            });


        return RedirectToAction("Index", new { panelId });
    }

    [HttpPost]
    [Authorize(Roles = CustomIdentityConstants.OrganisatieRole)]
    public async Task<IActionResult> AddWerksessiePost(
        Guid panelId,
        string title,
        DateTime sessionDate,
        string sessionTime,
        bool informPeopleViaMail)
    {
        if (!ModelState.IsValid)
        {
            return await SendBack("addWerksessieModal", panelId);
        }
        
        // Parse the time, throw an error if it fails.
        if (!TimeSpan.TryParse(sessionTime, out var parsedTime))
        {
            return await HandleValidationError("Ongeldig tijdstip.", "addWerksessieModal", panelId);
        }

        // Add the meeting post to the panel
        _projectPageManager.AddMeetingPost(
            panelId, 
            title, 
            TimeZoneInfo.ConvertTimeToUtc(sessionDate.Date + parsedTime), 
            true);

        // Notify panel members if requested
        _ = HandleMailSending(informPeopleViaMail, true, panelId)
            .ContinueWith(task => 
            {
                if (task.IsFaulted)
                {
                    _logger.Log(LogLevel.Error,task.Exception, "Failed to send email notifications");
                }
            });


        return RedirectToAction("Index", new { panelId });
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddSummaryToMeetingPost(Guid panelId, Guid meetingId, IFormFile verslagFile)
    {
        var extension = Path.GetExtension(verslagFile.FileName).ToLowerInvariant();
        if (!AllowedFileExtensions.Contains(extension))
        {
            ModelState.AddModelError("file", "Ongeldig bestandstype. Toegestane types: afbeeldingen, pdf, txt, dockx. Uw bestand is type: " + extension);
            ViewBag.SummaryCreationFailed = true;
            return RedirectToAction("Index", new { panelId });
        }
        
        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(verslagFile.FileName);
        await _storageManager.AddFileAsync(uniqueFileName, verslagFile.ContentType, verslagFile.OpenReadStream());
        _projectPageManager.AddDocumentToPost(meetingId, uniqueFileName);
        return RedirectToAction("Index", new { panelId });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddSuggestionPost(NewSuggestionPostDto suggestionPostDto)
    {
        if (!ModelState.IsValid)
        {
            return await SendBack("addSuggestionModal", suggestionPostDto.PanelId);
        }

        _projectPageManager.AddSuggestionPost(suggestionPostDto.PanelId, suggestionPostDto.Title,
            suggestionPostDto.VisibleForPanelMember);

        _ = HandleMailSending(suggestionPostDto.InformPeopleViaMail, true, suggestionPostDto.PanelId)
            .ContinueWith(task => 
            {
                if (task.IsFaulted)
                {
                    _logger.Log(LogLevel.Error,task.Exception, "Failed to send email notifications");
                }
            });


        return RedirectToAction("Index", new { suggestionPostDto.PanelId });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddSuggestion(Guid panelId, Guid postId, string suggestion)
    {
        if (string.IsNullOrWhiteSpace(suggestion) || suggestion.Length > 1000)
        {
            return RedirectToAction("Index", new { panelId });
        }
        
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var email = "onbekend";
        if (user != null && !string.IsNullOrEmpty(user.Email))
        {
            email = user.Email;
        }
        
        _projectPageManager.AddSuggestionToPost(postId, suggestion, email);

        return RedirectToAction("Index", new { panelId });

    }
}