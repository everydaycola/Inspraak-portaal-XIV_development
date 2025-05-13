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

    private readonly string[] allowedExtensions = new[]
        { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg", ".pdf", ".txt", ".dockx" };

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

    private IActionResult HandleValidationError(string message, Guid panelId, string modalName)
    {
        _logger.Log(LogLevel.Warning, modalName + ": Validation Error: " + message);
        ModelState.AddModelError("", message);
        var panel = _panelManager.GetPanelWithPosts(panelId);
        var projectPageDto = new ProjectPageDto { Panel = panel };
        ViewBag.OpenModal = modalName;
        return View("Index", projectPageDto);
    }

    [Authorize(Roles = "Organisatie,PanelMember")]
    public async Task<IActionResult> Index(Guid? PanelId)
    {
        if (!PanelId.HasValue)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            PanelId = _customUserManager.getPanelForUser(user.Id).Id;
        }

        var panel = _panelManager.GetPanelWithPosts(PanelId.Value);
        var projectPageDto = new ProjectPageDto
        {
            Panel = panel,
        };
        return View(projectPageDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddTextPost(Guid panelId, string title, string content, bool visibleForPanelMember,
        bool informPeopleViaMail)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
        {
            return HandleValidationError("Vul alle verplichte velden correct in.", panelId, "addTextModal");
        }

        _panelManager.AddTextPost(panelId, title, content, visibleForPanelMember);
        var updatedPanel = _panelManager.GetPanelWithPosts(panelId);
        var updatedProjectPageDto = new ProjectPageDto
        {
            Panel = updatedPanel
        };
        await HandleMailSending(informPeopleViaMail, visibleForPanelMember, updatedPanel);

        return View("Index", updatedProjectPageDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddDocumentPost(string title, IFormFile file, Guid panelId,
        bool visibleForPanelMember, bool informPeopleViaMail)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return HandleValidationError("Vul alle verplichte velden correct in.", panelId, "addBestandModal");
        }

        if (file == null || file.Length == 0)
        {
            return HandleValidationError("Geen bestand geselecteerd of leeg bestand.", panelId, "addBestandModal");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            return HandleValidationError("Ongeldig bestandstype. Toegestane types: afbeeldingen, pdf, txt, dockx.",
                panelId, "addBestandModal");
        }

        // TEMP Create a folder path for uploads
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);
        //Generate a unique filename
        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        // TEMP Save the file localy
        await _storageManager.AddFileAsync(uniqueFileName, file.ContentType, file.OpenReadStream());
        //SAVE META DATA IN DB
        _panelManager.AddDocumentPost(panelId, title, uniqueFileName, visibleForPanelMember);
        var updatedPanel = _panelManager.GetPanelWithPosts(panelId);
        var updatedProjectPageDto = new ProjectPageDto
        {
            Panel = updatedPanel
        };
        await HandleMailSending(informPeopleViaMail, visibleForPanelMember, updatedPanel);

        return View("Index", updatedProjectPageDto);
    }

    private async Task HandleMailSending(bool informPeopleViaMail, bool visibleForPanelMember, Panel panel)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        if (informPeopleViaMail)
        {
            //Informeer project group.
            var projectGroupmembers = _panelManager.GetAllPlanningGroupMembersWithIdentityUserForPanel(panel.Id);
            _logger.Log(LogLevel.Information, "Projectgroep op de hoogte brengen.");
            string mailSubject = "Er is een nieuwe post geplaatst op een panel waaraan jij deelneemt!";
            string textPart = "Nieuwe post op" + panel.Name + "geplaats";
            string htmlPart =
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
                    if (panelMember.HasRegistered && panelMember.Selected)
                    {
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
    }
    
    
    [HttpPost]
    public async Task<IActionResult> AddVideoPost(Guid panelId,
        string title,
        string youtubeUrl,
        string videoUrl,
        bool visibleForPanelMember,
        bool informPeopleViaMail)
    {
        if (string.IsNullOrWhiteSpace(youtubeUrl))
        {
            if (string.IsNullOrWhiteSpace(videoUrl))
            {
                return HandleValidationError("Geef een url in.", panelId, "addVideoModal");
            }
            return await AddEmbedVideoPost(panelId, title, videoUrl, visibleForPanelMember, informPeopleViaMail);
        }
        if (string.IsNullOrWhiteSpace(videoUrl))
        {
            return await AddYoutubeVideoPost(panelId, title, youtubeUrl, visibleForPanelMember, informPeopleViaMail);
        }
        return HandleValidationError("U heeft zowel een youtube als video url ingegeven", panelId, "addVideoModal");
    }

    private async Task<IActionResult> AddEmbedVideoPost(
        Guid panelId,
        string title,
        string videoUrl,
        bool visibleForPanelMember,
        bool informPeopleViaMail)
    {
        // Validate URL
        if (!Uri.TryCreate(videoUrl, UriKind.Absolute, out var uriResult) ||
            (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
        {
            return HandleValidationError("Voer een geldige video URL in.", panelId, "addVideoModal");
        }


        _panelManager.AddEmbedVideoPost(
            panelId,
            title,
            videoUrl,
            visibleForPanelMember
        );

        var updatedPanel = _panelManager.GetPanelWithPosts(panelId);
        var updatedProjectPageDto = new ProjectPageDto
        {
            Panel = updatedPanel
        };

        // Send email notifications
        await HandleMailSending(informPeopleViaMail, visibleForPanelMember, updatedPanel);

        return View("Index", updatedProjectPageDto);
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
        if (!string.IsNullOrWhiteSpace(youtubeUrl)) {
            var match = new Regex(
                @"(?:youtube(?:-nocookie)?\.com/(?:[^/]+/.+/|(?:v|e(?:mbed)?)/|.*[?&]v=)|youtu\.be/)([^""&?/\s]{11})", 
                RegexOptions.IgnoreCase).Match(youtubeUrl);

            if (match.Success && match.Groups.Count > 1) {
                youtubeId = match.Groups[1].Value;
            }
        }
        
        if (youtubeId == null) {
            return HandleValidationError("Ongeldige YouTube URL. Voer een geldige YouTube video URL in.", panelId, 
                "addVideoModal");
        }

        _panelManager.AddYoutubeVideoPost(
            panelId,
            title,
            youtubeId,
            visibleForPanelMember
        );

        var updatedPanel = _panelManager.GetPanelWithPosts(panelId);
        var updatedProjectPageDto = new ProjectPageDto
        {
            Panel = updatedPanel
        };

        // Send email notifications
        await HandleMailSending(informPeopleViaMail, visibleForPanelMember, updatedPanel);

        return View("Index", updatedProjectPageDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddWerksessiePost(
        Guid panelId,
        string title,
        DateTime sessionDate,
        string sessionTime,
        bool informPeopleViaMail)
    {
        // Check if all required fields are filled
        if (string.IsNullOrWhiteSpace(title) || sessionDate == default || string.IsNullOrWhiteSpace(sessionTime))
        {
            return HandleValidationError("Vul alle verplichte velden correct in.", panelId, "addWerksessieModal");
        }

        // Check if sessionDate is not in the past
        if (sessionDate < DateTime.Now.Date)
        {
            return HandleValidationError("De datum mag niet in het verleden liggen.", panelId, "addWerksessieModal");
        }

        // Check if the sessionTime is valid
        if (!TimeSpan.TryParse(sessionTime, out var parsedTime))
        {
            return HandleValidationError("Ongeldig tijdstip.", panelId, "addWerksessieModal");
        }

        var meetingDateTime = sessionDate.Date + parsedTime;
        DateTime utcMeetingTime = TimeZoneInfo.ConvertTimeToUtc(meetingDateTime);

        // Add the meeting post to the panel
        _panelManager.AddMeetingPost(panelId, title, utcMeetingTime, true);
        var updatedPanel = _panelManager.GetPanelWithPosts(panelId);
        var updatedProjectPageDto = new ProjectPageDto { Panel = updatedPanel };

        // Notify panel members if requested
        if (informPeopleViaMail)
        {
            var projectGroupmembers = _panelManager.GetAllPlanningGroupMembersWithIdentityUserForPanel(panelId);
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var mailSubject = "Er is een nieuwe werksessie ingepland op een panel waaraan jij deelneemt!";
            var textPart = "Nieuwe serksessie op " + updatedPanel.Name + "ingepland";
            var htmlPart = "<h1>Nieuwe werksessie op panel " + updatedPanel.Name + "</h1>" +
                           $"<p>Gebruik onderstaande link om deze te bekijken</p><a href={baseUrl}/PanelProjectPage?panelId={updatedPanel.Id}>Project pagina bezoeken.</a>";

            foreach (var member in projectGroupmembers)
            {
                await _sendMailManager.SendSingleMailAsync(member.User.Email,
                    mailSubject, textPart, htmlPart);
                _logger.Log(LogLevel.Information, "Mail succesvol verstuurd!");
            }

            var panelMembers = _panelManager.GetAllPanelMembersForPanel(updatedPanel.Id);
            _logger.Log(LogLevel.Information, "Panelmembers op de hoogte brengen.");
            foreach (var panelMember in panelMembers)
            {
                if (panelMember.HasRegistered && panelMember.Selected)
                {
                    await _sendMailManager.SendSingleMailAsync(
                        panelMember.Email,
                        mailSubject,
                        textPart,
                        htmlPart
                    );
                    _logger.Log(LogLevel.Information, "Mail succesvol verstuurd!");
                }
            }
        }

        return View("Index", updatedProjectPageDto);
    }


    [HttpPost]
    public async Task<IActionResult> AddSummaryToMeetingPost(Guid panelId, Guid MeetingId, IFormFile VerslagFile)
    {
        if (VerslagFile == null || VerslagFile.Length == 0)
        {
            ModelState.AddModelError("VerslagFile", "Geen bestand geselecteerd.");
            return RedirectToAction("Index");
        }

        var extension = Path.GetExtension(VerslagFile.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError("file", "Ongeldig bestandstype. Toegestane types: afbeeldingen, pdf, txt, dockx.");
            var panel = _panelManager.GetPanelWithPosts(panelId);
            var projectPageDto = new ProjectPageDto { Panel = panel };
            ViewBag.SummaryCreationFailed = true;
            return View("Index", projectPageDto);
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);
        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(VerslagFile.FileName);
        await _storageManager.AddFileAsync(uniqueFileName, VerslagFile.ContentType, VerslagFile.OpenReadStream());
        _panelManager.AddSummaryToMeetingPost(MeetingId, uniqueFileName);
        var updatedPanel = _panelManager.GetPanelWithPosts(panelId);
        var updatedProjectPageDto = new ProjectPageDto { Panel = updatedPanel };
        return View("Index", updatedProjectPageDto);
    }
}