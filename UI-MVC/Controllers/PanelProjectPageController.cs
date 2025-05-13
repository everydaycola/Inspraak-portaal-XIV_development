using System.Text.RegularExpressions;
using BL.Interfaces;
using Domain;
using Domain.CitizenPanel;
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
    private static readonly string[] AllowedFileExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg", ".pdf", ".txt", ".dockx"];

    public PanelProjectPageController(ILogger<PanelProjectPageController> logger, IPanelManager panelManager,
        IStorageManager storageManager, ICustomUserManager customUserManager, UserManager<ApplicationUser> userManager,
        ISendMailManager sendMailManager)
    {
        _logger = logger;
        _panelManager = panelManager;
        _storageManager = storageManager;
        _customUserManager = customUserManager;
        _userManager = userManager;
        _sendMailManager = sendMailManager;
    }
    
    private IActionResult HandleValidationError(string message, string modalName, Guid panelId)
    {
        _logger.Log(LogLevel.Warning, modalName + ": Validation Error: " + message);
        ModelState.AddModelError("", message);
        return SendBack(modalName, panelId);
    }
    
    private IActionResult SendBack(string modalName, Guid panelId)
    {
        ViewBag.OpenModal = modalName;
        return View("Index", new ProjectPageDto
        {
            Panel = _panelManager.GetPanelWithPosts(panelId)
        });
    }

    [Authorize(Roles = "Organisatie,PanelMember")]
    public async Task<IActionResult> Index(Guid? panelId)
    {
        if (!panelId.HasValue)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            panelId = _customUserManager.getPanelForUser(user.Id).Id;
        }

        var panel = _panelManager.GetPanelWithPosts(panelId.Value);
        var projectPageDto = new ProjectPageDto
        {
            Panel = panel,
        };
        return View(projectPageDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddTextPost(TextPostDto textPost)
    {
        if (!ModelState.IsValid)
        {
            return SendBack("addTextModal", textPost.PanelId);
        }

        _panelManager.AddTextPost(textPost.PanelId, textPost.Title, textPost.Content, textPost.VisibleForPanelMember);
        
        _ = HandleMailSending(textPost.InformPeopleViaMail, textPost.VisibleForPanelMember, _panelManager.GetPanelWithPosts(textPost.PanelId));

        return View("Index", new ProjectPageDto
        {
            Panel = _panelManager.GetPanelWithPosts(textPost.PanelId)
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddDocumentPost(DocumentPostDto documentPost)
    {
        if (!ModelState.IsValid)
        {
            return SendBack("addBestandModal", documentPost.PanelId);
        }
        
        //Generate a unique filename
        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(documentPost.File.FileName);
        // Save the file
        await _storageManager.AddFileAsync(uniqueFileName, documentPost.File.ContentType, documentPost.File.OpenReadStream());
        //SAVE META DATA IN DB
        _panelManager.AddDocumentPost(documentPost.PanelId, documentPost.Title, uniqueFileName, documentPost.VisibleForPanelMember);
        // handle mail sending
        _ = HandleMailSending(documentPost.InformPeopleViaMail, documentPost.VisibleForPanelMember, _panelManager.GetPanelWithPosts(documentPost.PanelId));

        return View("Index", new ProjectPageDto
        {
            Panel = _panelManager.GetPanelWithPosts(documentPost.PanelId)
        });
    }

    private async Task HandleMailSending(bool informPeopleViaMail, bool visibleForPanelMember, Panel panel)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        if (informPeopleViaMail)
        {
            //Informeer project group.
            var projectGroupmembers = _panelManager.GetAllPlanningGroupMembersWithIdentityUserForPanel(panel.Id);
            _logger.Log(LogLevel.Information, "Projectgroep op de hoogte brengen.");
            var mailSubject = "Er is een nieuwe post geplaatst op een panel waaraan jij deelneemt!";
            var textPart = "Nieuwe post op" + panel.Name + "geplaatst";
            var htmlPart =
                $"<p>Gebruik onderstaande link om deze te bekijken</p><a href={baseUrl}/PanelProjectPage?panelId={panel.Id}>Project pagina bezoeken.</a>";
            foreach (var member in projectGroupmembers)
            {
                await _sendMailManager.SendSingleMailAsync(member.User.Email,
                    mailSubject, textPart, htmlPart);
                _logger.Log(LogLevel.Information, "Mail succesvol verstuurd!");
            }

            //Breng panelleden op de hoogte wanneer dit zichtbaar is voor hen.
            if (visibleForPanelMember)
            {
                var panelMembers = _panelManager.GetAllPanelMembersForPanel(panel.Id);
                _logger.Log(LogLevel.Information, "Panelmembers op de hoogte brengen.");
                foreach (var panelMember in panelMembers)
                {
                    if (!panelMember.HasRegistered || !panelMember.Selected) continue;
                    await _sendMailManager.SendSingleMailAsync(panelMember.Email,
                        mailSubject,
                        "Nieuwe post op " + panel.Name + " geplaatst",
                        "<h1>Nieuwe post op panel " + panel.Name + "</h1>" +
                        $"<p>Gebruik onderstaande link om deze te bekijken</p><a href={baseUrl}/PanelProjectPage?panelId={panel.Id}>Project pagina bezoeken.</a>");
                    _logger.Log(LogLevel.Information, "Mail succesvol verstuurd!");
                }
            }
        }
    }
    
    
    [HttpPost]
    public async Task<IActionResult> AddVideoPost(VideoPostDto videoPost)
    {
        if (!ModelState.IsValid)
        {
            return SendBack("addVideoModal", videoPost.PanelId);
        }
        
        if (!string.IsNullOrWhiteSpace(videoPost.YoutubeUrl))
        {
            return await AddYoutubeVideoPost(
                videoPost.PanelId,
                videoPost.Title,
                videoPost.YoutubeUrl,
                videoPost.VisibleForPanelMember,
                videoPost.InformPeopleViaMail);
        } 
        
        return await AddEmbedVideoPost(
            videoPost.PanelId,
            videoPost.Title,
            videoPost.VideoUrl,
            videoPost.VisibleForPanelMember,
            videoPost.InformPeopleViaMail);
            
    }

    private async Task<IActionResult> AddEmbedVideoPost(
        Guid panelId,
        string title,
        string videoUrl,
        bool visibleForPanelMember,
        bool informPeopleViaMail)
    {
        _panelManager.AddEmbedVideoPost(
            panelId,
            title,
            videoUrl,
            visibleForPanelMember
        );

        // Send email notifications
        _ = HandleMailSending(informPeopleViaMail, visibleForPanelMember, _panelManager.GetPanelWithPosts(panelId));

        return View("Index", new ProjectPageDto
        {
            Panel = _panelManager.GetPanelWithPosts(panelId)
        });
    }

    private async Task<IActionResult> AddYoutubeVideoPost(
        Guid panelId,
        string title,
        string youtubeUrl,
        bool visibleForPanelMember,
        bool informPeopleViaMail)
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
            return HandleValidationError("Ongeldige YouTube URL. Voer een geldige YouTube video URL in.", "addVideoModal", panelId);
        }

        _panelManager.AddYoutubeVideoPost(
            panelId,
            title,
            youtubeId,
            visibleForPanelMember
        );

        // Send email notifications
        var panelWithPosts = _panelManager.GetPanelWithPosts(panelId);
        _ = HandleMailSending(informPeopleViaMail, visibleForPanelMember, panelWithPosts);

        return View("Index", new ProjectPageDto
        {
            Panel = panelWithPosts
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddWerksessiePost(
        Guid panelId,
        string title,
        DateTime sessionDate,
        string sessionTime,
        bool informPeopleViaMail)
    {
        if (!ModelState.IsValid)
        {
            return SendBack("addWerksessieModal", panelId);
        }
        
        // Check if the sessionTime is valid
        if (!TimeSpan.TryParse(sessionTime, out var parsedTime))
        {
            return HandleValidationError("Ongeldig tijdstip.", "addWerksessieModal", panelId);
        }

        var utcMeetingTime = TimeZoneInfo.ConvertTimeToUtc(sessionDate.Date + parsedTime);

        // Add the meeting post to the panel
        _panelManager.AddMeetingPost(panelId, title, utcMeetingTime, true);
        var updatedPanel = _panelManager.GetPanelWithPosts(panelId);

        // Notify panel members if requested
        _ = HandleMailSending(informPeopleViaMail, true, updatedPanel);

        return View("Index", new ProjectPageDto { Panel = updatedPanel });
    }


    [HttpPost]
    public async Task<IActionResult> AddSummaryToMeetingPost(Guid panelId, Guid meetingId, IFormFile verslagFile)
    {
        var extension = Path.GetExtension(verslagFile.FileName).ToLowerInvariant();
        if (!AllowedFileExtensions.Contains(extension))
        {
            ModelState.AddModelError("file", "Ongeldig bestandstype. Toegestane types: afbeeldingen, pdf, txt, dockx. Uw bestand is type: " + extension);
            ViewBag.SummaryCreationFailed = true;
            return View("Index", new ProjectPageDto { Panel = _panelManager.GetPanelWithPosts(panelId) });
        }
        
        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(verslagFile.FileName);
        await _storageManager.AddFileAsync(uniqueFileName, verslagFile.ContentType, verslagFile.OpenReadStream());
        _panelManager.AddSummaryToMeetingPost(meetingId, uniqueFileName);
        return View("Index", new ProjectPageDto { Panel = _panelManager.GetPanelWithPosts(panelId) });
    }

    // public IActionResult AddSuggestionPost(string title, Guid panelId, bool visibleForPanelMember, bool informPeopleViaMail)
    // {
    //     
    // }
}